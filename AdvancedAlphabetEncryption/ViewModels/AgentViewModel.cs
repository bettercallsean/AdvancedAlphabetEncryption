using AdvancedAlphabetEncryption.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AdvancedAlphabetEncryption.ViewModels
{
    public class AgentViewModel : BaseViewModel
    {       
        public string FullName { get => FirstName + " " + LastName; }

        public string FirstName { get => App.agent.FirstName; }

        public string LastName { get => App.agent.LastName; }

        public string Email { get => App.agent.Email; }

        public BitmapImage ProfilePicture { get => Utilities.ImageConverter.LoadImage(App.agent.ProfilePicture); }

    }
}
