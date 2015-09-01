using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RK.Wpf3DSampleBrowser
{
    public static class Infrastructure
    {
        public const string SAMPLE_CONTRACT = "SampleContract";

        private static AssemblyCatalog s_assemblyCatalog;
        private static CompositionContainer s_compositionContainer;

        /// <summary>
        /// Handles all imports of the given object.
        /// </summary>
        /// <param name="target">The target object.</param>
        public static void Compose(object target)
        {
            if (s_assemblyCatalog == null) { s_assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly()); }
            if (s_compositionContainer == null) { s_compositionContainer = new CompositionContainer(s_assemblyCatalog); }

            s_compositionContainer.ComposeParts(target);
        }
    }
}
