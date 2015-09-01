
using System.Xml.Serialization;
namespace Articles.Workflow.GuiCustomization.Data
{
    [XmlType(Namespace = DataCore.XML_NAMESPACE)]
    public class Delivery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Delivery"/> class.
        /// </summary>
        public Delivery()
        {

        }

        [XmlAttribute]
        public string Number
        {
            get;
            set;
        }

        [XmlAttribute]
        public string CustomerID
        {
            get;
            set;
        }

        [XmlAttribute]
        public string CustomerName
        {
            get;
            set;
        }

        [XmlAttribute]
        public decimal PriceForGames
        {
            get;
            set;
        }

        [XmlAttribute]
        public decimal PriceForHardware
        {
            get;
            set;
        }

        [XmlAttribute]
        public decimal PriceForSoftware
        {
            get;
            set;
        }

        [XmlAttribute]
        public decimal AvailableMoney
        {
            get;
            set;
        }

        [XmlAttribute]
        public decimal Sum
        {
            get { return AvailableMoney - (PriceForGames + PriceForHardware + PriceForSoftware); }
        }
    }
}
