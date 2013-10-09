using System;
using System.Collections.Generic;
using System.Linq;
using Category_I;
using LinqToExcel;

namespace Grau.Setup
{
    public class TemplateIdMap
    {
        public string TemplateTitle { get; set; }
        public string TemplateType { get; set; }
        public string TemplateId { get; set; }
    }


    public class CodeSystemMap
    {
        public string CodeSystemName { get; set; }
        public string CodeSystemOid { get; set; }
    }

    public class ExcelReader
    {
        private readonly  string _fileName;

        public List<EntryLevelTemplate> ExcelEntryLevelTemplates { get; set; }



        public ExcelReader(string fileName)
        {
            _fileName = fileName;
        }

        public void CreateMappingFiles()
        {
            var excel = new ExcelQueryFactory(_fileName);
            var sheetNames = excel.GetWorksheetNames();
            ExcelEntryLevelTemplates = sheetNames.Select(name =>
                {
                    var sheet = excel.WorksheetNoHeader((string) name);
                    var rows = sheet.ToList();
                    var entry = new EntryLevelTemplate
                        {
                            Name = rows[0][0].Value.ToString(),
                            Info = rows[1][0].Value.ToString(),
                            UsedBy = new List<string>(),
                            ContainEntries = new List<string>()
                        };

                    const int startRow = 2;
                    //used by table
                    var inUsedByTable = false;
                    var inConstraintTable = false;
                    var firstRowOfConstraintTable = 0;
                    for (var i = startRow; i < rows.Count; ++i)
                    {
                        var row = rows[i];
                        var col1Val = row[0].Value.ToString().Trim();
                        var col2Val = row[1].Value.ToString().Trim();
                        if (col1Val.Equals("Used By:") && col2Val.Equals("Contains Entries:"))
                        {
                            inUsedByTable = true;
                            continue;
                        }
                        if (col1Val.Equals("Name") && col2Val.Equals("XPath"))
                        {
                            inConstraintTable = true;
                            firstRowOfConstraintTable = i + 1;
                            inUsedByTable = false;
                            break;
                        }
                        if (!inUsedByTable) continue;
                        if (!string.IsNullOrEmpty(col1Val))
                        {
                            entry.UsedBy.Add(col1Val);
                        }
                        if (!string.IsNullOrEmpty(col2Val))
                        {
                            entry.ContainEntries.Add(col2Val);
                        }
                    }

                    if (!inUsedByTable && inConstraintTable)
                    {
                        entry.Info2Name = rows[firstRowOfConstraintTable][0].Value.ToString();
                        entry.Info2 = rows[firstRowOfConstraintTable][1].Value.ToString();
                        firstRowOfConstraintTable++;
                        entry.InfoDetail = new List<EntryLevelTemplateDetail>();
                        for (var i = firstRowOfConstraintTable; i < rows.Count; ++i)
                        {
                            var row = rows[i];
                            var detail = new EntryLevelTemplateDetail
                                {
                                    Name = row[0],
                                    Xpath = row[1],
                                    Cardinality = row[2],
                                    Verb = row[3],
                                    DataType = row[4],
                                    Conf = row[5],
                                    FixedValue = row[6]
                                };
                            entry.InfoDetail.Add(detail);
                        }
                    }
                    return entry;
                }).ToList();
        }
    }
}
