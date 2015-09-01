using System;
using System.IO;
using System.Windows;
using RK.Common.GraphicsEngine.Objects.Construction;
using RK.Common.GraphicsEngine.Objects.Loaders;

#if WINRT
using Windows.ApplicationModel;
#endif

namespace RK.Common.GraphicsEngine.Objects.ObjectTypes
{
    public abstract class ObjectType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectType"/> class.
        /// </summary>
        public ObjectType()
        {

        }

        /// <summary>
        /// Builds all vertex structures needed for this object.
        /// </summary>
        public abstract VertexStructure[] BuildStructure();
        
        ///// <summary>
        ///// Loads an ObjectType from the given file path.
        ///// </summary>
        ///// <param name="filePath">The target file path.</param>
        //public static ObjectType FromFile(string filePath)
        //{
        //    return FromACFile(filePath);
        //}

        ///// <summary>
        ///// Loads an AC-File from the given path.
        ///// </summary>
        ///// <param name="targetFile">The target file path.</param>
        //public static ObjectType FromACFile(string targetFile)
        //{
        //    using (FileStream inStream = File.OpenRead(targetFile))
        //    {
        //        return FromACFile(inStream);
        //    }
        //}

        public static ObjectType FromACFile(string fileName)
        {
#if WINRT
            using (Stream inStream = Package.Current.InstalledLocation.OpenStreamForReadAsync(fileName).Result)
            {
                return FromACFile(inStream);
            }
#endif
#if DESKTOP
            using (Stream inStream = File.OpenRead(fileName))
            {
                return FromACFile(inStream);
            }
#endif
        }

#if DESKTOP
        /// <summary>
        /// Loads an object-type from given .ac file.
        /// </summary>
        public static ObjectType FromACFile(Uri resourceUri)
        {
            using (Stream inStream = Application.GetResourceStream(resourceUri).Stream)
            {
                return ObjectType.FromACFile(inStream);
            }
        }
#endif

        /// <summary>
        /// Loads an object-type from given .ac file.
        /// </summary>
        public static ObjectType FromACFile(byte[] rawBytes)
        {
            return ACFileLoader.ImportObjectType(rawBytes);
        }

        /// <summary>
        /// Loads an object-type from given .ac file.
        /// </summary>
        public static ObjectType FromACFile(Stream inStream)
        {
            return ACFileLoader.ImportObjectType(inStream);
        }
    }
}