using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nugr.calculator;
using nugr.contract.calculator;
using NUnit.Framework;

namespace test.calculator
{
    [TestFixture]
    public class testCalculator
    {
        [Test]
        public void testPrimeCalc()
        {
            ICalculator c;
            c = new PrimeCalc();

            long[] primes;
            primes = c.Calculate(1, 1);
            Assert.IsNotNull(primes);
            Assert.AreEqual(1, primes.Length);
            Assert.AreEqual(1, primes[0]);

            primes = c.Calculate(1, 2);
            Assert.AreEqual(2, primes.Length);
            Assert.AreEqual(1, primes[0]);
            Assert.AreEqual(2, primes[1]);

            primes = c.Calculate(1, 5);
            Assert.AreEqual(4, primes.Length);

            primes = c.Calculate(10, 20);
            long[] expectedPrimes = new long[] {11, 13, 17, 19};
            Assert.AreEqual(expectedPrimes.Length, primes.Length);
            for (int i = 0; i < expectedPrimes.Length; i++)
                Assert.AreEqual(expectedPrimes[i], primes[i]);
        }
    }
}
