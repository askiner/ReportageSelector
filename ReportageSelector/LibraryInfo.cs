using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportageSelector
{
    public class LibraryInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PicturesPath { get; set; }
        public string ReportagePath { get; set; }
        public string ReportageZeroPath { get; set; }
        public string XmlPath { get; set; }

        public LibraryInfo() { }
    }
}
