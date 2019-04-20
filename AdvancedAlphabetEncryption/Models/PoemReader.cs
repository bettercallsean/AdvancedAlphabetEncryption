using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.Models
{   
    public class PoemReader
    {
        // Creates readonly string containing the contents of the POEMx.json file
        readonly string POEM1 = Properties.Resources.POEM1;
        readonly string POEM2 = Properties.Resources.POEM2;
        readonly string POEM3 = Properties.Resources.POEM3;

        public PoemReader (int poemSelection, int lineSelection, int wordSelection)
        {
            Dictionary<int, string[]> poem = new Dictionary<int, string[]>();
            string[] wordArray;
            
            switch (poemSelection)
            {
                case 1:
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(POEM1);
                    break;
                case 2:
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(POEM2);
                    break;
                case 3:
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(POEM3);
                    break;
            }

            try
            {
                KeywordCode = string.Format("{0}.{1}.{2}", poemSelection, lineSelection, wordSelection); 

                // Accounting for the 0-based array indexing of the words
                wordSelection--;

                wordArray = poem[lineSelection];
                Keyword = wordArray[wordSelection];
                ValidKeyword = true;
                Console.WriteLine(Keyword);
            }
            catch(IndexOutOfRangeException)
            {
                Console.WriteLine("Index out of bounds!");
                ValidKeyword = false;
            }
        }

        public string Keyword { get; private set; }

        public string KeywordCode { get; private set; }

        private bool ValidKeyword { get; set; }

    }
}
