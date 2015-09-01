using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlExplorerModule;
using XmlExplorerShared;

namespace XmlExplorer.UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ExplorerViewPresentationModelTest
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

        internal ExplorerViewPresentationModel CreatePresentationModel()
        {
            return Container.Resolve<ExplorerViewPresentationModel>();
        }

        internal IUnityContainer CreateTestContainer<TNodeListProvider, TEventAggregator>()
            where TNodeListProvider : INodeListProvider
            where TEventAggregator : IEventAggregator
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<INodeListProvider, TNodeListProvider>();
            container.RegisterType<IEventAggregator, TEventAggregator>(new ContainerControlledLifetimeManager());
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
            Container = CreateTestContainer<FakeNodeListProvider, EventAggregator>();
        }

        //
        // Use TestCleanup to run code after each test has run
        //[TestCleanup]
        //public void TestCleanup() 
        //{ 
        //}
        #endregion

        [TestMethod]
        public void TestFileOpened()
        {
            bool nodesChanged = false;
            ExplorerViewPresentationModel model = CreatePresentationModel();
            model.PropertyChanged += (s, e) =>
                {
                    nodesChanged = (e.PropertyName == "Nodes");
                };
            IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();
            FileChangedEvent fileChangedEvent = eventAggregator.GetEvent<FileChangedEvent>();
            fileChangedEvent.Publish("Test.xml");
            Assert.IsTrue(nodesChanged);
            Assert.IsNotNull(model.Nodes);
            Assert.AreEqual(1, model.Nodes.Count);
            Assert.IsInstanceOfType(model.Nodes[0], typeof(XmlDocument));
            Assert.AreEqual("test", model.Nodes[0].FirstChild.Name);
        }

        [TestMethod]
        public void TestFileClosed()
        {
            bool nodesChanged = false;
            ExplorerViewPresentationModel model = CreatePresentationModel();
            model.PropertyChanged += (s, e) =>
            {
                nodesChanged = (e.PropertyName == "Nodes");
            };
            IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();
            FileChangedEvent fileChangedEvent = eventAggregator.GetEvent<FileChangedEvent>();
            fileChangedEvent.Publish(null);
            Assert.IsTrue(nodesChanged);
            Assert.IsNull(model.Nodes);
        }
    }
}
