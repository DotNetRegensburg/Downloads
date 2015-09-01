using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.Util
{
    public class AssemblyResourceReader
    {
        private static Type s_attribType;

        private Type m_targetType;
        private Assembly m_targetAssembly;
        private List<AssemblyResourceInfo> m_resources;
        private Dictionary<string, AssemblyResourceInfo> m_resourcesDict;
        private ResourceInfoCollection m_publicResources;

        /// <summary>
        /// Static constructor
        /// </summary>
        static AssemblyResourceReader()
        {
            s_attribType = typeof(AssemblyResourceFileAttribute);
        }

        /// <summary>
        /// Creates a new AssemblyResourceReader object
        /// </summary>
        public AssemblyResourceReader(Type targetType)
        {
            m_targetType = targetType;

            TypeInfo targetTypeInfo = m_targetType.GetTypeInfo();
            m_targetAssembly = targetTypeInfo.Assembly;

            m_resources = new List<AssemblyResourceInfo>();
            m_resourcesDict = new Dictionary<string, AssemblyResourceInfo>();
            foreach (AssemblyResourceFileAttribute actAttrib in targetTypeInfo.GetCustomAttributes<AssemblyResourceFileAttribute>())
            {
                ManifestResourceInfo resInfo = m_targetAssembly.GetManifestResourceInfo(actAttrib.ResourcePath);
                if (resInfo != null)
                {
                    AssemblyResourceInfo fileInfo = new AssemblyResourceInfo(m_targetAssembly, actAttrib.ResourcePath, actAttrib.Key);
                    m_resources.Add(fileInfo);

                    if ((actAttrib.Key != null) && (!m_resourcesDict.ContainsKey(actAttrib.Key)))
                    {
                        m_resourcesDict.Add(actAttrib.Key, fileInfo);
                    }
                }
                else
                {
                    throw new CommonLibraryException("Resource " + actAttrib.ResourcePath + " not found!");
                }
            }

            m_publicResources = new ResourceInfoCollection(this);
        }

        /// <summary>
        /// Opens the resource at the given index for reading
        /// </summary>
        public Stream OpenRead(int index)
        {
            AssemblyResourceInfo info = m_resources[index];
            return info.OpenRead();
        }

        /// <summary>
        /// Opens the resource with the given key for reading
        /// </summary>
        public Stream OpenRead(string key)
        {
            AssemblyResourceInfo info = m_resourcesDict[key];
            return info.OpenRead();
        }

        /// <summary>
        /// Gets complete text of the given resource.
        /// </summary>
        /// <param name="key">Key of the resource.</param>
        public string GetText(string key)
        {
            using(Stream inStream = OpenRead(key))
            using (StreamReader inStreamReader = new StreamReader(inStream))
            {
                return inStreamReader.ReadToEnd();
            }
        }

        /// <summary>
        /// Gets complete text of the given resource.
        /// </summary>
        /// <param name="index">Index of the resource.</param>
        public string GetText(int index)
        {
            using (Stream inStream = OpenRead(index))
            using (StreamReader inStreamReader = new StreamReader(inStream))
            {
                return inStreamReader.ReadToEnd();
            }
        }

        /// <summary>
        /// Gets all bytes of the given resource.
        /// </summary>
        /// <param name="key">Key of the resource.</param>
        public byte[] GetBytes(string key)
        {
            using (Stream inStream = OpenRead(key))
            {
                byte[] result = new byte[(int)inStream.Length];
                inStream.Read(result, 0, (int)inStream.Length);
                return result;
            }
        }

        /// <summary>
        /// Gets all bytes of the given resource.
        /// </summary>
        /// <param name="index">Index of the resource.</param>
        public byte[] GetBytes(int index)
        {
            using (Stream inStream = OpenRead(index))
            {
                byte[] result = new byte[(int)inStream.Length];
                inStream.Read(result, 0, (int)inStream.Length);
                return result;
            }
        }

        /// <summary>
        /// Gets the target type
        /// </summary>
        public Type TargetType
        {
            get { return m_targetType; }
        }

        /// <summary>
        /// Gets the target assembly
        /// </summary>
        public Assembly TargetAssembly
        {
            get { return m_targetAssembly; }
        }

        /// <summary>
        /// Gets a collection contaning all resource files
        /// </summary>
        public ResourceInfoCollection ResourceFiles
        {
            get { return m_publicResources; }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// Custom collection class for AssemblyResourceReader
        /// </summary>
        public class ResourceInfoCollection : IEnumerable<AssemblyResourceInfo>
        {
            private AssemblyResourceReader m_owner;

            /// <summary>
            /// 
            /// </summary>
            public ResourceInfoCollection(AssemblyResourceReader owner)
            {
                m_owner = owner;
            }

            /// <summary>
            /// Is the given resource file available?
            /// </summary>
            public bool ContainsResourceFile(string key)
            {
                return m_owner.m_resourcesDict.ContainsKey(key);
            }

            /// <summary>
            /// IEnumerable implementation
            /// </summary>
            public IEnumerator<AssemblyResourceInfo> GetEnumerator()
            {
                return m_owner.m_resources.GetEnumerator();
            }

            /// <summary>
            /// IEnumerable implementation
            /// </summary>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return m_owner.m_resources.GetEnumerator();
            }

            /// <summary>
            /// Gets total count of resource files
            /// </summary>
            public int Count
            {
                get { return m_owner.m_resources.Count; }
            }

            /// <summary>
            /// Gets the AssemblyResourceInfo object at the given index
            /// </summary>
            public AssemblyResourceInfo this[int index]
            {
                get { return m_owner.m_resources[index]; }
            }

            /// <summary>
            /// Gets the AssemblyResourceInfo object with the given key
            /// </summary>
            public AssemblyResourceInfo this[string key]
            {
                get { return m_owner.m_resourcesDict[key]; }
            }
        }
    }
}
