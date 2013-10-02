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
            var cdaType = Assembly.GetExecutingAssembly().GetType("Category_I.POCD_MT000040ClinicalDocument");
            var props = cdaType.GetProperties();
//            foreach (var propertyInfo in props)
//            {
//                Console.WriteLine(propertyInfo.Name);
//            }

            var anyType = typeof (ANY);
            var customAttributes = (IEnumerable<XmlIncludeAttribute>) anyType.GetCustomAttributes(typeof(XmlIncludeAttribute));
            foreach (var customAttribute in customAttributes)
            {
                Console.WriteLine(customAttribute.Type.Name);
            }


        }

    }
}
