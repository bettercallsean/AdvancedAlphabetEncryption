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
        private EncryptedMessage _encryptedMessage = new EncryptedMessage();
        
        public string MessageString
        {
            get => _encryptedMessage.MessageString;

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _encryptedMessage.MessageString = value;
                    _encryptedMessage.Keyword = Keyword.GetKeyword;

                    OnPropertyChanged();
                }
                    
            }
        }

        public ICommand EncryptCommand
        {
            get => new RelayCommand(o => EncryptMessage());
        }

        public void EncryptMessage()
        {
            _encryptedMessage.Encrypt();

            if(SaveToFileChecked)
                SaveToFile();
        }

        public void SaveToFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = _encryptedMessage.EncryptionDate.ToString("yyyyMMddHHmmss"),
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Save an encrypted file"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog.FileName))
                {
                    file.WriteLine(MessageString);
                    file.WriteLine(Keyword.GetKeyword);
                }
            }
            
        }

        
    }

    

}
