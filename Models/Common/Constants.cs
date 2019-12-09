using System;

namespace Models.Common
{
    public class Constants
    {
        public static string TRANSACTION_START = "*TRANSACTION START*";
        public static string TRANSACTION_STOP = "TRANSACTION END";
        public static string CARD_INSERTED = "CAPTURE AT CARD INSERTED";
        public static string CARD_NUMBER = "CARD NUMBER:";
        public static string CUSTOMER_NAME = "CUSTOMER NAME:";
        public static string PIN_ENTERED = "PIN ENTERED";
        public static string TRANSACTION_TYPE = "TRANSACTION TYPE:";
        public static string AMOUNT = "AMOUNT:";
        public static string CARD_TAKEN = "CARD TAKEN";
        public static string TRANSACTION_END = "TRANSACTION END";
        public static string NOTES_TAKEN = "NOTES TAKEN";
        public static string NOTES_PRESENTED = "NOTES PRESENTED";
        public static string NOTES_RETRACTED = "NOTES RETRACTED";


        public static string TRANSACTION_START_JRN = "TRANSACTION START";
        public static string TRANSACTION_STOP_JRN = "<- TRANSACTION END";
        public static string CASH = "CASH";
        public static string CASH_REQUEST = "CASH REQUEST:";
        public static string CASH_PRESENTED = "CASH PRESENTED";
        public static string CASH_TAKEN = "CASH TAKEN";
        public static string TRACK_2_DATA = "TRACK 2 DATA:";

        public static string Filter1 = "\u001b[020t";

        public const string ORACLE_SESSION_USER = "SESSIONINFO_USERNAME";
        public const string ORACLE_SESSIONKEY = "SESSIONKEY";
        public const string ORACLE_REPORT_ID = "REPORT_ID";
        public const string ORACLE_STARTPOINT = "pv_STARTPOINT";
        public const string ORACLE_REPORT_DETAILID = "DETAILID";
        public const string ORACLE_REPORT_RPTLOGSID = "RPTLOGSID";
        public const string ORACLE_CURSOR_OUTPUT = "STORE_OUTPUT";
        public const string ORACLE_MODULE_ID = "PV_MODID";

        public const string ORACLE_EXCEPTION_PARAMETER_NAME = "PROC_RETURN";
        public const string ORACLE_OUT_PARAMETER_SECID = "SECID";
        public const int ORACLE_USER_HANDLED_EXCEPTION_CODE = 20000;

        public const string PROC_ATMINFOS_ADD = "SP_ATMINFOS_ADD";
        public const string PROC_ATMINFOSJRN_ADD = "SP_ATMINFOSJRN_ADD";
        public const string PROC_ACCOUNTING_ADD = "SP_ACCOUNTING_TRANS_INS";
        public const string PROC_BILLINGINFO_ADD = "SP_BILLINGINFO_ADD";
        public const string PROC_GET_ACCOUNTING = "SP_ACCOUNTING_TRANS_INS";
        public const string PROC_GET_ATMPOS = "SP_ACCOUNTING_TRANS_INS";
        public const string PROC_GET_ATMPOSJRN = "SP_ACCOUNTING_TRANS_INS";
        public const string PROC_LOG_ADD = "SP_SYS_DATAFILE_LOG_ADD";


        public const string YES = "Y";
        public const string NO = "N";

        public const string EXTENSION_JRN = "JRN";
        public const string EXTENSION_LOG = "LOG";
        public const string EXTENSION_Excel = "XLSX";

        public const string READFILE_SUCCESS = "S";
        public const string READFILE_FAIL = "F";

        public const int POS_BASE = 0;
        public const int POS_1 = POS_BASE + 4;
        public const int POS_2 = POS_BASE + 25;
        public const int POS_3 = POS_BASE + 33;
        public const int POS_4 = POS_BASE + 45;
        public const int POS_5 = POS_BASE + 51;
        public const int POS_6 = POS_BASE + 57;
        public const int POS_7 = POS_BASE + 61;
        public const int POS_8 = POS_BASE + 65;
        public const int POS_9 = POS_BASE + 69;
        public const int POS_10 = POS_BASE + 75;
        public const int POS_11 = POS_BASE + 83;
        public const int POS_12 = POS_BASE + 86;
        public const int POS_13 = POS_BASE + 106;
        public const int POS_14 = POS_BASE + 126;
        public const int POS_15 = POS_BASE + 130;


        public const int StartRow = 6;
    }
}
