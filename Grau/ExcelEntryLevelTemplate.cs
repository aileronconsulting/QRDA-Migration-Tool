using System.Collections.Generic;

namespace Grau
{
    public class ExcelEntryLevelTemplate
    {
        public string Name { get; set; }

        public Dictionary<string, List<string>> UsedByTable { get; set; }
 


    }
}