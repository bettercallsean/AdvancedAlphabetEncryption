using AdvancedAlphabetEncryption.AlphabetEncryptionDbContext;
using System;
using System.Collections.Generic;
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

        private bool _failedLogin = false;
        public bool FailedLogin
        {
            get => _failedLogin;
            set { _failedLogin = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get => new RelayCommand(o => Login(o)); }

        public void Login(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            using (var db = new AdvancedAlphabetEncryptionContext())
            {
                var query = db.Agent.Where(a => a.Email == Email).FirstOrDefault();


                if (query != null && query.ValidPassword(passwordBox.Password))
                {
                    FailedLogin = false;
                    App.agent = query;

                    //I know that this isn't MVVM compliant, however it's used only once and people on StackOverflow etc. seem to agree that 
                    // it isn't worth the extra setup for just a simple onetime action.
                    Application.Current.MainWindow = new MainWindow();
                    Application.Current.MainWindow.Show();

                    OnClosingRequest();

                }
                
                FailedLogin = true;
                

            }
        }
    }
}
