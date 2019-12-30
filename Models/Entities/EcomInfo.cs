using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class EcomInfo
    {
        public EcomInfoHeader ecomInfoHeader { get; set;}
        public string DETAILRECORD { get; set; } // Loai ban ghi
        public string PANNO { get;set;} // So the
        public  string PROCESSING_CODE  { get;set;} // Ma xu ly
        public  string AMOUNT  { get;set;} // So tien giao dich
        public  string TRACE_NUMBER  { get;set;} // So trace
        public  string TRANS_TIME  { get;set;} // Gio giao dich
        public  string TRANS_DATE  { get;set;} // Ngay giao dich
        public  string PAYMENT_DATE  { get;set;} // Ngay thanh toan
        public  string DEVICE_TYPE  { get;set;} // Loai thiet bi
        public string BANK_CODE { get; set; } // Ma to chuc chap nhan the
        public  string AUTH_NUMBER { get;set;} // So tham tra
        public string DEVICE_CODE { get; set; } // Ma so thiet bi chap nhan the
        public string CCY { get; set; } // Ma tien te
        public string TRANSACTION_CODE { get; set; } // Ma giao dich tai SML
        public  string AUTHORISATION_CODE { get;set;} // Ma giao dich tai ngan hang
        public  string MTI  { get;set;} // Ma dinh dang thong diep
        public string STATUS { get; set; } // Trang thai
        public string CHECKSUM { get; set; } // Checksum
        public EcomInfoFooter  ecomInfoFooter { get; set; }
    }
}
