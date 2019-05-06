using AdvancedAlphabetEncryption.AlphabetEncryptionDbContext;
using AdvancedAlphabetEncryption.Models;
using AdvancedAlphabetEncryption.View;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdvancedAlphabetEncryption.ViewModels
{
    public class LoginViewModel : CloseableViewModel
    {
        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private bool _successfulLogin = true;
        public bool SuccessfulLogin
        {
            get => _successfulLogin;
            set { _successfulLogin = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get => new RelayCommand(o => Login(o)); }

        public void Login(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            using (var db = new AdvancedAlphabetEncryptionContext())
            {
                Agent query = db.Agent.Where(a => a.Email == Email).FirstOrDefault();

                if (query != null && query.ValidPassword(passwordBox.Password))
                {
                    App.agent = query;
                    SuccessfulLogin = true;
                    //I know that this isn't MVVM compliant, however it's used only once and people on StackOverflow etc. seem to agree that 
                    // it isn't worth the extra setup for just a simple onetime action.
                    if (KeywordHasBeenSet())
                        Application.Current.MainWindow = new MainWindow();
                    else
                        Application.Current.MainWindow = new KeywordGeneratorWindow();

                    Application.Current.MainWindow.Show();

                    OnClosingRequest();

                }
                
                SuccessfulLogin = false;
                
            }
        }

        private bool KeywordHasBeenSet()
        {
            // Retrieves the last Keyword stored in the datbase
            using (var db = new AdvancedAlphabetEncryptionContext())
            {
                App.keyword = db.Keyword.OrderByDescending(p => p.DaySet).FirstOrDefault();

                // If the query is not a null value and the date is still the same, then the same keyword
                // is used
                if (!(App.keyword is null) && App.keyword.DaySet.Date == DateTime.Now.Date)
                    return true;

                return false;
            }
        }
    }
}
