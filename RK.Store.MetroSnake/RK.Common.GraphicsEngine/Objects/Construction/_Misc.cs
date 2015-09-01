
namespace RK.Common.GraphicsEngine.Objects.Construction
{
    /// <summary>
    /// Delegate used for accessing a tesselation function
    /// </summary>
    public delegate Vector3 TesselationFunction(float u, float v);

    /// <summary>
    /// Enumeration containing all components of texture coordinate
    /// </summary>
    public enum TextureCoordinateComponent
    {
        /// <summary>
        /// U component of a texture coordinate
        /// </summary>
        U,

        /// <summary>
        /// V component of a texture coordinate
        /// </summary>
        V
    }

    public enum FitToCuboidMode
    {
        MaintainAspectRatio,

        Stretch
    }

    public enum FitToCuboidOrigin
    {
        Center,

        LowerCenter
    }
}
