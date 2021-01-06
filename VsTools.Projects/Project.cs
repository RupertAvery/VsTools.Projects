using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Project
    {
        internal static XNamespace MsbuildSchema = "http://schemas.microsoft.com/developer/msbuild/2003";

        private readonly XDocument _source;
        private string _sourceFile;

        public bool Is2017Project => _source.Root.Attribute("Sdk") != null;

        /// <summary>
        /// Instantiates a new Project
        /// </summary>
        private Project(bool is2017Project, string toolsVersion = "15.0")
        {
            _source = new XDocument();
            var root = new ProjectRoot(is2017Project, toolsVersion);
            _source.Add(root.Node);
        }

        public static Project CreatePreVS2017Project(string toolsVersion = "15.0")
        {
            return new Project(false, toolsVersion);
        }
        
        public static Project CreateVS2017Project()
        {
            return new Project(true);
        }

        /// <summary>
        /// Instantiates a Project from an XML document
        /// </summary>
        /// <param name="source"></param>
        public Project(XDocument source)
        {
            _source = source;
        }



        /// <summary>
        /// Loads a .csporj file and returns a new Project instance
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Project Load(string file)
        {
            var xml = XDocument.Load(file, LoadOptions.PreserveWhitespace);
            if (xml.Root.Name.LocalName != "Project")
            {
                throw new Exception("Invalid Project file, root element not found!");
            }

            var project = new Project(xml) { _sourceFile = file };

            return project;
        }

        public void Add(ProjectChildElement element)
        {
            new ProjectRoot(_source.Root).AddChild(element);
        }

        public IEnumerable<ItemGroup> ItemGroups
        {
            get
            {
                return _source.Root.Elements().Where(x => x.Name.LocalName == "ItemGroup").Select(x => new ItemGroup(x));
            }
        }

        public IEnumerable<Import> Imports
        {
            get
            {
                return _source.Root.Elements().Where(x => x.Name.LocalName == "Import").Select(x => new Import(x));
            }
        }

        public IEnumerable<PropertyGroup> PropertyGroups
        {
            get
            {
                return _source.Root.Elements().Where(x => x.Name.LocalName == "PropertyGroup").Select(x => new PropertyGroup(x));
            }
        }


        /// <summary>
        /// Returns a <see cref="Item"/> instance with an Include matching the given string, or null otherwise
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="include"></param>
        /// <returns></returns>
        public T FindItemGroupContent<T>(string include) where T : Item, new()
        {
            var node =
                _source.DescendantNodes()
                    .Where(x => x is XElement)
                    .FirstOrDefault(
                        x =>
                            ((XElement)x).Name.LocalName == typeof(T).Name &&
                            ((XElement)x).Attributes().First(y => y.Name.LocalName == "Include").Value == include);

            if (node == null) return default(T);

            return (T)Activator.CreateInstance(typeof(T), new object[] { node });
        }

        /// <summary>
        /// Returns true if an <see cref="Item"/> with a matching Include exists in the Project
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="include"></param>
        /// <returns></returns>
        public bool ItemGroupContentExists<T>(string include) where T : Item
        {
            return _source.DescendantNodes()
                .Where(x => x is XElement)
                .FirstOrDefault(
                    x =>
                        ((XElement)x).Name.LocalName == typeof(T).Name &&
                        ((XElement)x).Attributes().First(y => y.Name.LocalName == "Include").Value == include) != null;
        }

        /// <summary>
        /// Returns a <see cref="Item"/> instance with an Include matching the given string, or null otherwise
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="include"></param>
        /// <returns></returns>
        public bool ProjectReferenceExists(string include)
        {
            return _source.DescendantNodes()
                .Where(x => x is XElement)
                .FirstOrDefault(
                    x =>
                        ((XElement)x).Name.LocalName == "ProjectReference" &&
                        ((XElement)x).Attributes().First(y => y.Name.LocalName == "Include").Value == include) != null;
        }

        /// <summary>
        /// Returns a <see cref="ProjectReference"/> instance with an Include matching the given string, or null otherwise
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="include"></param>
        /// <returns></returns>
        public ProjectReference FindProjectReference(string include)
        {
            var node =
                _source.DescendantNodes()
                    .Where(x => x is XElement)
                    .FirstOrDefault(
                        x =>
                            ((XElement)x).Name.LocalName == "ProjectReference" &&
                            ((XElement)x).Attributes().First(y => y.Name.LocalName == "Include").Value == include);

            if (node == null) return null;

            return new ProjectReference(node);
        }

        /// <summary>
        /// Saves the Project
        /// </summary>
        public void Save()
        {
            if (_sourceFile != null)
            {
                SaveAs(_sourceFile);
            }
            else
            {
                throw new Exception("Project file was created with Load(). Please use Save(file)");
            }
        }

        /// <summary>
        /// Saves the Project as an Xml Document
        /// </summary>
        /// <param name="file"></param>
        public void SaveAs(string file)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
            };

            if (!string.IsNullOrEmpty(_source.Root.Name.Namespace.NamespaceName))
            {
                SetDefaultXmlNamespace(_source.Root, _source.Root.Name.Namespace);
            }

            using (XmlWriter writer = XmlWriter.Create(file, settings))
            {
                _source.Save(writer);
            }
        }

        /// <summary>
        /// Saves the Project to the given Stream
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream stream)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
            };

            if (!string.IsNullOrEmpty(_source.Root.Name.Namespace.NamespaceName))
            {
                SetDefaultXmlNamespace(_source.Root, _source.Root.Name.Namespace);
            }

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                _source.Save(writer);
            }
        }

        /// <summary>
        /// Returns the underlying Xml Document
        /// </summary>
        /// <returns></returns>
        public XDocument ToXml()
        {
            return _source;
        }

        protected void SetDefaultXmlNamespace(XElement xelem, XNamespace xmlns)
        {
            if (xelem.Name.NamespaceName == string.Empty)
                xelem.Name = xmlns + xelem.Name.LocalName;
            foreach (var e in xelem.Elements())
                SetDefaultXmlNamespace(e, xmlns);
        }
    }
}