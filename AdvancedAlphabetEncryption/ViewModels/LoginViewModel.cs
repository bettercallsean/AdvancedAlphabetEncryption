using AdvancedAlphabetEncryption.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private Agent _agent = new Agent();
        public string Email
        {
            get => _agent.Email;

            set
            {
                if(!string.IsNullOrWhiteSpace(value))
                {
                    _agent.Email = value;

                    OnPropertyChanged();
                }
            }
        }
    }
}
