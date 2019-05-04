using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.Models
{   
    public class Keyword
    {
        public Keyword()
        {

        }

        public Keyword(string keywordString, string agentInitials, string code, DateTime daySet)
        {
            KeywordString = keywordString;
            SetBy = agentInitials;
            Code = code;
            DaySet = daySet;
        }


        public int Id { get; set; }
        public string KeywordString { get; set; } = "";

        public string SetBy { get; set; }

        public string Code { get; private set; }

        public DateTime DaySet { get; private set; }

    }
}
