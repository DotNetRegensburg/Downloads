using NUnit.Framework;
using nugr.application;

namespace nugr.application_test
{
    [TestFixture]
    public class TestFibonnacci
    {
        private Fibonnacci _fibonnacci;

        [SetUp]
        public void Init()
        {
            this._fibonnacci = new Fibonnacci();
        }

        [Test]
        public void Calculate_Fibonnacci()
        {
            int[][] cases ={new int[]{0,0}, new int[]{1,1}, new int[]{2,1}, 
                new int[]{3,2}, new int[]{4,3}, new int[]{5,5}};

	        for (int i = 0; i < cases.Length; i++)
	        {
                Assert.AreEqual(cases[i][1], _fibonnacci.Calculate(cases[i][0]));
	        }
        }
    }
}
