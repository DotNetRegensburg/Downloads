using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Common.GraphicsEngine.Objects.Construction;
using Common.GraphicsEngine.Objects.Loaders;

namespace Common.GraphicsEngine.Objects.ObjectTypes
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
