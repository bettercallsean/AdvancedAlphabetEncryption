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

        private bool _successfulLogin;
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
                var query = db.Agent.Where(a => a.Email == Email).FirstOrDefault();


                if (query != null && query.ValidPassword(passwordBox.Password))
                {
                    SuccessfulLogin = true;
                    App.agent = query;

                    Application.Current.MainWindow = new MainWindow();
                    Application.Current.MainWindow.Show();

                    OnClosingRequest();

                }
                else
                    SuccessfulLogin = false;
                

            }
        }
    }
}
