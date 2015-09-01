using System.Collections.Generic;
using System.Xml.Serialization;

namespace Articles.Workflow.GuiCustomization.Data
{
    [XmlType(Namespace=XML_NAMESPACE)]
    public class DataCore
    {
        public const string XML_NAMESPACE = "http://www.rkoenig.eu/articles/workflow/gui-customization";

        private List<Delivery> m_deliveries;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataCore"/> class.
        /// </summary>
        public DataCore()
        {
            m_deliveries = new List<Delivery>();
        }

        /// <summary>
        /// Gets a list containing all deliveries.
        /// </summary>
        [XmlElement]
        public List<Delivery> Deliveries
        {
            get
            {
                return m_deliveries;
            }
        }
    }
}
