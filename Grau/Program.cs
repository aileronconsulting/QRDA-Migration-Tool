using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Category_I;
using LinqToExcel;

namespace Grau
{
    class Program
    {
        public class TemplateIdMap
        {
            public string TemplateTitle { get; set; }
            public string TemplateType { get; set; }
            public string TemplateId { get; set; }
        }

        [STAThread]
        static void Main(string[] args)
        {
            var reader =new ExcelReader("F:\\QRDA\\Entries.xlsx");
            reader.CreateMappingFiles();
            var templates = reader.ExcelEntryLevelTemplates;
            var usedByLists = templates.Select(x => x.UsedBy).Aggregate((f, s) => f.Union(s).ToList());
            var usedBys = new HashSet<string>(usedByLists);
            var containsEntries = templates.Select(x => x.ContainEntries).Aggregate((f, s) => f.Union(s).ToList());
            var containEntry = new HashSet<string>(containsEntries);

            var excel = new ExcelQueryFactory("F:\\QRDA\\TemplateIds.xlsx");
            excel.AddMapping<TemplateIdMap>(t => t.TemplateId, "templateId");
            excel.AddMapping<TemplateIdMap>(t => t.TemplateTitle, "Template Title");
            excel.AddMapping<TemplateIdMap>(t => t.TemplateType, "Template Type");
            var templateIds = excel.Worksheet<TemplateIdMap>().ToList();


            /*
                     [XmlElement("act", typeof (POCD_MT000040Act))]
        [XmlElement("encounter", typeof (POCD_MT000040Encounter))]
        [XmlElement("observation", typeof (POCD_MT000040Observation))]
        [XmlElement("observationMedia", typeof (POCD_MT000040ObservationMedia))]
        [XmlElement("organizer", typeof (POCD_MT000040Organizer))]
        [XmlElement("procedure", typeof (POCD_MT000040Procedure))]
        [XmlElement("regionOfInterest", typeof (POCD_MT000040RegionOfInterest))]
        [XmlElement("substanceAdministration",
            typeof (POCD_MT000040SubstanceAdministration))]
        [XmlElement("supply", typeof (POCD_MT000040Supply))]

             */

        }

    }
}
