using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nugr.contract.application;
using nugr.contract.calculator;
using nugr.contract.dbadapter;
using nugr.contract.domainmodel;

namespace nugr.application
{
    public class PrimeApp : IApplication
    {
        private IDBAdapter _db;
        private ICalculator _calc;

        public PrimeApp(IDBAdapter db, ICalculator calc)
        {
            _db = db;
            _calc = calc;
        }

        #region IApplication Members

        public CalcPrj Calculate(long from, long to)
        {
            long[] primes = _calc.Calculate(from, to);
            return new CalcPrj(from, to, primes);
        }

        public CalcPrj Load(string filename)
        {
            return _db.Load(filename);
        }

        public void Save(CalcPrj prj, string filename)
        {
            _db.Save(prj, filename);
        }

        #endregion
    }
}
