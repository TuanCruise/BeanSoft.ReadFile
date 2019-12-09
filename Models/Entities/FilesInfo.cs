using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class FilesInfo
    {
        public string DirName { get; set; }
        public string Extension { get; set; }
        public List<string> FileName { get; set; }
    }
}
