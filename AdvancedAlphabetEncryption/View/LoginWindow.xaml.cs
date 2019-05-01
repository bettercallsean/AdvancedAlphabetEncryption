using AdvancedAlphabetEncryption.AlphabetEncryptionDbContext;
using AdvancedAlphabetEncryption.Models;
using AdvancedAlphabetEncryption.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdvancedAlphabetEncryption
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public void Login(object sender, RoutedEventArgs e)
        {
            string email = emailBox.Text;

            if (emailBox == null || passwordBox == null)
                MessageBox.Show("Please enter a username and password");

            using (var db = new AdvancedAlphabetEncryptionContext())
            {
                var query = db.Agent.Where(b => b.Email == email).FirstOrDefault();
                if (query.ValidPassword(passwordBox.Password))
                {
                    App.agent = query;
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
                else
                    loginErrorTextBlock.Visibility = Visibility.Visible;
                
            }
        }
    }
}
