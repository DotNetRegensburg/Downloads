using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.GraphicsEngine.Objects.Construction;
using Common.GraphicsEngine.Objects.ObjectTypes;

namespace Common.GraphicsEngine.Objects.Loaders
{
    public static class XFileLoader
    {

        /// <summary>
        /// Reads an XFileDocument from the given stream.
        /// </summary>
        /// <param name="inStream">Stream to read from.</param>
        private static XFileDocument ReadDocument(StreamReader inStream)
        {
            //Check starting string
            string xofSign = ReadString(inStream, 4);
            if (xofSign != "xof ") { throw new GraphicsLibraryException("Unable to read x-file: xof not found!"); }

            //Read version numbers
            XFileDocument result = new XFileDocument();
            result.VersionMajor = Int32.Parse(ReadString(inStream, 2));
            result.VersionMinor = Int32.Parse(ReadString(inStream, 2));

            //Check format type
            string formatType = ReadString(inStream, 4);
            if (formatType != "txt ") { throw new GraphicsLibraryException("Unable to read x-file: Format not supported!"); }

            //Read floatingpoint precission 
            ReadString(inStream, 4);

            //Start reading loop
            Stack<Object> readingStack = new Stack<object>();
            string actLine = string.Empty;
            while ((actLine = inStream.ReadLine()) != null)
            {
                //Remove all comments from current line
                int comment1Index = actLine.IndexOf('#');
                int comment2Index = actLine.IndexOf("//");
                if (comment1Index < 0) { comment1Index = Int32.MaxValue; }
                if (comment2Index < 0) { comment2Index = Int32.MaxValue; }
                int commentIndex = comment1Index < comment2Index ? comment1Index : comment2Index;
                if (commentIndex != Int32.MaxValue)
                {
                    actLine = actLine.Substring(0, comment1Index);
                }

                       
            }

            return result;
        }

        //private void ProcessWord(string word, XFileDocument xDocument)
        //{

        //}

        /// <summary>
        /// Reads given count of bytes from given stream.
        /// </summary>
        /// <param name="inStream">Stream to read from.</param>
        /// <param name="signCount">Total count of signs to read.</param>
        private static string ReadString(StreamReader inStream, int signCount)
        {
            char[] block = new char[signCount];
            if (inStream.ReadBlock(block, 0, signCount) < signCount) 
            {
                throw new GraphicsLibraryException("Unable to read " + signCount + " bytes");
            }
            return new string(block);
        }

        /// <summary>
        /// Reads the next word in the given string.
        /// </summary>
        private static string ReadWord(StreamReader inStream)
        {
            StringBuilder resultBuilder = new StringBuilder();

            int actByte = -1;
            while ((actByte = inStream.Read()) > -1)
            {
                char actChar = (char)actByte;
                if ((actChar == ' ') || (actChar == '\r') || (actChar == '\n')) 
                { 
                    break; 
                }
                else { resultBuilder.Append(actChar); }
            }
 
            return resultBuilder.ToString();
        }

        //private static string ReadWord(string currentLine)
        //{
        //    StringBuilder sBuilder = new StringBuilder();

        //    return null;
        //}

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// A class containing all data of an x-file.
        /// </summary>
        private class XFileDocument
        {
            private Dictionary<string, XTemplate> m_templates;
            private int m_versionMajor;
            private int m_versionMinor;

            /// <summary>
            /// Initializes a new instance of the <see cref="XFileDocument"/> class.
            /// </summary>
            public XFileDocument()
            {
                m_templates = new Dictionary<string, XTemplate>();
                m_versionMajor = 0;
                m_versionMinor = 0;
            }

            /// <summary>
            /// Gets a dictionary containing all templates within the file structure.
            /// </summary>
            public Dictionary<string, XTemplate> Templates
            {
                get { return m_templates; }
            }

            /// <summary>
            /// Gets or sets the major version.
            /// </summary>
            public int VersionMajor
            {
                get { return m_versionMajor; }
                set { m_versionMajor = value; }
            }

            /// <summary>
            /// Gets or sets the minor version.
            /// </summary>
            public int VersionMinor
            {
                get { return m_versionMinor; }
                set { m_versionMinor = value; }
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// Stores all data of a template.
        /// </summary>
        private class XTemplate
        {


        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// Stores all data of a frame.
        /// </summary>
        private class XFrame
        {

        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// Sores meta information about a field.
        /// </summary>
        private class XField
        {
            private string m_name;
            private XFieldType m_fieldType;
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// 
        /// </summary>
        private enum XFieldType
        {
            Primitive,
            Array
        }
    }
}
