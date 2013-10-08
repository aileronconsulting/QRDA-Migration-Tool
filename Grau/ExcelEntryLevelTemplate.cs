using System.Collections.Generic;

namespace Grau
{
    public class ExcelEntryLevelTemplate
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public List<string> UsedBy { get; set; }
        public List<string> ContainEntries { get; set; } 
        public string Info2 { get; set; }
        public List<ExcelEntryLevelTemplateDetail> InfoDetail { get; set; }
        public string Info2Name { get; set; }
    }

    public class ExcelEntryLevelTemplateDetail
    {
        public string Name { get; set; }
        public string Xpath { get; set; }
        public string Cardinality { get; set; }
        public string Verb { get; set; }
        public string DataType { get; set; }
        public string Conf { get; set; }
        public string FixedValue { get; set; }
    }
}