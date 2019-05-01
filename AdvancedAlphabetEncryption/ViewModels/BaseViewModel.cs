using AdvancedAlphabetEncryption.AlphabetEncryptionDbContext;
using AdvancedAlphabetEncryption.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdvancedAlphabetEncryption.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public string KeywordString
        {
            get => App.keyword.KeywordString;
        }

        private bool _isEncrypted;
        public bool IsEncrypted
        {
            get => _isEncrypted;
            set
            {
                _isEncrypted = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }

        }

        public void SaveToFile(Message message)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = message.CreationDate.ToString("yyyyMMddHHmmss"),
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Save an encrypted file"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog.FileName))
                {
                    file.WriteLine(message.MessageString);
                    file.WriteLine(App.keyword.KeywordString);
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
