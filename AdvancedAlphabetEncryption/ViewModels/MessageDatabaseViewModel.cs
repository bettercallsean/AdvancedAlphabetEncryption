using AdvancedAlphabetEncryption.AlphabetEncryptionDbContext;
using AdvancedAlphabetEncryption.Models;
using AdvancedAlphabetEncryption.Models.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.ViewModels
{
    class MessageDatabaseViewModel : BaseViewModel
    {
        public MessageDatabaseViewModel()
        {
            ListMessages();
        }

        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get => _messages;

            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        public void ListMessages()
        {

            using (var db = new AdvancedAlphabetEncryptionContext())
            {
                Messages = new ObservableCollection<Message>(db.EncryptedMessages);
            }
        }
    }
}
