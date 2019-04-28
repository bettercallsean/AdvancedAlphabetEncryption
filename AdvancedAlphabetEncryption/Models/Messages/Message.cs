using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using AdvancedAlphabetEncryption.Models.Messages;
using Microsoft.Win32;

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

        

        [Key]
        public int MessageId { get; set; }

        public string MessageString { get; set; }

    }

    public class MessagesContext : DbContext
    {
        public DbSet<DecryptedMessage> DecryptedMessages { get; set; }
        public DbSet<EncryptedMessage> EncryptedMessages { get; set; }
        
    }

}
