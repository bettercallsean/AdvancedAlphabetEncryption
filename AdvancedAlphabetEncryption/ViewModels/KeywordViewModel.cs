using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.ViewModels
{
    public class KeywordViewModel
    {
        public string Keyword { get => App.keyword.KeywordString; }
        public string KeywordDate { get => App.keyword.DaySet.ToString("dddd dd MMM"); }
    }
}
