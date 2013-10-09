using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Category_I;
using Grau.Setup;
using LinqToExcel;

namespace Grau
{
    class Program
    {


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

            var excel1 = new ExcelQueryFactory("F:\\QRDA\\CodeSystems.xlsx");
            excel1.AddMapping<CodeSystemMap>(c => c.CodeSystemName, "Code System Name");
            excel1.AddMapping<CodeSystemMap>(c => c.CodeSystemOid, "Code System OID");
            var codeSystemsUsed = excel1.Worksheet<CodeSystemMap>().ToList();

        }

    }



}
