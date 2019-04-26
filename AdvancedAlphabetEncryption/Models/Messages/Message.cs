using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using AdvancedAlphabetEncryption.Models.Messages;

namespace AdvancedAlphabetEncryption.Models
{
    public class Message
    {
        protected readonly Dictionary<char, int> charToIntDictionary = JsonConvert.DeserializeObject<Dictionary<char, int>>(Properties.Resources.CharacterToInt);
        protected readonly Dictionary<int, char> intToCharDictionary = JsonConvert.DeserializeObject<Dictionary<int, char>>(Properties.Resources.IntToCharacter);

        public Message(string message = "")
        {
            MessageString = message;
        }

        public void SaveToFile(string filepath = "")
        {
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss");

            if (string.IsNullOrWhiteSpace(filepath))
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Google Drive\Applied Programming\Assignment\Messages\" + filename + ".txt"))
                {
                    file.WriteLine(MessageString);
                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath + filename + ".txt"))
                {
                    file.WriteLine(MessageString);
                }
            }
        }

        [Key]
        public int MessageId { get; set; }

        public string MessageString { get; protected set; }

    }

    public class MessagesContext : DbContext
    {
        public DbSet<DecryptedMessage> DecryptedMessages { get; set; }
        public DbSet<EncryptedMessage> EncryptedMessages { get; set; }
        
    }

}
