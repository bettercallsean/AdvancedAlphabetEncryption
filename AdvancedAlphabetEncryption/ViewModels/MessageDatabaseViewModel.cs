using AdvancedAlphabetEncryption.AlphabetEncryptionDbContext;
using AdvancedAlphabetEncryption.Models;
using AdvancedAlphabetEncryption.Models.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AdvancedAlphabetEncryption.ViewModels
{
    class MessageDatabaseViewModel : BaseViewModel
    {
        AdvancedAlphabetEncryptionContext db = new AdvancedAlphabetEncryptionContext();
        public MessageDatabaseViewModel()
        {
            MessageViewList = new List<string>
            {
                "All Messages",
                "Encrypted",
                "Decrypted"
            };

            FilterOptions = new List<string>
            {
                "No Filter",
                "Filter by Keyword",
                "Filter by Agent"
            };
        }

        private int _totalEncryptedMessages;
        public int TotalEncryptedMessages
        {
            get => _totalEncryptedMessages;
            set { _totalEncryptedMessages = value; OnPropertyChanged(); }
        }

        private int _totalDecryptedMessages;
        public int TotalDecryptedMessages
        {
            get => _totalDecryptedMessages; 
            set { _totalDecryptedMessages = value; OnPropertyChanged(); }
        }


        // Used to allow the user to switch between encrypted and decrypted message views
        private List<string> _messageViewList;
        public List<string> MessageViewList
        {
            get => _messageViewList;
            set { _messageViewList = value; OnPropertyChanged(); }
        }

        // Lists the available filters for the messages
        private List<string> _filterOptions;
        public List<string> FilterOptions
        {
            get => _filterOptions;
            set { _filterOptions = value; OnPropertyChanged(); }
        }

        // Used to determine what filtering option is selected
        private string _selectedFilterOption;
        public string SelectedFilterOption
        {
            get { return _selectedFilterOption; }
            set
            {
                _selectedFilterOption = value;
                OnPropertyChanged();
                FilterMessages();

                // Clears the textbox ready for the new filter
                //FilterInput = "";
            }
        }

        // Sets the message view based on whether they're encrypted or decrypted
        private string _selectedMessageView;
        public string SelectedMessageView
        {
            get => _selectedMessageView;
            set
            {
                _selectedMessageView = value;
                OnPropertyChanged();
                ListMessages();
            }

        }

        // Stores the selected filter option
        private ICollectionView _messagesView;
        public ICollectionView MessagesView
        {
            get => _messagesView; 
            set { _messagesView = value; OnPropertyChanged(); }
        }

        // When they user enters text into the filter textbox, the stored value is used when it comes to filter
        // through the messages
        private string _filterInput;
        public string FilterInput
        {
            get => _filterInput; 
            set
            {
                _filterInput = value;
                OnPropertyChanged();
                FilterMessages();
            }
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
                switch (SelectedMessageView)
                {
                    case "Encrypted":
                        Messages = new ObservableCollection<Message>(db.EncryptedMessages.OrderByDescending(m => m.CreationDate));
                        break;
                    case "Decrypted":
                        Messages = new ObservableCollection<Message>(db.DecryptedMessages.OrderByDescending(m => m.CreationDate));
                        break;
                    default:
                        Messages = new ObservableCollection<Message>(db.Messages.OrderByDescending(m => m.CreationDate));
                        break;
                }
            }
        }

        private void FilterMessages()
        {
            // If there is a value stored in FilterInput, then it can proceed to filter the results
            if (SelectedFilterOption != "No Filter" && !string.IsNullOrWhiteSpace(FilterInput))
            {
                switch (SelectedFilterOption)
                {
                    case "Filter by Keyword":
                        Messages = new ObservableCollection<Message>(db.Messages.Where(m => m.Keyword == FilterInput.ToLower()));
                        break;
                    case "Filter by Agent":
                        Messages = new ObservableCollection<Message>(db.Messages.Where(a => a.CreatedBy == FilterInput.ToUpper()));
                        break;
                }
            }
            else
            {
                Messages = new ObservableCollection<Message>(db.Messages);
            }

            TotalEncryptedMessages = Messages.Count(m => m is EncryptedMessage);
            TotalDecryptedMessages = Messages.Count(m => m is DecryptedMessage);
        }
    }
}
