using AdvancedAlphabetEncryption.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdvancedAlphabetEncryption.View
{
    /// <summary>
    /// Interaction logic for KeywordGeneratorWindow.xaml
    /// </summary>
    public partial class KeywordGeneratorWindow : Window
    {
        public KeywordGeneratorWindow()
        {
            InitializeComponent();

            KeywordViewModel.ClosingRequest += (sender, e) => Close();
        }

        // Checks to see if the input is an int value
        public void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
