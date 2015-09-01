using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XmlExplorer.UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MainWindowPresentationModelTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get;
            set;
        }

        internal IUnityContainer Container
        {
            get;
            private set;
        }

        internal MainWindowPresentationModel CreatePresentationModel()
        {
            return Container.Resolve<MainWindowPresentationModel>();
        }

        internal IUnityContainer CreateTestContainer<TFileProvider, TEventAggregator>()
            where TFileProvider : IFileProvider
            where TEventAggregator : IEventAggregator
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IFileProvider, TFileProvider>();
            container.RegisterType<IEventAggregator, TEventAggregator>();
            return container;
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize]
        //public static void ClassInitialize(TestContext testContext) 
        //{
        //}

        // Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup]
        //public static void ClassCleanup() 
        //{ 
        //}

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            Container = CreateTestContainer<FakeFileProvider, FakeEventAggregator>();
        }

        //
        // Use TestCleanup to run code after each test has run
        //[TestCleanup]
        //public void TestCleanup() 
        //{ 
        //}
        #endregion

        [TestMethod]
        public void CanOpenWithNoFile()
        {
            MainWindowPresentationModel model = CreatePresentationModel();
            Assert.IsTrue(model.Open.CanExecute(null));
        }

        [TestMethod]
        public void CannotCloseWithNoFile()
        {
            MainWindowPresentationModel model = CreatePresentationModel();
            Assert.IsFalse(model.Close.CanExecute(null));
        }

        [TestMethod]
        public void CanExitOnWithNoFile()
        {
            MainWindowPresentationModel model = CreatePresentationModel();
            Assert.IsFalse(model.Close.CanExecute(null));
        }

        [TestMethod]
        public void CanCloseWithFile()
        {
            MainWindowPresentationModel model = CreatePresentationModel();
            model.Open.Execute(null);
            Assert.IsTrue(model.Close.CanExecute(null));
        }

        [TestMethod]
        public void SetsCurrentFile()
        {
            MainWindowPresentationModel model = CreatePresentationModel();
            model.Open.Execute(null);
            Assert.IsNotNull(model.CurrentFileName);
            Assert.AreEqual(FakeFileProvider.FileName, model.CurrentFileName);
        }

        [TestMethod]
        public void ClearsCurrentFile()
        {
            MainWindowPresentationModel model = CreatePresentationModel();
            model.Open.Execute(null);
            Assert.IsNotNull(model.CurrentFileName);
            model.Close.Execute(null);
            Assert.IsNull(model.CurrentFileName);
        }

        [TestMethod]
        public void CancelCloseLeavesFileOpen()
        {
            Container = CreateTestContainer<NullFileProvider, FakeEventAggregator>();
            MainWindowPresentationModel model = CreatePresentationModel();
            string expected = "Test.xml";
            model.CurrentFileName = expected;
            model.Open.Execute(null);
            Assert.AreEqual(expected, model.CurrentFileName);
        }
    }
}
