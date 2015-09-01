using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nugr.contract.domainmodel
{
    [Serializable]
    public class CalcPrj
    {
        public CalcPrj(long from, long to, long[] primes)
        {
            this.From = from;
            this.To = to;
            this.Primes = primes;
        }

        public long From { get; private set; }
        public long To { get; private set; }
        public long[] Primes { get; set; }
    }
}
