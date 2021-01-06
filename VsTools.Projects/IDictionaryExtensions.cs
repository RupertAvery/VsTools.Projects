using System.Collections.Generic;
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
}