using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqToExcel;

namespace Grau
{
    public class ExcelReader
    {
        private readonly  string _fileName;

        public ExcelEntryLevelTemplate ExcelEntryLevelTemplate { get; set; }
        
        public ExcelReader(string fileName)
        {
            _fileName = fileName;
        }

        public void CreateMappingFiles()
        {
            var excel = new ExcelQueryFactory(_fileName);
            var sheetCount = excel.Worksheet().Count();
        }
    }

    public class ExcelEntryLevelTemplate
    {
    }
}
