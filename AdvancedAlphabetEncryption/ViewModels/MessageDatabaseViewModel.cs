﻿using AdvancedAlphabetEncryption.AlphabetEncryptionDbContext;
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
using System.Windows.Input;

namespace AdvancedAlphabetEncryption.ViewModels
{
    class MessageDatabaseViewModel : BaseViewModel
    {
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

            // Initialises the Messages observable collection with messages from the database
            FilterMessages();
        }

        private int _totalMessages;
        public int TotalMessages
        {
            get => _totalMessages;
            set { _totalMessages = value; OnPropertyChanged(); }
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

        // Holds the collection of messages to be displayed
        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set { _messages = value; OnPropertyChanged(); }
        }


        #region FilteringOptions

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

                FilterMessages();
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

        #endregion


        #region DateFilters

        // If enabled, the DatePickers menu will appear below the filtering options
        public bool _filterByDateEnabled = false;
        public bool FilterByDateEnabled
        {
            get => _filterByDateEnabled;
            set { _filterByDateEnabled = value; OnPropertyChanged(); }
        }

        // If enabled, the DatePicker for EndDate will appear in the UI,
        // allowing the user to search through a date range
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

        // The default date to search, or the starting date if SearchRangeOfDatesEnabled
        // is set to true
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

        // If SearchRangeOfDatesEnabled is enabled, a DatePicker will appear that will allow
        // the user to enter an end date which will be used when searching for messages written
        // within a certain date range
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

        public ICommand RefreshMessagesCommand { get => new RelayCommand(o => FilterMessages()); }

        private void FilterMessages()
        {
            // Forces the message filter to research through the available messages again after a property
            // has been updated

            using (var db = new AdvancedAlphabetEncryptionContext())
            {
                // If there is a value stored in FilterInput, then it can proceed to filter the results
                if (SelectedFilterOption != 0 && !string.IsNullOrWhiteSpace(FilterInput))
                {
                    switch (SelectedFilterOption)
                    {
                        // Keyword filtering
                        case 1:
                            Messages = new ObservableCollection<Message>(db.Messages.Where(m => m.Keyword == FilterInput.ToLower()));
                            break;
                        // Agent initials filtering
                        case 2:
                            Messages = new ObservableCollection<Message>(db.Messages.Where(a => a.CreatedBy == FilterInput.ToUpper()));
                            break;
                        // Keyword and Date filtering
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
                    // If there's no filter applied, simple display all the messages
                    Messages = new ObservableCollection<Message>(db.Messages);
            }
            

            TotalMessages = Messages.Count();
            TotalEncryptedMessages = Messages.Count(m => m is EncryptedMessage);
            TotalDecryptedMessages = Messages.Count(m => m is DecryptedMessage);

            ListMessages();
        }

        

        private void ListMessages()
        {
            // Changes what messages are displayed on screen depending on what message view is selected
            using (var db = new AdvancedAlphabetEncryptionContext())
            {
                switch (SelectedMessageView)
                {
                    case "Encrypted":
                        Messages = new ObservableCollection<Message>(Messages.Where(m => m is EncryptedMessage).OrderByDescending(m => m.CreationDate));
                        break;
                    case "Decrypted":
                        Messages = new ObservableCollection<Message>(Messages.Where(m => m is DecryptedMessage).OrderByDescending(m => m.CreationDate));
                        break;
                    default:
                        Messages = new ObservableCollection<Message>(Messages.OrderByDescending(m => m.CreationDate));
                        break;
                }
            }
        }
    }
}
