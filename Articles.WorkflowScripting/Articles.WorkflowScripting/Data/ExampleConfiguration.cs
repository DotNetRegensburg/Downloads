using System.Xml.Serialization;
using Common.GraphicsEngine.Core;

namespace Articles.WorkflowScripting.Data
{
    [XmlType("example-configuration", Namespace="http://www.rkoenig.eu/examples")]
    public class ExampleConfiguration
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleConfiguration"/> class.
        /// </summary>
        public ExampleConfiguration()
        {

        }

        /// <summary>
        /// Gets or sets the TargetHardware.
        /// </summary>
        [XmlAttribute("target-hardware")]
        private TargetHardware TargetHardware
        {
            get;
            set;
        }
    }
}
