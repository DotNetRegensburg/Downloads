using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nugr.application;
using nugr.contract.application;
using nugr.contract.domainmodel;
using NUnit.Framework;

namespace test.application
{
    [TestFixture]
    public class testPrimeApp
    {
        [Test]
        public void testLoad()
        {
            MockupDBAdapter mdb = new MockupDBAdapter();
            IApplication app = new PrimeApp(mdb, null);

            CalcPrj prj = app.Load("test.blub");
            Assert.IsNotNull(prj);
            Assert.AreEqual(1, prj.From);
            Assert.AreEqual(2, prj.To);
            Assert.IsNull(prj.Primes);
            Assert.AreEqual("test.blub", mdb._filename);
        }

        [Test]
        public void testSave()
        {
            MockupDBAdapter mdb = new MockupDBAdapter();
            IApplication app = new PrimeApp(mdb, null);

            CalcPrj prj = new CalcPrj(10, 11, null);
            app.Save(prj, "test.blub");

            Assert.AreEqual("test.blub", mdb._filename);
            Assert.IsNotNull(mdb._prj);
            Assert.AreEqual(10, mdb._prj.From);
            Assert.AreEqual(11, mdb._prj.To);
        }

        [Test]
        public void testCalc()
        {
            MockupCalculator calc = new MockupCalculator();
            IApplication app = new PrimeApp(null, calc);

            CalcPrj prj = app.Calculate(1, 10);
            Assert.IsNotNull(prj);
            Assert.AreEqual(1, prj.From);
            Assert.AreEqual(10, prj.To);
            Assert.AreEqual(3, prj.Primes.Length);
            Assert.AreEqual(1, prj.Primes[0]);
            Assert.AreEqual(10, prj.Primes[1]);
            Assert.AreEqual(99, prj.Primes[2]);
        }
    }
}
