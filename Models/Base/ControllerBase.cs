using System;
using System.Collections.Generic;
using System.Text;
using Models.Common;

namespace Models.Base
{
    public class ControllerBase : IDisposable
    {
        protected string ConnectionString { get; set; }

        protected ControllerBase()
        {
            ConnectionString = App.Configs.ConnectionString;
        }

        protected ControllerBase(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
