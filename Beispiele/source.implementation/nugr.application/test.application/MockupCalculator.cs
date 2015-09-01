using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nugr.contract.calculator;

namespace test.application
{
    class MockupCalculator : ICalculator
    {
        #region ICalculator Members

        public long[] Calculate(long from, long to)
        {
            return new long[] {from, to, 99};
        }

        #endregion
    }
}
