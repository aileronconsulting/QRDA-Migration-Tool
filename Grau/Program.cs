using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using Category_I;

namespace Grau
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var anyType = typeof(POCD_MT000040Section);
            var customAttributes = (IEnumerable<XmlIncludeAttribute>) anyType.GetCustomAttributes(typeof(XmlIncludeAttribute));
            foreach (var customAttribute in customAttributes)
            {
                Console.WriteLine(customAttribute.Type.Name);
            }


        }

    }
}
