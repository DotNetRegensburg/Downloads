using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RK.Common.GraphicsEngine.Objects.Construction
{
    public class MaterialProperties
    {
        /// <summary>
        /// Gets or sets the name of the material.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path to the texture.
        /// </summary>
        public string TextureName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the diffuse color component of this material.
        /// </summary>
        public Color4 DiffuseColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ambient color component.
        /// </summary>
        public Color4 AmbientColor 
        {
            get; 
            set;
        }

        public Color4 EmissiveColor 
        { 
            get; 
            set; 
        }

        public Color4 Specular 
        { 
            get; 
            set; 
        }

        public float Shininess 
        { 
            get; 
            set; 
        }
    }
}
