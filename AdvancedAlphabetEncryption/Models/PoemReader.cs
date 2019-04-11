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
        string POEM1 = @"W:\Applied Programming\Assignment\AdvancedAlphabetEncryption\AdvancedAlphabetEncryption\Poems\POEM1.txt";
        string POEM2 = @"W:\Applied Programming\Assignment\AdvancedAlphabetEncryption\AdvancedAlphabetEncryption\Poems\POEM2.txt";
        string POEM3 = @"W:\Applied Programming\Assignment\AdvancedAlphabetEncryption\AdvancedAlphabetEncryption\Poems\POEM3.txt";

        public PoemReader (int poemSelection, int lineSelection, int wordSelection)
        {
            lineSelection--;
            wordSelection--;

            string[] lines = new string[0];
            string[] words;
            
            switch (poemSelection)
            {
                case 1:
                    lines = System.IO.File.ReadAllLines(POEM1);
                    break;
                case 2:
                    lines = System.IO.File.ReadAllLines(POEM2);
                    break;
                case 3:
                    lines = System.IO.File.ReadAllLines(POEM3);
                    break;
            }

            words = lines[lineSelection].Split();
            Console.WriteLine(words[wordSelection]);

            


        }
    }
}
