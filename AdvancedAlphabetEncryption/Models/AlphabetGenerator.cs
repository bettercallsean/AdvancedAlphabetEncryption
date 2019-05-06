using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.Models
{
    public class AlphabetGenerator
    {
        public Dictionary<int, char> intToCharDictionary;
        public Dictionary<char, int> charToIntDictionary;

        public AlphabetGenerator(string keyword)
        {
            char[] keywordChars = keyword.ToCharArray();
            Matrix = new char[26, keyword.Length];

            AlphabetArrayGenerator(keywordChars);

        }

        private void AlphabetArrayGenerator(char[] keyword)
        {
            intToCharDictionary = JsonConvert.DeserializeObject<Dictionary<int, char>>(Properties.Resources.IntToCharacter);
            charToIntDictionary = JsonConvert.DeserializeObject<Dictionary<char, int>>(Properties.Resources.CharacterToInt);

            // loops through every character in the keyword, this will be the starting letter of each alphabet array
            for (int i = 0; i < keyword.Length; i++)
            {
                // Converts the character to upper case because that is how they are stored in he .json files
                char letter = char.ToUpper(keyword[i]);
                int letterPosition = charToIntDictionary[letter];

                for(int j = 0; j < 26; j++, letterPosition++)
                {
                    // When the dictionary reaches the end of its alphabet values, it will start at the beginning again
                    // and insert the rest of the alphabet characters
                    if (letterPosition == 26)
                        letterPosition = 0;

                    Matrix[j, i] = intToCharDictionary[letterPosition];
                }
            }
        }

        public char[,] Matrix { get; private set; }

        // Used during debugging
        public void AlphabetPrinter()
        {
            for(int j = 0; j < Matrix.GetLength(1); j++)
            {
                for (int i = 0; i < Matrix.GetLength(0); i++)
                    Console.Write(Matrix[i, j]);

                Console.WriteLine();
            }
        }

        // Used to create the CharacterToInt and IntToCharacter, not used elsewhere unless called
        private void AlphabetSerializer()
        {
            intToCharDictionary = new Dictionary<int, char>();
            charToIntDictionary = new Dictionary<char, int>();

            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            for (int i = 0; i < 26; i++)
            {
                char letter = alpha[i];
                charToIntDictionary[letter] = i;
                intToCharDictionary[i] = letter;
            }

            string intToCharFilename = @"W:\Applied Programming\Assignment\AdvancedAlphabetEncryption\AdvancedAlphabetEncryption\Resources\CharacterToInt.json";
            string charToIntFilename = @"W:\Applied Programming\Assignment\AdvancedAlphabetEncryption\AdvancedAlphabetEncryption\Resources\IntToCharacter.json";

            using (StreamWriter file = File.CreateText(intToCharFilename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, charToIntDictionary);
            }

            using (StreamWriter file = File.CreateText(charToIntFilename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, intToCharDictionary);
            }
        }

    }
}
