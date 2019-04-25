using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.Models.Messages
{
    public class EncryptedMessage : Message
    {
        public EncryptedMessage(Agent agent, string message, string keyword) : base(message)
        {
            Keyword = keyword;
            Encrypt();
            EncryptedBy = agent.Initials;
        }
        public EncryptedMessage()
        {

        }
        private void Encrypt()
        {
            AlphabetGenerator alphabet = new AlphabetGenerator(Keyword);
            char[] unencryptedMessage = MessageText.ToCharArray();
            char[] encryptedMessage = new char[MessageText.Length];
            int alphabetMatrixLineNumber = 0;

            for (int i = 0; i < unencryptedMessage.Length; i++)
            {
                // All the letters in the charToIntDictionary are stored in upper case, so it's important that each letter is converted to an uppercase value
                // or else the Dictionary search will fail
                char letter = char.ToUpper(unencryptedMessage[i]);

                if (!char.IsLetter(letter))
                {
                    encryptedMessage[i] = letter;
                    continue;
                }

                // If the loop has iterated through every row in the alphabetMatrix, it will be reset to start on the first row again
                if (alphabetMatrixLineNumber == alphabet.Matrix.GetLength(1))
                    alphabetMatrixLineNumber = 0;

                // Finds the letter's position in the alphabet and assigns the value that is in it's position in the alphabet matrix 
                int letterPosition = charToIntDictionary[letter];
                encryptedMessage[i] = alphabet.Matrix[letterPosition, alphabetMatrixLineNumber];

                alphabetMatrixLineNumber++;
            }

            MessageText = new string(encryptedMessage);
            EncryptionDate = DateTime.Now;
        }

        public string Keyword { get; private set; }
        public DateTime EncryptionDate { get; private set; }
        public string EncryptedBy { get; private set; }
    }

    public class EncryptedMessagesContext : DbContext
    {
        public DbSet<EncryptedMessage> Messages { get; set; }
    }
}
