namespace Models.Entities
{
    public class VisaDebitFee
    {
        public string BranchID { get; set; }
        public string CardNameOrg { get; set; }
        public string CardNo { get; set; }
        public string Acctno { get; set; }
        public string ValidDate { get; set; }
        public string ExpiredDate { get; set; }
        public string CardType { get; set; }
        public string Type { get; set; }
        public string FeeAmount { get; set; }
    }
}
