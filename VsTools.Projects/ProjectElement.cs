using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace VsTools.Projects
{
    public abstract class ProjectElement
    {
        private XNode ClosingElementSpace
        {
            get
            {
                if (Element.Nodes().Any())
                {
                    var lastNode = Element.Nodes().Last();
                    if (lastNode.NodeType == XmlNodeType.Text)
                    {
                        var lastNodeText = ((XText)lastNode).Value;
                        if (lastNodeText.Trim('\r', '\n', ' ') == string.Empty)
                        {
                            return lastNode;
                        }
                    }
                }
                return null;
            }
        }

        public abstract int Depth { get; }
        public virtual string ElementName => GetType().Name;

        public XNode Node { get; set; }
        public XElement Element => (XElement)Node;

        public string Condition
        {
            get => GetAttributeValue();
            set => AddOrUpdateAttribute(value);
        }

        /// <summary>
        /// Creates an ProjectElement with an element with the specified name
        /// </summary>
        /// <param name="name"></param>
        protected ProjectElement(string name)
        {
            Node = new XElement(name);
        }

        protected ProjectElement()
        {
            Node = new XElement(ElementName);
        }

        protected ProjectElement(XName name)
        {
            Node = new XElement(name);
        }

        protected ProjectElement(XNode node)
        {
            Node = node;
        }

        public void AddChild(ProjectElement element)
        {
            if (ClosingElementSpace == null)
            {
                var closingElementSpace = new XText("\r\n" + new string(' ', (Depth) * 2));
                Element.Add(closingElementSpace);
            }
            Element.Add(new XText(new string(' ', 2)));
            Element.Add(element.Node);
            element.Node.AddAfterSelf(new XText("\r\n" + new string(' ', Depth * 2)));
        }

        public void AddChild(Property element)
        {
            if (ClosingElementSpace == null)
            {
                var closingElementSpace = new XText("\r\n" + new string(' ', (Depth) * 2));
                Element.Add(closingElementSpace);
            }
            Element.Add(new XText(new string(' ', 2)));
            Element.Add(element.Node);
            element.Node.AddAfterSelf(new XText("\r\n" + new string(' ', Depth * 2)));
        }


        public void AddBeforeSelf(ProjectElement element)
        {
            Node.AddBeforeSelf(element.Node);
            element.Node.AddAfterSelf(new XText("\r\n" + new string(' ', element.Depth * 2)));
        }

        public void AddAfterSelf(ProjectElement element)
        {
            Node.AddAfterSelf(element.Node);
            element.Node.AddBeforeSelf(new XText("\r\n" + new string(' ', element.Depth * 2)));
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

        public void AddAttribute(string name, string value)
        {
            var attr = new XAttribute(XName.Get(name), value);
            Element.Add(attr);
        }

        public void AddOrUpdateElement(string name, string value, string condition)
        {
            var child = Element.Elements().FirstOrDefault(x => x.Name.LocalName == name);
            if (child != null)
            {
                child.Value = value;
                child.SetAttributeValue("Condition", condition);
            }
            else
            {
                if (ClosingElementSpace == null)
                {
                    var closingElementSpace = new XText("\r\n" + new string(' ', (Depth) * 2));
                    Element.Add(closingElementSpace);
                }
                child = new XElement(name);
                child.Add(value);
                child.SetAttributeValue("Condition", condition);
                ClosingElementSpace.AddBeforeSelf(child);
                child.AddBeforeSelf(new XText("\r\n" + new string(' ', (Depth + 1) * 2)));
            }
        }

        public XElement GetElement(string name)
        {
            return Element.Element(name);
        }

        public void AddOrUpdateElement(string name, string value)
        {
            var child = Element.Elements().FirstOrDefault(x => x.Name.LocalName == name);
            if (child != null)
            {
                if (value == null)
                {
                    child.Remove();
                }
                else
                {
                    child.Value = value;
                }
            }
            else
            {
                if (ClosingElementSpace == null)
                {
                    var closingElementSpace = new XText("\r\n" + new string(' ', (Depth) * 2));
                    Element.Add(closingElementSpace);
                }
                child = new XElement(name);
                child.Add(value);
                ClosingElementSpace.AddBeforeSelf(child);
                child.AddBeforeSelf(new XText("\r\n" + new string(' ', (Depth + 1) * 2)));
            }
        }

        public void AddElement(XNode child)
        {
            if (ClosingElementSpace == null)
            {
                var closingElementSpace = new XText("\r\n" + new string(' ', (Depth) * 2));
                Element.Add(closingElementSpace);
            }
            ClosingElementSpace.AddBeforeSelf(child);
            child.AddBeforeSelf(new XText("\r\n" + new string(' ', (Depth + 1) * 2)));
        }

        public Attribute GetAttribute(string name)
        {
            return new Attribute(Element.Attribute(name));
        }

        public Element GetChildElement(string name)
        {
            return new Element(Element.Element(name), Depth + 1);
        }

        /// <summary>
        /// Returns the elements attributes. Only use this method if you are accessing custom attributes, otherwise use the properties on the current type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Attribute> GetAttributes()
        {
            return Element.Attributes().Select(x => new Attribute(x));
        }

        /// <summary>
        /// Returns the child elements. Only use this method if you are accessing custom elements, otherwise use the properties on the current type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Element> GetChildElements()
        {
            return Element.Elements().Select(x => new Element(x, Depth + 1));
        }



        /// <summary>
        /// Retrieves the xml elements contained within the current element as a dictionary
        /// </summary>
        /// <returns></returns>
        protected IDictionary<string, XElement> GetXElements()
        {
            return Element.Elements().ToDictionary(x => x.Name.LocalName);
        }

        public Property GetMetadata(string elementName)
        {
            var existing = Element.Elements().FirstOrDefault(x => x.Name.LocalName == elementName);
            return existing == null ? null : new Property(existing);
        }

        public bool HasMetadata(string elementName)
        {
            return Element?.Elements(elementName) != null;
        }

        public void SetMetadata(Property element)
        {
            var existing = Element.Elements().FirstOrDefault(x => x.Name.LocalName == element.ElementName);
            existing?.Remove();
            AddChild(element);
        }

        public void SetMetadata(string elementName, string value)
        {
            var existing = Element.Elements().FirstOrDefault(x => x.Name.LocalName == elementName);
            existing?.Remove();
            AddChild(new Property(elementName, value));
        }

        public void SetMetadata(string elementName, string value, string condition)
        {
            var existing = Element.Elements().FirstOrDefault(x => x.Name.LocalName == elementName);
            existing?.Remove();
            AddChild(new Property(elementName, value) { Condition = condition });
        }

        public void AddMetadata(string elementName, string value)
        {
            AddChild(new Property(elementName, value));
        }

        public void AddMetadata(string elementName, string value, string condition)
        {
            AddChild(new Property(elementName, value) { Condition = condition });
        }



    }
}