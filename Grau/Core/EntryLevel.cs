using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Category_I;
using Grau.Setup;

namespace Grau.Core
{
    public class EntryLevel
    {
        public Type Type { get; set; }
        public EntryLevelTemplate EntryLevelTemplate { get; set; }
        public Dictionary<string, Type> TypeByEntryType = new Dictionary<string, Type>
            {
                {"act", typeof (POCD_MT000040Act)},
                {"encounter", typeof (POCD_MT000040Encounter)},
                {"observation", typeof (POCD_MT000040Observation)},
                {"observationMedia", typeof (POCD_MT000040ObservationMedia)},
                {"organizer", typeof (POCD_MT000040Organizer)},
                {"procedure", typeof (POCD_MT000040Procedure)},
                {"regionOfInterest", typeof (POCD_MT000040RegionOfInterest)},
                {"substanceAdministration", typeof (POCD_MT000040SubstanceAdministration)},
                {"supply", typeof (POCD_MT000040Supply)}
            };

        public void ReadEntryLevelTemplate()
        {
            const string infoRegexPattern = @"([\w]*):\s*(\w*)\s*(\w.*)";
            var fixedValueRegexPattern = @"([\w|\.]*)\s*\((\w*)\)\s*\=\s*(\w*)";
            var fixedValueTrue = "true";
            var fixedValueFalse = "false";
            var fixedValueRegexNoCodeSystemInfo = @"([\w|\.]*)";
            var infoRegex = new Regex(infoRegexPattern);
            var infoRegexMatch = infoRegex.Matches(EntryLevelTemplate.Info)[0].Groups;
            var typeOfEntry = infoRegexMatch[0].Value;
            var templateIdRoot = infoRegexMatch[1].Value;
            Type = TypeByEntryType[typeOfEntry];





        }

    }
}
