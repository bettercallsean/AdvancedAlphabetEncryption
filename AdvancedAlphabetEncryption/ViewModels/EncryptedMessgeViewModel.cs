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
    public class EncryptedMessgeViewModel : INotifyPropertyChanged
    {
        private EncryptedMessage _encryptedMessage = new EncryptedMessage();
        
        public string MessageString
        {
            get => _encryptedMessage.MessageString;

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _encryptedMessage.MessageString = value;
                    _encryptedMessage.Keyword = Keyword.GetKeyword;

                    OnPropertyChanged("MessageString");
                }
                    
            }
        }

        // Used to determine whether the SaveFileDialog box appears
        private bool _saveToFileChecked = false;
        public bool SaveToFileChecked
        {
            get => _saveToFileChecked;
            set
            {
                _saveToFileChecked = value;
                OnPropertyChanged("SaveToFileChecked");
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter ?? "<N/A>");
        }

    }

}
