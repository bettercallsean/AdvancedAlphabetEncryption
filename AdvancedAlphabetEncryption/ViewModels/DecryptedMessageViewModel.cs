using AdvancedAlphabetEncryption.Models;
using AdvancedAlphabetEncryption.Models.Messages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdvancedAlphabetEncryption.ViewModels
{
    public class DecryptedMessageViewModel : ViewModelBase
    {
        readonly private DecryptedMessage _decryptedMessage = new DecryptedMessage();

        public string MessageString
        {
            get => _decryptedMessage.MessageString;

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _decryptedMessage.MessageString = value;

                    OnPropertyChanged();
                }

            }
        }

        public string Keyword
        {
            get => _decryptedMessage.Keyword;

            set
            {
                value = value.Replace(" ", "");
                if(!string.IsNullOrEmpty(value))
                {
                    _decryptedMessage.Keyword = value;
                    ValidKeyword = true;

                    OnPropertyChanged();
                }
                else
                {
                    ValidKeyword = false;
                }
            }
        }

        private bool _validKeyword;
        public bool ValidKeyword
        {
            get => _validKeyword;

            private set
            {
                _validKeyword = value;
                OnPropertyChanged();
            }
        }

        public ICommand DecryptCommand
        {
            get => new RelayCommand(o => DecryptMessage());

        }

        public void DecryptMessage()
        {
            if (ValidKeyword)
            {
                _decryptedMessage.Decrypt();

                if (SaveToFileChecked)
                    SaveToFile(_decryptedMessage);
            }
        }
    }
}
