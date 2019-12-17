using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class TransInfo
    {
        public string TRANSACTION_START { get; set; }
        public string CARD_INSERTED { get; set; }
        public string CARD_NUMBER { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string PIN_ENTERED {get; set;}
        public string TRANSACTION_TYPE { get; set; }
        public string AMOUNT { get; set; }
        public string CARD_TAKEN { get; set; }
        public string TRANSACTION_END { get; set; }
        public string NOTES_PRESENTED { get; set; }
        public string NOTES_RETRACTED { get; set; }
        public string NOTES_TAKEN { get; set; }
        
        public string CASH  { get; set; }
        public string CASH_REQUEST { get; set; }
        public string CASH_PRESENTED { get; set; }
        public string CASH_TAKEN { get; set; }
        public string CASH_RETRACTED { get; set; }
        public string TRANSACTION_DATE { get; set; }
    }
}