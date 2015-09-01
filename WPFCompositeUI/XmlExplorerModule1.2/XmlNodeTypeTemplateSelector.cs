using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace XmlExplorerModule
{
    public class XmlNodeTypeTemplateSelector : DataTemplateSelector
    {
        public static readonly string UnknownTemplateKey = "Unknown";

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element == null)
            {
                return null;
            }

            XmlNode node = item as XmlNode;
            string key = (node != null) ? node.NodeType.ToString() : UnknownTemplateKey;
            DataTemplate template = element.TryFindResource<DataTemplate>(key);
            return template;
        }
    }
}
