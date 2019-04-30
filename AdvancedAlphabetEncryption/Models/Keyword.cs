using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.Models
{   
    public class Keyword
    {
        // Creates readonly string containing the contents of the POEMx.json file
        readonly string POEM1 = Properties.Resources.POEM1;
        readonly string POEM2 = Properties.Resources.POEM2;
        readonly string POEM3 = Properties.Resources.POEM3;

        private string _keyword = "";

        public Keyword ()
        {

        }

        public void GenerateKeyword(int poemSelection, int lineSelection, int wordSelection)
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

            // Performs checks to ensure that the numbers that have been passed through don't throw an IndexOutOfRange exception
            // By setting ValidKeyword to false, it will allow a while loop to keep asking for input until these conditions are satisfied
            if (lineSelection > poem.Count || lineSelection < 1)
            {
                ValidKeyword = false;
                return;
            }

            if(wordSelection > poem[lineSelection].Length || wordSelection < 1)
            {
                ValidKeyword = false;
                return;
            }

            // Adds '0' padding to the number. e.g. '8' will turn to '08' but '14' will stay as '14'
            // Will produce a code in the following format - XX.XX.XX
            KeywordCode = string.Format("{0}.{1}.{2}", poemSelection.ToString().PadLeft(2, '0'), lineSelection.ToString().PadLeft(2, '0'), wordSelection.ToString().PadLeft(2, '0'));

            // Accounting for the 0-based array indexing of the words (word 1 will be at index 0)
            wordSelection--;

            wordArray = poem[lineSelection];
            KeywordString = wordArray[wordSelection];
            ValidKeyword = true;
            DaySet = DateTime.Now;
        }

        public void GenerateRandomKeyword()
        {
            Random random = new Random();
            Dictionary<int, string[]> poem = new Dictionary<int, string[]>();
            string[] wordArray;

            int poemSelection = random.Next(1, 3);

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

            int lineSelection = random.Next(1, poem.Count);

            wordArray = poem[lineSelection];
            int wordSelection = random.Next(0, wordArray.Length);

            KeywordString = wordArray[wordSelection];
            KeywordCode = string.Format("{0}.{1}.{2}", poemSelection.ToString().PadLeft(2, '0'), lineSelection.ToString().PadLeft(2, '0'), wordSelection++.ToString().PadLeft(2, '0'));
            ValidKeyword = true;
            DaySet = DateTime.Now;

        }

        public int Id { get; set; }
        public string KeywordString
        {
            get => _keyword;

            set
            {
                if(!string.IsNullOrWhiteSpace(value))
                {
                    _keyword = value;
                    ValidKeyword = true;
                }
            }
        }

        public string SetBy { get; set; }

        public string KeywordCode { get; private set; }

        // When a Keyword object is created, there will be no valid keyword assigned to it
        public bool ValidKeyword { get; set; } = false;

        public DateTime DaySet { get; private set; }

    }
}
