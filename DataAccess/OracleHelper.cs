using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Models.Common;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace DataAccess
{
    public static class OracleHelper
    {
        internal static Dictionary<string, OracleParameter[]> m_CachedParameters;

        static OracleHelper()
        {
            m_CachedParameters = new Dictionary<string, OracleParameter[]>();
        }
        private static void AssignParameters(OracleCommand comm, params object[] values)
        {
            try
            {
                DiscoveryParameters(comm);
                // assign value
                var index = 0;
                foreach (OracleParameter param in comm.Parameters)
                {
                    if (param.OracleDbType == OracleDbType.RefCursor)
                    {
                        param.Direction = ParameterDirection.Output;
                    }
                    else if (param.Direction == ParameterDirection.Input || param.Direction == ParameterDirection.InputOutput)
                    {
                        if (values[index] == null || (values[index] is string && (string)values[index] == string.Empty))
                        {
                            param.Value = DBNull.Value;
                        }
                        else if (param.OracleDbType == OracleDbType.NClob)
                        {
                            var lob = new OracleClob(comm.Connection);
                            var buffer = Encoding.Unicode.GetBytes(values[index].ToString());
                            lob.Write(buffer, 0, buffer.Length);

                            param.Value = lob;
                        }
                        else
                        {
                            switch (param.OracleDbType)
                            {
                                case OracleDbType.Date:
                                    //param.Value = Convert.ToDateTime(values[index], Culture);
                                    //break;
                                case OracleDbType.Byte:
                                case OracleDbType.Int16:
                                case OracleDbType.Int32:
                                case OracleDbType.Int64:
                                case OracleDbType.Single:
                                case OracleDbType.Double:
                                case OracleDbType.Decimal:
                                    //param.Value = Convert.ToDecimal(values[index], App.Environment.ServerInfo.Culture);
                                    //break;
                                default:
                                    param.Value = values[index];
                                    break;
                            }
                        }
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ErrorUtils.CreateErrorWithSubMessage(
                //    ERR_SQL.ERR_SQL_ASSIGN_PARAMS_FAIL, ex.Message,
                //    comm.CommandText, values);
            }
        }

        public static List<T> ExecuteStoreProcedureGeneric<T>(string connectionString, string commandText, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ex;
                    //throw ErrorUtils.CreateErrorWithSubMessage(
                    //    ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message,
                    //    commandText, values);
                }

                try
                {
                    var comm = new OracleCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
                    AssignParameters(comm, values);

                    using (var dr = comm.ExecuteReader())
                    {
                        if (
                            comm.Parameters.Contains(Constants.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                            comm.Parameters[Constants.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                            )
                        {
                            var errCode = int.Parse(comm.Parameters[Constants.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                            //if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                        }
                        var list = new List<T>();
                        while (dr.Read())
                        {
                            list.Add((T)Convert.ChangeType(dr.GetValue(0), typeof(T)));
                        }
                        return list;
                    }
                }
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
                }
                
                catch (Exception ex)
                {
                    throw ex;
                    //throw ErrorUtils.CreateErrorWithSubMessage(
                    //    ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message,
                    //    commandText, values);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static List<T> ExecuteStoreProcedure<T>(string connectionString, string commandText, params object[] values)
            where T : class, new()
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ex;
                    //throw ErrorUtils.CreateErrorWithSubMessage(
                    //    ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message,
                    //    commandText, values);
                }

                using (var comm = new OracleCommand(commandText, conn))
                {
                    try
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, values);

                        using (var dr = comm.ExecuteReader())
                        {
                            if (
                                comm.Parameters.Contains(Constants.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                                comm.Parameters[Constants.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                                )
                            {
                                var errCode = int.Parse(comm.Parameters[Constants.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                                //if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                            }
                            return dr.ToList<T>();
                        }
                    }
                    catch (OracleException ex)
                    {
                        throw ThrowOracleUserException(ex, commandText);
                    }
                   
                    catch (Exception ex)
                    {
                        throw ex;
                        //throw ErrorUtils.CreateErrorWithSubMessage(
                        //    ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message,
                        //    commandText, values);
                    }
                    finally
                    {
                        if (comm.Parameters.Contains(Constants.ORACLE_EXCEPTION_PARAMETER_NAME))
                        {
                            var value = comm.Parameters[Constants.ORACLE_EXCEPTION_PARAMETER_NAME].Value;
                            if (value != DBNull.Value)
                            {
                                // throw ErrorUtils.CreateError(int.Parse(value.ToString()));
                            }
                        }

                        conn.Close();
                    }
                }
            }
        }

        public static void DiscoveryParameters(OracleCommand comm)
        {
            try
            {
                // discovery parameter
                var cachedKey = comm.CommandText;
                if (m_CachedParameters.ContainsKey(cachedKey))
                {
                    var source = m_CachedParameters[cachedKey];
                    foreach (var param in source)
                    {
                        comm.Parameters.Add((OracleParameter)param.Clone());
                    }
                }
                else
                {
#if DEBUG
                    comm.CommandText += "--" + (new Random().Next());
#endif
                    OracleCommandBuilder.DeriveParameters(comm);
                    comm.CommandText = cachedKey;
                    var source = new OracleParameter[comm.Parameters.Count];
                    for (var i = 0; i < comm.Parameters.Count; i++)
                    {
                        source[i] = (OracleParameter)comm.Parameters[i].Clone();
                    }
                    m_CachedParameters.Add(cachedKey, source);
                }
            }
            catch (Exception ex)
            {
                //throw ErrorUtils.CreateErrorWithSubMessage(
                //    ERR_SQL.ERR_SQL_DISCOVERY_PARAMS_FAIL, ex.Message,
                //    comm.CommandText);
            }
        }

        public static void ExecuteStoreProcedure(string connectionString, string commandText, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    var comm = new OracleCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
                    AssignParameters(comm,  values);
                    comm.ExecuteNonQuery();
                    if (
                        comm.Parameters.Contains(Constants.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                        comm.Parameters[Constants.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                        )
                    {
                        var errCode = int.Parse(comm.Parameters[Constants.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                        //if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                    }
                }
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
                }
             
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

       
        public static void FillDataTable(string connectionString, string commandText, out DataTable resultTable, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ex;
                    //throw ErrorUtils.CreateErrorWithSubMessage(
                    //    ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message, commandText);
                }

                try
                {
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        var adap = new OracleDataAdapter(comm);
                        var ds = new DataSet();
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, values);

                        adap.Fill(ds);
                        if (
                            comm.Parameters.Contains(Constants.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                            comm.Parameters[Constants.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                            )
                        {
                            var errCode = int.Parse(comm.Parameters[Constants.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                            //if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                        }
                        resultTable = ds.Tables[0];
                    }
                }
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
                }
               
                catch (Exception ex)
                {
                    throw ex;
                    //throw ErrorUtils.CreateErrorWithSubMessage(
                    //    ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, commandText);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

      

        public static Exception ThrowOracleUserException(OracleException ex, string commandText)
        {
            if (ex.Number == Constants.ORACLE_USER_HANDLED_EXCEPTION_CODE)
            {
                var match = Regex.Match(ex.Message, "<ERROR ID=([0-9]+)>([^<]*)</ERROR>");
                if (match.Success)
                {
                    var errCode = int.Parse(match.Groups[1].Value);
                    var errMessage = match.Groups[2].Value;

                    if (!string.IsNullOrEmpty(errMessage))
                    {
                        //return ErrorUtils.CreateErrorWithSubMessage(errCode, errMessage);
                        return null;
                    }
                    //return ErrorUtils.CreateError(errCode);
                    return null;
                }
            }
            //return ErrorUtils.CreateErrorWithSubMessage(
            //    ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, commandText);
            return null;
        }


        public static void FillDataSetWithoutPram(string connectionString,  string commandText, out DataSet ds)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ex;
                    //throw ErrorUtils.CreateErrorWithSubMessage(
                    //    ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message,
                    //    commandText);
                }

                try
                {
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm);
                        ds = new DataSet();
                        var adap = new OracleDataAdapter(comm);
                        adap.Fill(ds);
                        if (
                            comm.Parameters.Contains(Constants.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                            comm.Parameters[Constants.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                            )
                        {
                            var errCode = int.Parse(comm.Parameters[Constants.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                           // if (errCode != 0) throw ErrorUtils.WriteLog(errCode + commandText);
                        }
                    }
                }
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
                }
                
                catch (Exception ex)
                {
                    throw ex;
                    //throw ErrorUtils.CreateErrorWithSubMessage(
                    //    ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message,
                    //    commandText);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //end duchvm add
    }

    public static class ComponentExtensions
    {
        private static readonly Dictionary<Type, object> CachedMapInfo = new Dictionary<Type, object>();
        public static List<T> ToList<T>(this OracleDataReader reader)
            where T : class, new()
        {
            var col = new List<T>();
            while (reader.Read())
            {
                var obj = new T();
                reader.MapObject(obj);
                col.Add(obj);
            }
            return col;
        }

        public static T ToObject<T>(this OracleDataReader reader)
            where T : class, new()
        {
            var obj = new T();
            reader.Read();
            MapObject(reader, obj);
            return obj;
        }

        private static void MapObject<T>(this OracleDataReader reader, T obj)
            where T : class, new()
        {
            var mapInfo = GetMapInfo<T>();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader[i] != DBNull.Value && mapInfo.ContainsKey(reader.GetName(i)))
                {
                    var prop = mapInfo[reader.GetName(i)];
                    prop.SetValue(obj, Convert.ChangeType(reader[i], prop.PropertyType), null);
                }
            }
        }

        private static Dictionary<string, PropertyInfo> GetMapInfo<T>()
        {
            var type = typeof(T);
            if (CachedMapInfo.ContainsKey(type))
            {
                return (Dictionary<string, PropertyInfo>)CachedMapInfo[type];
            }

            var mapInfo = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                var attributes = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                foreach (ColumnAttribute attr in attributes)
                {
                    mapInfo.Add(attr.Name, prop);
                }
            }

            CachedMapInfo.Add(type, mapInfo);
            return mapInfo;
        }
    }
}
