using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class BillingInfo
    {
        public  string MTI { get;set;}
        public  string PAN_ACC  { get;set;}
        public  string PROCESS_CODE { get;set;}
        public  string AMOUNT  { get;set;}
        public  string TRACE_NUMBER  { get;set;}
        public  string TRANS_TIME  { get;set;}
        public  string TRANS_DATE  { get;set;}
        public  string PAYMENT_DATE  { get;set;}
        public  string DEVICE_TYPE  { get;set;}
        public string BANK_CODE { get; set; }
        public  string AUTH_NUMBER { get;set;}
        public  string DEVICE_CODE  { get;set;}
        public  string CCY  { get;set;}
        public  string ACC_FROM  { get;set;}
        public  string ACC_TO  { get;set;}
        public string STATUS { get; set; }

    }
}
