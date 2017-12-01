using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace VsTools.Projects
{
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Returns the text value of the matching element in the dicionary, or null if the element name is not found
        /// </summary>
        /// <param name="elementDictionary"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string TryGetXElementValue(this IDictionary<string, XElement> elementDictionary, string name)
        {
            return elementDictionary.TryGetValue(name, out XElement value) ? value.Value : null;
        }

        /// <summary>
        /// Returns the text value of the matching element in the dicionary, throws an exception otherwise
        /// </summary>
        /// <param name="elementDictionary"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetXElementValue(this IDictionary<string, XElement> elementDictionary, string name)
        {
            return elementDictionary[name].Value;
        }

    }

    public abstract class CsProjectNode
    {
        private XNode _closingElementSpace;
        private string _condition;
        public abstract int Depth { get; }

        public virtual string ElementName
        {
            get { return GetType().Name; }
            set { throw new NotImplementedException(); }
        }

        protected const string MsbuildSchema = "http://schemas.microsoft.com/developer/msbuild/2003";

        public string Condition
        {
            get { return _condition; }
            set
            {
                AddOrUpdateAttribute("Condition", value);
                _condition = value;
            }
        }

        public XNode Node { get; set; }

        protected CsProjectNode()
        {
            Node = new XElement(XName.Get(ElementName, MsbuildSchema));
        }

        protected CsProjectNode(XNode node)
        {
            Node = node;

            var condition = Element.Attributes().FirstOrDefault(x => x.Name.LocalName == "Condition");
            if (condition != null)
            {
                Condition = condition.Value;
            }

            if (Element.Nodes().Any())
            {
                var lastNode = Element.Nodes().Last();
                if (lastNode.NodeType == XmlNodeType.Text)
                {
                    var lastNodeText = ((XText)lastNode).Value;
                    if (lastNodeText.Trim('\r', '\n', ' ') == string.Empty)
                    {
                        _closingElementSpace = lastNode;
                    }
                }
            }
        }

        public void AddAfterSelf(CsProjectNode node)
        {
            Node.AddAfterSelf(node.Node);
            node.Node.AddBeforeSelf(new XText("\r\n" + new string(' ', node.Depth * 2)));
        }

        public void RemoveAttribute(string name)
        {
            var attr = Element.Attributes().First(x => x.Name.LocalName == name);
            attr.Remove();
        }

        /// <summary>
        /// Adds an non-existing attribute, or updates an existing attribute, or removes the attribute if the value is set to null
        /// </summary>
        /// <param name="name">The name of the attribute to add, update or remove</param>
        /// <param name="value">The value of the attribute or null to remove the attribute</param>
        public void AddOrUpdateAttribute(string name, string value)
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

        public void AddOrUpdateElement(string name, string value)
        {
            var child = Element.Elements().FirstOrDefault(x => x.Name.LocalName == name);
            if (child != null)
            {
                child.Value = value;
            }
            else
            {
                if (_closingElementSpace == null)
                {
                    _closingElementSpace = new XText("\r\n" + new string(' ', (Depth) * 2));
                    Element.Add(_closingElementSpace);
                }
                child = new XElement(XName.Get(name, MsbuildSchema));
                child.Add(value);
                _closingElementSpace.AddBeforeSelf(child);
                child.AddBeforeSelf(new XText("\r\n" + new string(' ', (Depth + 1) * 2)));
            }
        }

        public void AddElement(XNode child)
        {
            if (_closingElementSpace == null)
            {
                _closingElementSpace = new XText("\r\n" + new string(' ', (Depth) * 2));
                Element.Add(_closingElementSpace);
            }
            _closingElementSpace.AddBeforeSelf(child);
            child.AddBeforeSelf(new XText("\r\n" + new string(' ', (Depth + 1) * 2)));
        }

        public Attribute GetAttribute(string name)
        {
            return new Attribute(Element.Attribute(name));
        }

        public Element GetChildElement(string name)
        {
            return new Element(Element.Element(XName.Get(name, MsbuildSchema)), Depth + 1);
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

        public XElement Element => (XElement)Node;

        /// <summary>
        /// Retrieves the xml elements contained within the current node as a dictionary
        /// </summary>
        /// <returns></returns>
        protected IDictionary<string, XElement> GetXElements()
        {
            return ((XElement)Node).Elements().ToDictionary(x => x.Name.LocalName);
        }

    }
}