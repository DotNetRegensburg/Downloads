using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nugr.contract.application;
using nugr.contract.domainmodel;

namespace test.uiportal
{
    class MockupApplication : IApplication
    {
        #region IApplication Members

        public CalcPrj Calculate(long from, long to)
        {
            return new CalcPrj(from, to, new long[] {from, to});
        }

        public CalcPrj Load(string filename)
        {
            return new CalcPrj(10, 14, new long[] { 11, 13 });
        }

        public void Save(nugr.contract.domainmodel.CalcPrj prj, string filename)
        {
        }

        #endregion
    }
}
