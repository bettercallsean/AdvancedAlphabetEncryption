using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AdvancedAlphabetEncryption.Models;
using AdvancedAlphabetEncryption.Models.Messages;
using Microsoft.Win32;

namespace AdvancedAlphabetEncryption.ViewModels
{
    public class EncryptedMessageViewModel : ViewModelBase
    {
        readonly private EncryptedMessage _encryptedMessage = new EncryptedMessage();
        
        public EncryptedMessageViewModel()
        {
            _encryptedMessage.Keyword = Keyword.GetKeyword;
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
            }

            if (SaveToFileChecked)
                SaveToFile(_encryptedMessage);
        }
        
    }

    

}
