using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Property
    {
        public XNode Node { get; set; }
        public XElement Element => (XElement)Node;
        public virtual string ElementName => GetType().Name;

        public string Condition
        {
            get => GetAttributeValue();
            set => AddOrUpdateAttribute(value);
        }

        public Property(string propertyName, string value)
        {
            Node = new XElement(propertyName);
            Element.Add(value);
        }

        protected Property(string value)
        {
            Node = new XElement(ElementName);
            Element.Add(new XText(value));
        }

        public Property(XNode node)
        {
            var type = GetType();

            if (type != typeof(Property) && ((XElement)node).Name.LocalName != ElementName)
            {
                throw new Exception($"Element does not match expected type {type.Name}");
            }
            Node = node;
        }

        public string Value
        {
            get => ((XText)Element.FirstNode)?.Value;
            set
            {
                if ((Element.FirstNode) == null)
                {
                    Element.Add(new XText(value));
                }
                else
                {
                    ((XText)Element.FirstNode).Value = value;
                }
            }
        }


        public void RemoveAttribute(string name)
        {
            var attr = Element.Attributes().First(x => x.Name.LocalName == name);
            attr.Remove();
        }

        public string GetAttributeValue([CallerMemberName] string name = null)
        {
            var attr = Element.Attributes().First(x => x.Name.LocalName == name);
            return attr.Value;
        }

        /// <summary>
        /// Adds an non-existing attribute, or updates an existing attribute, or removes the attribute if the value is set to null
        /// </summary>
        /// <param name="name">The name of the attribute to add, update or remove</param>
        /// <param name="value">The value of the attribute or null to remove the attribute</param>
        public void AddOrUpdateAttribute(string value, [CallerMemberName] string name = null)
        {
            var attr = Element.Attributes().FirstOrDefault(x => x.Name.LocalName == name);
            if (attr != null)
            {
                if (value == null)
                {
                    RemoveAttribute(name);
                }
                else
                {
                    attr.Value = value;
                }
            }
            else
            {
                if (value == null)
                {
                    throw new Exception($"Cannot remove non-existant attribute {name}");
                }
                attr = new XAttribute(XName.Get(name), value);
                Element.Add(attr);
            }
        }

    }
}