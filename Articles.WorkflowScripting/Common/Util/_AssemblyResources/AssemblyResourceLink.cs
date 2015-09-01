using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Util
{
    public class AssemblyResourceLink
    {
        private Assembly m_targetAssembly;
        private string m_resourcePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyResourceLink"/> class.
        /// </summary>
        /// <param name="targetAssembly">The target assembly.</param>
        /// <param name="resourcePath">The resource path.</param>
        public AssemblyResourceLink(Assembly targetAssembly, string resourcePath)
        {
            m_targetAssembly = targetAssembly;
            m_resourcePath = resourcePath;
        }

        /// <summary>
        /// Opens the resource for reading.
        /// </summary>
        public Stream OpenRead()
        {
            return m_targetAssembly.GetManifestResourceStream(m_resourcePath);
        }

        /// <summary>
        /// Gets the raw resource in text form.
        /// </summary>
        public string GetText()
        {
            Stream inStream = null;
            StreamReader inStreamReader = null;
            try
            {
                inStream = OpenRead();
                inStreamReader = new StreamReader(inStream);

                return inStreamReader.ReadToEnd();
            }
            finally
            {
                if (inStreamReader != null) { inStreamReader.Close(); }
                if (inStream != null) { inStream.Close(); }
            }
        }

        /// <summary>
        /// Gets the target assembly.
        /// </summary>
        public Assembly TargetAssembly
        {
            get { return m_targetAssembly; }
        }

        /// <summary>
        /// Gets the resource path.
        /// </summary>
        public string ResourcePath
        {
            get { return m_resourcePath; }
        }
    }
}
