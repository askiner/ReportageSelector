using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace ReportageSelector
{
    public class TextWriterWithTimeTraceListener : System.Diagnostics.TextWriterTraceListener
    {
        public TextWriterWithTimeTraceListener(string fileName) : base(fileName) { }
        public TextWriterWithTimeTraceListener(string fileName, string name) : base(fileName, name) { }

        public override void WriteLine(string message)
        {
            base.Write(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            base.Write(", ");
            base.WriteLine(message);
        }
    }
}
