using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.Models.Messages
{
    public class DecryptedMessage : Message
    {

        public DecryptedMessage(Agent agent, string message, string keyword) : base(message)
        {
            Keyword = keyword;
            Decrypt();
            DecryptedBy = agent.Initials;
        }
        
        private DecryptedMessage()
        {

        }
        public void Decrypt()
        {
            char[] keywordChars = Keyword.ToCharArray();
            char[] encryptedMessage = MessageText.ToCharArray();
            char[] unencryptedMessage = new char[MessageText.Length];
            int alphabetMatrixLineNumber = 0;

            for (int i = 0; i < encryptedMessage.Length; i++)
            {

                // All the letters in the charToIntDictionary are stored in upper case, so it's important that each letter is converted to an uppercase value
                // or else the Dictionary search will fail
                char letter = char.ToUpper(encryptedMessage[i]);

                if (!char.IsLetter(letter))
                {
                    unencryptedMessage[i] = letter;
                    continue;
                }

                // If the loop has iterated through every row in the alphabetMatrix, it will be reset to start on the first row again
                if (alphabetMatrixLineNumber == keywordChars.Length)
                    alphabetMatrixLineNumber = 0;

                // Gets the position of the first letter of the current row of the alphabetMatrix
                // This position can be used to calculate how offset each character is in regards to their original position in the alphabet
                // For example:'W' is the 23rd letter in the alphabet, which means that the current letter in the encrypted message is 4 positions out of place.
                // We can then find the current letter's original position in the alphabet and add 4 to find the original letter (which is 'H').
                int charOffset = 26 - charToIntDictionary[char.ToUpper(keywordChars[alphabetMatrixLineNumber])];

                // If the letter's position hasn't changed, append it
                if (charOffset == 26)
                    unencryptedMessage[i] = letter;
                else
                {
                    // Gets the position of the current character and finds it's position in the 'normal' alphabet.
                    // It's original letter is then calculated by adding the offset to the letter's current position
                    int originalLetterlPositionInAlphabet = charToIntDictionary[letter];
                    int characterPosition = originalLetterlPositionInAlphabet + charOffset;

                    // If the addition goes over 25, then the alphabet has looped back round to the beginning, so subtracting 26 from the total
                    // will give us a value towards the beginning of the alphabet
                    if (originalLetterlPositionInAlphabet + charOffset > 25)
                        characterPosition -= 26;

                    unencryptedMessage[i] = intToCharDictionary[characterPosition];
                }

                alphabetMatrixLineNumber++;
            }

            MessageText = new string(unencryptedMessage);
            DecryptionDate = DateTime.Now;
            
        }

        public string Keyword { get; set; }
        public DateTime DecryptionDate { get; private set; }
        public string DecryptedBy { get; private set; }
    }

    public class DecryptedMessagesContext : DbContext
    {
        public DbSet<DecryptedMessage> Messages { get; set; }
    }
}
