using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VsTools.Projects
{
    public class ItemGroup : ProjectChildElement
    {
        public ItemGroup()
        {
        }

        public ItemGroup(XNode node) : base(node)
        {

        }

        public void Add(Item content)
        {
            AddElement(content.Node);
        }

        public IEnumerable<Item> Items
        {
            get
            {
                // I know creating new objects every time you access the contents is very inefficient
                return Element.Elements()
                    .Select(x =>
                    {
                        var type = Reflection.GetItemGroupContentTypeFromName(x.Name.LocalName);
                        return (Item) Activator.CreateInstance(type, new object[] {x});
                    });
            }
        }

    }
}