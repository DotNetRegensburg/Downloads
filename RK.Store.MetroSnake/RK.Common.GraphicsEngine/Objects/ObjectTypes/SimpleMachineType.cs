using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RK.Common.GraphicsEngine.Objects.Construction;

namespace RK.Common.GraphicsEngine.Objects.ObjectTypes
{
    public class SimpleMachineType : ObjectType
    {
        public string MachineMaterial
        {
            get;
            set;
        }

        public string ColumnMaterial
        {
            get;
            set;
        }

        /// <summary>
        /// Builds all vertex structures needed for this object.
        /// </summary>
        public override VertexStructure[] BuildStructure()
        {
            //Build the machine
            VertexStructure[] machineStructures = new VertexStructure[2];
            machineStructures[0] = new VertexStructure();
            machineStructures[0].BuildCube24V(new Vector3(-1.5f, 0f, -1.5f), new Vector3(3f, 3f, 3f), Color4.White);
            machineStructures[0].BuildCube24V(new Vector3(-3.5f, 0f, -0.7f), new Vector3(2f, 2f, 1.4f), Color4.White);
            machineStructures[0].BuildCube24V(new Vector3(1.5f, 0f, -0.7f), new Vector3(2f, 2f, 1.4f), Color4.White);
            machineStructures[0].Material = this.MachineMaterial;
            machineStructures[1] = new VertexStructure();
            machineStructures[1].BuildCube24V(new Vector3(-1.5f, 0f, -1.5f), 0.1f, 3.01f, Color4.White);
            machineStructures[1].BuildCube24V(new Vector3(1.5f, 0f, -1.5f), 0.1f, 3.01f, Color4.White);
            machineStructures[1].BuildCube24V(new Vector3(1.5f, 0f, 1.5f), 0.1f, 3.01f, Color4.White);
            machineStructures[1].BuildCube24V(new Vector3(-1.5f, 0f, 1.5f), 0.1f, 3.01f, Color4.White);
            machineStructures[1].Material = this.ColumnMaterial;

            return machineStructures;
        }
    }
}
