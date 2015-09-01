using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nugr.contract.dbadapter;
using nugr.contract.domainmodel;

namespace test.application
{
    public class MockupDBAdapter : IDBAdapter
    {
        public string _filename;
        public CalcPrj _prj;

        #region IDBAdapter Members

        public CalcPrj Load(string filename)
        {
            _filename = filename;
            return new CalcPrj(1, 2, null);
        }

        public void Save(CalcPrj prj, string filename)
        {
            _prj = prj;
            _filename = filename;
        }

        #endregion
    }
}
