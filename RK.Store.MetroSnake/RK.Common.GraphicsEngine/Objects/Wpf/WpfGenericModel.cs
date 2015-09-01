using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RK.Common.GraphicsEngine.Objects.Construction;
using RK.Common.GraphicsEngine.Objects.ObjectTypes;

namespace RK.Common.GraphicsEngine.Objects.Wpf
{
    public class WpfGenericModel : WpfConstructed3DModel
    {
        public static readonly DependencyProperty ObjectTypeProperty =
            DependencyProperty.Register("ObjectType", typeof(ObjectType), typeof(WpfGenericModel), new PropertyMetadata(null));

        public ObjectType ObjectType
        {
            get { return (ObjectType)GetValue(ObjectTypeProperty); }
            set { SetValue(ObjectTypeProperty, value); }
        }

        public override VertexStructure[] BuildStructures()
        {
            if (this.ObjectType != null) { return ObjectType.BuildStructure(); }
            else { return null; }
        }
    }
}
