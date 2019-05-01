using AdvancedAlphabetEncryption.Models;
using AdvancedAlphabetEncryption.Models.Messages;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.AlphabetEncryptionDbContext
{
    public class AdvancedAlphabetEncryptionContext : DbContext
    {
        public DbSet<DecryptedMessage> DecryptedMessages { get; set; }
        public DbSet<EncryptedMessage> EncryptedMessages { get; set; }
        public DbSet<Keyword> Keyword { get; set; }
        public DbSet<Agent> Agent { get; set; }
        
    }
}
