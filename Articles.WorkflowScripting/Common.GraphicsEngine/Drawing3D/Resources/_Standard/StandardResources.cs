using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Util;

namespace Common.GraphicsEngine.Drawing3D.Resources
{
    public static class StandardResources
    {
        public const string SimpleRenderingEffectName = "Default.SimpleRendering";
        public const string SimpleDisplacedRenderingEffectName = "Default.SimpleDisplacedRendering";
        public const string SimpleNormalMappedRenderingEffectName = "Default.SimpleNormalMappedRendering";
        public const string SimpleTransformedTexturedRenderingName = "Default.SimpleTransformedTexturedRendering";
        public const string TexturePainterName = "Default.TexturePainter";

        /// <summary>
        /// Adds and loads the standard resource with the given name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static T AddAndLoadResource<T>(ResourceDictionary target, string resourceName)
            where T : Resource
        {
            //Get existing resource
            if (target.ContainsResource(resourceName)) { return target.GetResourceAndEnsureLoaded<T>(resourceName); }

            //Create resource object
            Resource newResource = null;
            switch (resourceName)
            {
                case SimpleRenderingEffectName:
                    newResource = new EffectResource(
                        SimpleRenderingEffectName,
                        "fx_5_0",
                        new AssemblyResourceLink(
                            Assembly.GetExecutingAssembly(),
                            "Common.GraphicsEngine.Resources.Effects.SimpleRendering.fx"));
                    break;

                case SimpleTransformedTexturedRenderingName:
                    newResource = new EffectResource(
                        SimpleTransformedTexturedRenderingName,
                        "fx_5_0",
                        new AssemblyResourceLink(
                            Assembly.GetExecutingAssembly(),
                            "Common.GraphicsEngine.Resources.Effects.SimpleTransformedTexturedRendering.fx"));
                    break;

                case SimpleDisplacedRenderingEffectName:
                    newResource = new EffectResource(
                        SimpleDisplacedRenderingEffectName,
                        "fx_5_0",
                        new AssemblyResourceLink(
                            Assembly.GetExecutingAssembly(),
                            "Common.GraphicsEngine.Resources.Effects.SimpleDisplacedRendering.fx"));
                    break;

                case SimpleNormalMappedRenderingEffectName:
                    newResource = new EffectResource(
                        SimpleNormalMappedRenderingEffectName,
                        "fx_5_0",
                        new AssemblyResourceLink(
                            Assembly.GetExecutingAssembly(),
                            "Common.GraphicsEngine.Resources.Effects.SimpleNormalMappedRendering.fx"));
                    break;

                case TexturePainterName:
                    newResource = new TexturePainterResource(TexturePainterName);
                    break;
            }

            //Cast resource to requested type
            T result = newResource as T;
            if(result == null){ throw new ArgumentException("Unable to create standard resource!", "resourceName"); }

            //Add and load the resource
            target.AddAndLoadResource(result);
            return result;
        }
    }
}
