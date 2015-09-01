using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.Util
{
    public class AssemblyResourceInfo
    {
        private Assembly m_targetAssembly;
        private string m_resourcePath;
        private string m_key;

        /// <summary>
        /// Creates a new AssemblyResourceInfo object
        /// </summary>
        internal AssemblyResourceInfo(Assembly targetAssembly, string resourcePath, string key)
        {
            m_targetAssembly = targetAssembly;
            m_resourcePath = resourcePath;
            m_key = key;
        }

        /// <summary>
        /// Opens a reading stream
        /// </summary>
        public Stream OpenRead()
        {
            return m_targetAssembly.GetManifestResourceStream(m_resourcePath);
        }

        /// <summary>
        /// Gets the path to the resource
        /// </summary>
        public string ResourcePath
        {
            get { return m_resourcePath; }
        }

        /// <summary>
        /// Gets the target assembly
        /// </summary>
        public Assembly TargetAssembly
        {
            get { return m_targetAssembly; }
        }

        /// <summary>
        /// Gets the key of this object
        /// </summary>
        public string Key
        {
            get { return m_key; }
        }
    }
}
