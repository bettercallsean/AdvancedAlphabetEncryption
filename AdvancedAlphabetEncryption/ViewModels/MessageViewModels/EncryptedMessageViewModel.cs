using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AdvancedAlphabetEncryption.AlphabetEncryptionDbContext;
using AdvancedAlphabetEncryption.Models;
using AdvancedAlphabetEncryption.Models.Messages;
using Microsoft.Win32;

namespace AdvancedAlphabetEncryption.ViewModels
{
    public class EncryptedMessageViewModel : MessageViewModel
    {
        readonly private EncryptedMessage _encryptedMessage = new EncryptedMessage();

        public EncryptedMessageViewModel()
        {
            _encryptedMessage.Keyword = App.keyword.KeywordString;
            _encryptedMessage.CreatedBy = App.agent.Initials;
        }

        public string MessageString
        {
            get => _encryptedMessage.MessageString;

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _encryptedMessage.MessageString = value;

                    if (IsEncrypted)
                        IsEncrypted = false;
                }

                OnPropertyChanged();
            }
        }

        public ICommand EncryptCommand { get => new RelayCommand(o => EncryptMessage()); }

        public void EncryptMessage()
        {
            if (!IsEncrypted)
            {
                _encryptedMessage.Encrypt();
                IsEncrypted = true;

                OnPropertyChanged("MessageString");

                SaveToDatabase();
            }

            if (SaveToFileChecked)
                SaveToFile(_encryptedMessage);

        }

        private void SaveToDatabase()
        {
            using (var db = new AdvancedAlphabetEncryptionContext())
            {
                db.EncryptedMessages.Add(_encryptedMessage);
                db.SaveChanges();
            }
        }

    }



}