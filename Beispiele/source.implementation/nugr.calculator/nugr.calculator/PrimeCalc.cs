using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nugr.contract.calculator;

namespace nugr.calculator
{
    public class PrimeCalc : ICalculator
    {
        #region ICalculator Members

        public long[] Calculate(long from, long to)
        {
            List<long> primes = new List<long>();
            for (long p = from; p <= to; p++)
            {
                bool isPrime = true;
                for(long t=2; t<=Math.Sqrt(p); t++)
                {
                    isPrime = p%t != 0;
                    if (!isPrime) break;
                }
                if (isPrime) primes.Add(p);
            }
            return primes.ToArray();
        }

        #endregion
    }
}
