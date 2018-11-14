using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportageSelector
{
    public class Metadata
    {
        public string Caption { get; set; }        
        public string FixedIdentifier { get; set; }
        public List<string> Keywords { get; set; }
        public string CountryName { get; set; }
        public string CountryNameEn { get; set; }
        public string CountryCode { get; set; }
        public string Source { get; set; }

        //Writer-Editor
    }
}
