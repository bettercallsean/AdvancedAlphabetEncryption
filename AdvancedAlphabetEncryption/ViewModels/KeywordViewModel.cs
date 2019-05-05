using AdvancedAlphabetEncryption.AlphabetEncryptionDbContext;
using AdvancedAlphabetEncryption.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AdvancedAlphabetEncryption.ViewModels
{
    public class KeywordViewModel : CloseableViewModel
    {
        public KeywordViewModel()
        {
            PoemsDictionary = new Dictionary<string, int>()
            {
                {"Poem 1", 1},
                {"Poem 2", 2},
                {"Poem 3", 3}
            };
        }

        // Creates readonly string containing the contents of the POEMx.json file
        readonly string Poem1 = Properties.Resources.POEM1;
        readonly string Poem2 = Properties.Resources.POEM2;
        readonly string Poem3 = Properties.Resources.POEM3;

        public Keyword Keyword
        {
            get => App.keyword;
            set => App.keyword = value;
        }

        #region ErrorValues
        private bool _validPoem = false;
        public bool ValidPoem
        {
            get => _validPoem;
            set { _validPoem = value; OnPropertyChanged(); }
        }

        private bool _validLine = false;
        public bool ValidLine
        {
            get => _validLine;
            set { _validLine = value; OnPropertyChanged(); }
        }

        private bool _validWord = true;
        public bool ValidWord
        {
            get => _validWord;
            set { _validWord = value; OnPropertyChanged(); }
        }
        #endregion

        #region KeywordProperties
        public string KeywordCode { get; private set; }

        public string KeywordString { get; private set; }

        public string KeywordSetBy { get; private set; }

        public DateTime KeywordDaySet { get; private set; }
        #endregion

        #region CustomKeywordProperties
        private bool _generateCustomKeywordChecked = false;
        public bool GenerateCustomKeywordChecked
        {
            get => _generateCustomKeywordChecked;
            set
            {
                _generateCustomKeywordChecked = value;
                OnPropertyChanged();
            }
        }

        // Used to fill the ComboBox with a list of available poems
        private Dictionary<string, int> _poemsDictionary;
        public Dictionary<string, int> PoemsDictionary
        {
            get => _poemsDictionary;
            set { _poemsDictionary = value; OnPropertyChanged(); }
        }

        // Takes the value from the PoemsDictionary (1, 2 or 3) and selects the poem based on that
        public Dictionary<int, string[]> Poem { get; set; }
        private int _poemSelection;
        public int PoemSelection
        {
            get => _poemSelection;
            set
            {
                _poemSelection = value;

                switch (value)
                {
                    case 1:
                        Poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(Poem1);
                        break;
                    case 2:
                        Poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(Poem2);
                        break;
                    case 3:
                        Poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(Poem3);
                        break;
                    default:
                        ValidPoem = false;
                        return;
                }

                ValidPoem = true;
                OnPropertyChanged();
            }
        }

        private int _lineSelection;
        public int LineSelection
        {
            get => _lineSelection;
            set
            {
                // Performs checks to ensure that the numbers that have been passed through don't throw an IndexOutOfRange exception
                // By setting ValidKeyword to false, it will allow a while loop to keep asking for input until these conditions are satisfied
                if (value > Poem.Count || value < 1)
                {
                    ValidLine = false;
                    return;
                }

                _lineSelection = value;
                ValidLine = true;
                OnPropertyChanged();
            }
        }

        private int _wordSelection;
        public int WordSelection
        {
            get => _wordSelection;
            set
            {
                if (value > Poem[LineSelection].Length || value < 1)
                {
                    ValidWord = false;
                    return;
                }

                _wordSelection = --value;
                MessageBox.Show(_wordSelection.ToString());
                ValidWord = true;
                OnPropertyChanged();
            }
        }
        #endregion

        public ICommand GenerateKeywordCommand { get => new RelayCommand(o => GenerateKeyword()); }

        public void GenerateKeyword()
        {
            if (GenerateCustomKeywordChecked)
                GenerateCustomKeyword();
            else
                GenerateRandomKeyword();
        }

        private void GenerateCustomKeyword()
        {
            string[] wordArray;

            // Adds '0' padding to the number. e.g. '8' will turn to '08' but '14' will stay as '14'
            // Will produce a code in the following format - XX.XX.XX
            KeywordCode = string.Format("{0}.{1}.{2}", PoemSelection.ToString().PadLeft(2, '0'), LineSelection.ToString().PadLeft(2, '0'), WordSelection.ToString().PadLeft(2, '0'));

            // Accounting for the 0-based array indexing of the words (word 1 will be at index 0)
            WordSelection--;

            wordArray = Poem[LineSelection];
            KeywordString = wordArray[WordSelection];
            KeywordSetBy = App.agent.Initials;
            KeywordDaySet = DateTime.Now;

            SetKeyword();
        }

        private void GenerateRandomKeyword()
        {
            Random random = new Random();
            Dictionary<int, string[]> poem = new Dictionary<int, string[]>();
            string[] wordArray;

            int PoemSelection = random.Next(1, 3);

            switch (PoemSelection)
            {
                case 1:
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(Poem1);
                    break;
                case 2:
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(Poem2);
                    break;
                case 3:
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(Poem3);
                    break;
            }

            LineSelection = random.Next(1, poem.Count);

            wordArray = poem[LineSelection];
            WordSelection = random.Next(0, wordArray.Length);

            KeywordString = wordArray[WordSelection];

            // The word selection will be using a 0-based indexing system, so each value needs to be increased by 1 to reflect that counting starts at 1
            // in the real world
            WordSelection++;

            // Adds '0' padding to the number. e.g. '8' will turn to '08' but '14' will stay as '14'
            // Will produce a code in the following format - XX.XX.XX
            KeywordCode = string.Format("{0}.{1}.{2}", PoemSelection.ToString().PadLeft(2, '0'), LineSelection.ToString().PadLeft(2, '0'), WordSelection.ToString().PadLeft(2, '0'));
            KeywordSetBy = App.agent.Initials;
            KeywordDaySet = DateTime.Now;

            SetKeyword();
        }

        private void SetKeyword()
        {
            using (var db = new AdvancedAlphabetEncryptionContext())
            {
                Keyword = new Keyword(KeywordString, KeywordSetBy, KeywordCode, KeywordDaySet);

                db.Keyword.Add(Keyword);
                db.SaveChanges();
            }

            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();

            OnClosingRequest();
        }
    }
}
