using AdvancedAlphabetEncryption.Models;
using AdvancedAlphabetEncryption.Models.Messages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdvancedAlphabetEncryption.ViewModels
{
    public class DecryptedMessageViewModel : ViewModelBase
    {
        readonly private DecryptedMessage _decryptedMessage = new DecryptedMessage();

        public DecryptedMessageViewModel()
        {
            _decryptedMessage.Keyword = Models.Keyword.GetKeyword;
        }

        public string MessageString
        {
            get => _decryptedMessage.MessageString;

            set
            {
                // As long as there are characters in the text box, the message will be assigned to the _decryptedMessage object
                if (!string.IsNullOrEmpty(value))
                {
                    _decryptedMessage.MessageString = value;

                    if (!IsEncrypted)
                        IsEncrypted = true;

                    OnPropertyChanged();
                }

            }
        }

        public string Keyword
        {
            get => _decryptedMessage.Keyword;

            set
            {
                // If the textbox doesn't contain solely whitespace, it will continue with the keyword assignment check 
                if (!string.IsNullOrWhiteSpace(value))
                {
                    value = value.Replace(" ", "");
                    // If the entered value only contains letters, then that is used as the keyword
                    if (Regex.IsMatch(value, @"^[a-zA-Z]+$"))
                        _decryptedMessage.Keyword = value;

                    // If there are special characters or numbers, then the keyword is reverted to the default
                    else
                        _decryptedMessage.Keyword = Models.Keyword.GetKeyword;

                }
                // If there is no text in the textbox, the keyword is reverted to the default
                else
                    _decryptedMessage.Keyword = Models.Keyword.GetKeyword;
            }
        }


        public ICommand DecryptCommand { get => new RelayCommand(o => DecryptMessage()); }

        public void DecryptMessage()
        {
            if (SaveToFileChecked)
                SaveToFile(_decryptedMessage);

            // If a valid keyword has been entered (any alphabetical characters), then the program can proceed
            // with the decryption process
            if (IsEncrypted)
            {
                _decryptedMessage.Decrypt();
                IsEncrypted = false;

                OnPropertyChanged("MessageString");
            }
        }
    }
}
