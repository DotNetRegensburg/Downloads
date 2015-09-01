using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.Util
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class AssemblyResourceFileAttribute : Attribute
    {
        private string m_resourcePath;
        private string m_key;

        /// <summary>
        /// Links the given resource
        /// </summary>
        public AssemblyResourceFileAttribute(string resourcePath)
        {
            m_resourcePath = resourcePath;
            m_key = null;
        }

        /// <summary>
        /// Links the given resource with the given key
        /// </summary>
        public AssemblyResourceFileAttribute(string resourcePath, string key)
        {
            m_resourcePath = resourcePath;
            m_key = key;
        }

        /// <summary>
        /// Gets the path to the resource
        /// </summary>
        public string ResourcePath
        {
            get { return m_resourcePath; }
        }

        /// <summary>
        /// Gets the key (may be null)
        /// </summary>
        public string Key
        {
            get { return m_key; }
        }
    }
}
