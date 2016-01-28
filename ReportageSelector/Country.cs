using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportageSelector
{
    public class Country
    {
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }

        public Country(string code, string nameRu, string nameEn)
        {
            this.Code = code;
            this.NameRu = nameRu;
            this.NameEn = nameEn;
        }
    }
}
