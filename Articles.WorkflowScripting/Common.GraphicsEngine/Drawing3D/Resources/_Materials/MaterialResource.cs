using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Drawing3D.Resources
{
    public abstract class MaterialResource : Resource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        public MaterialResource(string name)
            : base(name)
        {

        }

        /// <summary>
        /// Applies the material to the given render state.
        /// </summary>
        /// <param name="renderState">Current render state</param>
        public virtual void Apply(RenderState renderState) { }

        /// <summary>
        /// Discards the material in current render state.
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        public virtual void Discard(RenderState renderState) { }

        /// <summary>
        /// Generates the requested input layout.
        /// </summary>
        /// <param name="inputElements">An array of InputElements describing vertex input structure.</param>
        public abstract D3D11.InputLayout GenerateInputLayout(D3D11.InputElement[] inputElements);

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        /// <value></value>
        public override bool IsLoaded
        {
            get { return true; }
        }
    }
}
