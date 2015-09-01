using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using Common.GraphicsEngine.Core;

//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Drawing3D.Resources
{
    public class BlendStateResource : Resource
    {
        //Direct3D 11 resources
        private D3D11.BlendState m_blendState;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendStateResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        public BlendStateResource(string name)
            : base(name)
        {

        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

            D3D11.BlendStateDescription descriptor = new D3D11.BlendStateDescription();
            descriptor.AlphaToCoverageEnable = false;
            descriptor.IndependentBlendEnable = false;
            for (int loop = 0; loop < descriptor.RenderTargets.Length; loop++)
            {
                descriptor.RenderTargets[loop] = new D3D11.RenderTargetBlendDescription()
                {
                    BlendEnable = true,
                    BlendOperation = D3D11.BlendOperation.Add,
                    BlendOperationAlpha = D3D11.BlendOperation.Add,
                    SourceBlendAlpha = D3D11.BlendOption.Zero,
                    DestinationBlendAlpha = D3D11.BlendOption.Zero,
                    SourceBlend = D3D11.BlendOption.BlendFactor,
                    DestinationBlend = D3D11.BlendOption.InverseBlendFactor,
                    RenderTargetWriteMask = D3D11.ColorWriteMaskFlags.All
                };
            }
            m_blendState = D3D11.BlendState.FromDescription(device, descriptor);
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            m_blendState = GraphicsHelper.DisposeGraphicsObject(m_blendState);
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        /// <value></value>
        public override bool IsLoaded
        {
            get { return m_blendState != null; }
        }

        /// <summary>
        /// Gets the BlendState object.
        /// </summary>
        public D3D11.BlendState BlendState
        {
            get { return m_blendState; }
        }
    }
}
