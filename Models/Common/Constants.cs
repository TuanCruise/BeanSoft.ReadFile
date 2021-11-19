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
        public static string CASH_RETRACTED = "CASH RETRACTED";
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
        public const string PROC_ECOMINFO_ADD = "SP_ECOMINFO_ADD";

        public const string PROC_GET_ACCOUNTING = "SP_ACCOUNTING_TRANS_INS";
        public const string PROC_GET_ATMPOS = "SP_ACCOUNTING_TRANS_INS";
        public const string PROC_GET_ATMPOSJRN = "SP_ACCOUNTING_TRANS_INS";
        public const string PROC_LOG_ADD = "SP_SYS_DATAFILE_LOG_ADD";

        public const string PROC_GET_DATA_SRS = "SP_GET_DATA_SRS";


        public const string YES = "Y";
        public const string NO = "N";

        public const string EXTENSION_JRN = "JRN";
        public const string EXTENSION_LOG = "LOG";
        public const string EXTENSION_Excel = "XLSX";
        public const string EXTENSION_DAT = "DAT";

        public const string FOLDER_BILLING = "BILLING";
        public const string FOLDER_ECOM = "ECOM";

        public const string READFILE_SUCCESS = "S";
        public const string READFILE_FAIL = "F";

        public const string FILE_INC = "INC";
        public const string FILE_REF = "REF";
        public const string FILE_ACK = "ACK";
        public const string FILE_OUT = "OUT";

        public const int POS_BASE = 0;
        public const int POS_1 = POS_BASE + 4;
        public const int POS_2 = POS_1 + 19;
        public const int POS_3 = POS_2 + 6;
        public const int POS_4 = POS_3 + 12;
        public const int POS_5 = POS_4 + 6;
        public const int POS_6 = POS_5 + 6;
        public const int POS_7 = POS_6 + 4;
        public const int POS_8 = POS_7 + 4;
        public const int POS_9 = POS_8 + 4;
        public const int POS_10 = POS_9 + 8;
        public const int POS_11 = POS_10 + 6;
        public const int POS_12 = POS_11 + 8;
        public const int POS_13 = POS_12 + 3;
        public const int POS_14 = POS_13 + 20;
        public const int POS_15 = POS_14 + 20;
        public const int POS_16 = POS_15 + 4;
        public const int POS_17 = POS_16 + 4;
        public const int POS_18 = POS_17 + 32;



        public const int StartRow = 6;
        public const string FileData_JRN = "14";
        public const string FileData_LOG = "16";
        public const string FileData_HT = "28";
        public const string FileTopupBilling = "29";
        public const string FileEcom = "33";

    }
}
