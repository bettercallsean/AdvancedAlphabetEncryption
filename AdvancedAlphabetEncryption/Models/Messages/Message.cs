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

        public Message()
        {

        }

        public Message(Agent agent, string message)
        {
            MessageString = message;
            CreatedBy = agent.Initials;
        }
        

        [Key]
        public int MessageId { get; set; }
        public string MessageString { get; set; } = "";
        public DateTime CreationDate { get; set; }
        public string  CreatedBy { get; set; }
        public string Keyword { get; set; }
    }
}
