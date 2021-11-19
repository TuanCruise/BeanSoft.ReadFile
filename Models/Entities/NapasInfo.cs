namespace Models.Entities
{
    public class NapasInfo
    {
        public string Type { get; set; }
        public string FldName { get; set; } // Loai ban ghi
        public string DataType { get; set; } // So the
        public int MaxLength { get; set; } // Ma xu ly
        public string DefaultValue { get; set; } // So tien giao dich
        public string Position { get; set; } // Them defaulvalue vao ben trai hay ben phai . Mac dinh la ben trai
    }
}
