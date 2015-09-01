using System;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Reflection;
using System.Runtime.DurableInstancing;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace Articles.Workflow.WorkflowPersistence
{
    public class ZipWorkflowInstanceStore : InstanceStore
    {
        private const string PART_GUID = "/guid.txt";
        private const string CONTENT_TYPE_TXT = "text/text";
        private const string CONTENT_TYPE_XML = "text/xml";
        private const string WF_STATE_CLOSED = "Closed";

        private Guid m_instanceStoreGuid;
        private string m_zipFilePath;

        /// <summary>
        /// XMLs the workflow instance store.
        /// </summary>
        /// <param name="zipFilePath">The zip file path.</param>
        public ZipWorkflowInstanceStore(string zipFilePath)
        {
            m_zipFilePath = zipFilePath;

            if (File.Exists(m_zipFilePath))
            {
                //Load existing instance store
                try
                {
                    using (ZipPackage zipPackage = Package.Open(m_zipFilePath, FileMode.Open) as ZipPackage)
                    {
                        ZipPackagePart zipPackagePart = zipPackage.GetPart(new Uri(PART_GUID, UriKind.Relative)) as ZipPackagePart;
                        using (StreamReader inputStream = new StreamReader(zipPackagePart.GetStream(FileMode.Open)))
                        {
                            m_instanceStoreGuid = new Guid(inputStream.ReadToEnd());
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("File " + zipFilePath + " is no valid ZipWorkflowInstanceStore!", "zipFilePath", ex);
                }
            }
            else
            {
                //Create a new instance store
                m_instanceStoreGuid = Guid.NewGuid();
                using (ZipPackage zipPackage = Package.Open(m_zipFilePath, FileMode.Create) as ZipPackage)
                {
                    ZipPackagePart zipPackagePart = zipPackage.CreatePart(new Uri(PART_GUID, UriKind.Relative), CONTENT_TYPE_TXT, CompressionOption.Fast)
                        as ZipPackagePart;
                    using (StreamWriter outputStream = new StreamWriter(zipPackagePart.GetStream(FileMode.Create)))
                    {
                        outputStream.Write(m_instanceStoreGuid.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Tries to execute the given command.
        /// </summary>
        /// <param name="context">Context of execution.</param>
        /// <param name="command">The command to perform.</param>
        /// <param name="timeout">The timeout for executing.</param>
        protected override bool TryCommand(InstancePersistenceContext context, InstancePersistenceCommand command, TimeSpan timeout)
        {
            return EndTryCommand(BeginTryCommand(context, command, timeout, null, null));
        }

        /// <summary>
        /// Starts executing the given command. This is the main method of this InstanceStore object.
        /// </summary>
        /// <param name="context">The context of execution.</param>
        /// <param name="command">The command to be executed.</param>
        /// <param name="timeout">The timeout value.</param>
        /// <param name="callback">The callback method (Async Pattern).</param>
        /// <param name="state">The async state object (Async Pattern).</param>
        /// <returns></returns>
        protected override IAsyncResult BeginTryCommand(InstancePersistenceContext context, InstancePersistenceCommand command, TimeSpan timeout, AsyncCallback callback, object state)
        {
            Func<bool> asyncFunc = new Func<bool>(() =>
            {
                //This command locks the workflow
                CreateWorkflowOwnerCommand createOwnerCommand = command as CreateWorkflowOwnerCommand;
                if (createOwnerCommand != null)
                {
                    context.BindInstanceOwner(m_instanceStoreGuid, Guid.NewGuid());
                }

                //This command saves the workflow into the instance store
                SaveWorkflowCommand saveWorkflowCommand = command as SaveWorkflowCommand;
                if (saveWorkflowCommand != null)
                {
                    SaveData(saveWorkflowCommand.InstanceData);
                }

                //This command loads a workflow
                LoadWorkflowCommand loadWorkflowCommand = command as LoadWorkflowCommand;
                if (loadWorkflowCommand != null)
                {
                    Guid targetGuid = ReadProperty<Guid>(context.InstanceHandle, "Id");
                    IDictionary<XName, InstanceValue> loadedData = LoadData(targetGuid);
                    context.LoadedInstance(InstanceState.Initialized, loadedData, null, null, null);
                }

                return true;
            });
            return asyncFunc.BeginInvoke(
                callback, 
                state);
        }

        /// <summary>
        /// Ends execution of the command.
        /// </summary>
        /// <param name="result">The result.</param>
        protected override bool EndTryCommand(IAsyncResult result)
        {
            System.Runtime.Remoting.Messaging.AsyncResult asyncResult = result as System.Runtime.Remoting.Messaging.AsyncResult;
            Func<bool> func = asyncResult.AsyncDelegate as Func<bool>;
            return func.EndInvoke(result);
        }

        /// <summary>
        /// Reads the given property from the given target object.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="targetObject">The target object.</param>
        /// <param name="targetProperty">The target property name.</param>
        private T ReadProperty<T>(object targetObject, string targetProperty)
        {
            return (T)targetObject.GetType()
                .GetProperty(targetProperty, BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(targetObject, null);
        }

        /// <summary>
        /// Saves all given instance data into the zip file.
        /// </summary>
        /// <param name="instanceData">The instance data to save.</param>
        private void SaveData(IDictionary<XName, InstanceValue> instanceData)
        {
            using (ZipPackage zipPackage = Package.Open(m_zipFilePath, FileMode.Open) as ZipPackage)
            {
                //Create new document that will store all given data
                XmlDocument docToSave = new XmlDocument();

                //Create initial element
                docToSave.LoadXml("<InstanceValues />");

                //Serialize all data
                foreach (KeyValuePair<XName, InstanceValue> actPair in instanceData)
                {
                    XmlElement newInstance = docToSave.CreateElement("InstanceValue");
                    XmlAttribute typeAttrib = docToSave.CreateAttribute("type");
                    typeAttrib.Value = actPair.Key.LocalName;
                    newInstance.Attributes.Append(typeAttrib);

                    XmlElement newKey = SerializeObject(actPair.Key, "key", docToSave);
                    newInstance.AppendChild(newKey);

                    XmlElement newValue = SerializeObject(actPair.Value.Value, "value", docToSave);
                    newInstance.AppendChild(newValue);

                    docToSave.DocumentElement.AppendChild(newInstance);
                }

                //Get id of the workflow instance
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(docToSave.NameTable);
                namespaceManager.AddNamespace("activity", "http://schemas.datacontract.org/2010/02/System.Activities");
                namespaceManager.AddNamespace("linq", "http://schemas.datacontract.org/2004/07/System.Xml.Linq");
                XmlNode idNode = docToSave.SelectSingleNode("/InstanceValues/InstanceValue[@type='Workflow']/value/activity:Executor/activity:WorkflowInstanceId", namespaceManager);
                Guid workflowID = new Guid(idNode.InnerText);

                //Check for state
                XmlNode stateNode = idNode.ParentNode.SelectSingleNode("activity:state", namespaceManager);
                bool isWorkflowClosed = false;
                if (stateNode != null)
                {
                    isWorkflowClosed = stateNode.InnerText == WF_STATE_CLOSED;
                }

                //Open or create package part for writing
                string packagePartPath = "/instances/" + workflowID.ToString() + ".xml";
                Uri packagePartUri = new Uri(packagePartPath, UriKind.Relative);
                ZipPackagePart packagePart = null;
                if (zipPackage.PartExists(packagePartUri)) { packagePart = zipPackage.GetPart(packagePartUri) as ZipPackagePart; }
                else { packagePart = zipPackage.CreatePart(new Uri(packagePartPath, UriKind.Relative), "text/xml", CompressionOption.Fast) as ZipPackagePart; }

                //Write the generated sql
                using(Stream outStream = packagePart.GetStream(FileMode.Create, FileAccess.Write))
                {
                    docToSave.Save(outStream);
                }
            }
        }

        /// <summary>
        /// Loads all data theat belongs to the given instance id.
        /// </summary>
        /// <param name="instanceGuid">The instance id to load.</param>
        private IDictionary<XName, InstanceValue> LoadData(Guid instanceGuid)
        {
            Dictionary<XName, InstanceValue> result = new Dictionary<XName, InstanceValue>();

            using (ZipPackage zipPackage = Package.Open(m_zipFilePath, FileMode.Open) as ZipPackage)
            {
                //Check if there is any instance with the given guid
                string packagePartPath = "/instances/" + instanceGuid.ToString() + ".xml";
                Uri packagePartUri = new Uri(packagePartPath, UriKind.Relative);
                if (!zipPackage.PartExists(packagePartUri)) { throw new ApplicationException("Unable to find workflow instance " + instanceGuid + "!"); }

                //Read the file
                ZipPackagePart packagePart = zipPackage.GetPart(packagePartUri) as ZipPackagePart;
                using (Stream inputStream = packagePart.GetStream(FileMode.Open))
                using (XmlReader xmlReader = XmlReader.Create(inputStream))
                {
                    //Deserialize xml file
                    NetDataContractSerializer serializer = new NetDataContractSerializer();

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(xmlReader);

                    XmlNodeList instances = xmlDocument.GetElementsByTagName("InstanceValue");
                    foreach (XmlElement instanceElement in instances)
                    {
                        XmlElement keyElement = (XmlElement)instanceElement.SelectSingleNode("descendant::key");
                        XName key = (XName)DeserializeObject(serializer, keyElement);

                        XmlElement valueElement = (XmlElement)instanceElement.SelectSingleNode("descendant::value");
                        object value = DeserializeObject(serializer, valueElement);
                        InstanceValue instVal = new InstanceValue(value);

                        result[key] = instVal;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Serializes the given object into the given xml element.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <param name="elementName">The name of the generated element.</param>
        /// <param name="doc">The owner.</param>
        private XmlElement SerializeObject(object objectToSerialize, string elementName, XmlDocument doc)
        {
            NetDataContractSerializer serializer = new NetDataContractSerializer();
            XmlElement newElement = null;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, objectToSerialize);
                memoryStream.Position = 0;

                using (StreamReader rdr = new StreamReader(memoryStream))
                {
                    newElement = doc.CreateElement(elementName);
                    newElement.InnerXml = rdr.ReadToEnd();
                }
            }

            return newElement;
        }

        /// <summary>
        /// Deserializes the next object.
        /// </summary>
        /// <param name="serializer">The deserializer used for deserializing.</param>
        /// <param name="element">The element to read from.</param>
        private object DeserializeObject(NetDataContractSerializer serializer, XmlElement element)
        {
            object deserializedObject = null;

            MemoryStream stm = new MemoryStream();
            XmlDictionaryWriter wtr = XmlDictionaryWriter.CreateTextWriter(stm);
            element.WriteContentTo(wtr);
            wtr.Flush();
            stm.Position = 0;

            deserializedObject = serializer.Deserialize(stm);

            return deserializedObject;
        }
    }
}