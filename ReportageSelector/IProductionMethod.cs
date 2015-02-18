using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportageSelector
{
    public interface IProductionMethod
    {
        void Produce(string file, string prefix);
    }

    public class VisibleReportageMethod : IProductionMethod
    {
        public void Produce(string file, string prefix)
        {
            new NotImplementedException();   
        }
    }

    public class HiddenReportageMethod : IProductionMethod
    {
        public void Produce(string file, string prefix)
        {
            new NotImplementedException();
        }
    }

    public class NoReportageMethod : IProductionMethod
    {
        public void Produce(string file, string prefix)
        {
            new NotImplementedException();
        }
    }
}
