using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace AdvancedAlphabetEncryption.Models
{
    public class Message
    {
        protected readonly Dictionary<char, int> charToIntDictionary = JsonConvert.DeserializeObject<Dictionary<char, int>>(Properties.Resources.CharacterToInt);
        protected readonly Dictionary<int, char> intToCharDictionary = JsonConvert.DeserializeObject<Dictionary<int, char>>(Properties.Resources.IntToCharacter);

        public Message(string message = "")
        {
            MessageText = message;
        }

        public void SaveToFile(string filepath = "")
        {
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss");

            if (string.IsNullOrWhiteSpace(filepath))
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Google Drive\Applied Programming\Assignment\Messages\" + filename + ".txt"))
                {
                    file.WriteLine(MessageText);
                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath + filename + ".txt"))
                {
                    file.WriteLine(MessageText);
                }
            }
        }

        [Key]
        public int MessageId { get; set; }

        public string MessageText { get; protected set; }

    }

}
