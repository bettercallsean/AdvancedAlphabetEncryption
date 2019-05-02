using AdvancedAlphabetEncryption.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.ViewModels
{
    public class MessageViewModel : BaseViewModel
    {
        private bool _isEncrypted;
        public bool IsEncrypted
        {
            get => _isEncrypted;
            set { _isEncrypted = value; OnPropertyChanged(); }
        }

        // Used to determine whether the SaveFileDialog box appears
        private bool _saveToFileChecked = false;
        public bool SaveToFileChecked
        {
            get => _saveToFileChecked;
            set { _saveToFileChecked = value; OnPropertyChanged(); }
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
    }
}
