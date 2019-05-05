using AdvancedAlphabetEncryption.AlphabetEncryptionDbContext;
using AdvancedAlphabetEncryption.Models;
using AdvancedAlphabetEncryption.Models.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
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

            FilterOptions = new Dictionary<string, int>
            {
                {"No Filter", 0},
                {"Filter by Keyword", 1 },
                {"Filter by Agent", 2 },
                {"Filter by Keyword and Date", 3}
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
        private Dictionary<string, int> _filterOptions;
        public Dictionary<string, int> FilterOptions
        {
            get => _filterOptions;
            set { _filterOptions = value; OnPropertyChanged(); }
        }

        // Used to determine what filtering option is selected
        private int _selectedFilterOption;
        public int SelectedFilterOption
        {
            get { return _selectedFilterOption; }
            set
            {
                _selectedFilterOption = value;

                // If this option is selected, the DatePickers will appear to allow
                // the user to enter their search dates
                if (value == 3)
                    FilterByDateEnabled = true;
                else
                    FilterByDateEnabled = false;
                OnPropertyChanged();
                FilterMessages();
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

        // Holds the collection of messages to be displayed
        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set { _messages = value; OnPropertyChanged(); }
        }

        #region DateFilters
        public bool _filterByDateEnabled = false;
        public bool FilterByDateEnabled
        {
            get => _filterByDateEnabled;
            set { _filterByDateEnabled = value; OnPropertyChanged(); }
        }

        private bool _searchRangeOfDatesEnabled = false;
        public bool SearchRangeOfDatesEnabled
        {
            get => _searchRangeOfDatesEnabled; 
            set
            {
                _searchRangeOfDatesEnabled = value;
                OnPropertyChanged();
                FilterMessages();
            }
        }

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
                FilterMessages();
            }
        }

        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
                FilterMessages();
            }
        }
        #endregion

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
            if (SelectedFilterOption != 0)// && !string.IsNullOrWhiteSpace(FilterInput))
            {
                switch (SelectedFilterOption)
                {
                    case 1:
                        Messages = new ObservableCollection<Message>(db.Messages.Where(m => m.Keyword == FilterInput.ToLower()));
                        break;
                    case 2:
                        Messages = new ObservableCollection<Message>(db.Messages.Where(a => a.CreatedBy == FilterInput.ToUpper()));
                        break;
                    case 3:
                        if (StartDate != null)
                        {
                            // If the user wants to seaarh through a range of dates
                            // and EndDate is set to a later date than the start date, search can commence
                            if (SearchRangeOfDatesEnabled && EndDate > StartDate)
                                Messages = new ObservableCollection<Message>(db.Messages.Where(d => DbFunctions.TruncateTime(d.CreationDate) >= StartDate 
                                && DbFunctions.TruncateTime(d.CreationDate) <= EndDate 
                                && d.Keyword == FilterInput));

                            // Else if EndDate is less than StartDate or if it simply hasn't been set
                            else
                                Messages = new ObservableCollection<Message>(db.Messages.Where(d => DbFunctions.TruncateTime(d.CreationDate) == StartDate 
                                && d.Keyword == FilterInput));
                        }
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
