using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ReportageSelector
{
    /// <summary>
    /// Parameters of database query. Contains parameters to build query.
    /// </summary>
    public class LibraryInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PicturesPath { get; set; }
        public string ReportagePath { get; set; }
        public string ReportageZeroPath { get; set; }
        public string XmlPath { get; set; }

        /// <summary>
        /// Deep of query
        /// </summary>
        public int Period { get; set; }

        public LibraryInfo() { }

        public static readonly int DefaultPeriod = 3;
    }
}
