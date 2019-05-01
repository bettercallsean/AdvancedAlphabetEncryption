using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {       
        private bool _validEmail, _validPassword;
        public bool ValidEmail
        {
            get => _validEmail;
            set
            {
                _validEmail = value;
                OnPropertyChanged();
            }
        }
        public bool ValidPassword
        {
            get => _validPassword;
            set
            {
                _validPassword = value;
                OnPropertyChanged();
            }
        }
    }
}
