using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.ViewModels
{
    // Solution taken from https://stackoverflow.com/a/11948550/7416597
    public abstract class CloseableViewModel : BaseViewModel
    {
        public event EventHandler ClosingRequest;

        protected void OnClosingRequest()
        {
            ClosingRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
