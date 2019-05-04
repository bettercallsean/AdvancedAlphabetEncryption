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
            Poems = new Dictionary<string, int>()
            {
                {"Poem 1", 1},
                {"Poem 2", 2},
                {"Poem 3", 3}
            };
        }

        // Creates readonly string containing the contents of the POEMx.json file
        readonly string POEM1 = Properties.Resources.POEM1;
        readonly string POEM2 = Properties.Resources.POEM2;
        readonly string POEM3 = Properties.Resources.POEM3;

        public Keyword Keyword
        {
            get => App.keyword;
            set => App.keyword = value;
        }

        #region ErrorValues
        private bool _validLine = true;
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
            set { _generateCustomKeywordChecked = value; OnPropertyChanged(); }
        }

        private Dictionary<string, int> _poems;
        public Dictionary<string, int> Poems
        {
            get => _poems;
            set { _poems = value; OnPropertyChanged(); }
        }

        private int _poemSelection;
        public int PoemSelection
        {
            get => _poemSelection;
            set
            {
                _poemSelection = value;
                OnPropertyChanged("Poems");
            }
        }

        private int _lineSelection;
        public int LineSelection
        {
            get => _lineSelection;
            set
            {
                
                _lineSelection = value;
                OnPropertyChanged();
            }
        }

        private int _wordSelection;
        public int WordSelection
        {
            get => _wordSelection;
            set
            {
                _wordSelection = value;
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
            Dictionary<int, string[]> poem = new Dictionary<int, string[]>();
            string[] wordArray;

            switch (PoemSelection)
            {
                case 1:
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(POEM1);
                    break;
                case 2:
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(POEM2);
                    break;
                case 3:
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(POEM3);
                    break;
            }

            // Performs checks to ensure that the numbers that have been passed through don't throw an IndexOutOfRange exception
            // By setting ValidKeyword to false, it will allow a while loop to keep asking for input until these conditions are satisfied
            if (LineSelection > poem.Count || LineSelection < 1)
            {
                ValidLine = false;
                return;
            }

            ValidLine = true;

            if (WordSelection > poem[LineSelection].Length || WordSelection < 1)
            {
                ValidWord = false;
                return;
            }

            ValidWord = true;

            // Adds '0' padding to the number. e.g. '8' will turn to '08' but '14' will stay as '14'
            // Will produce a code in the following format - XX.XX.XX
            KeywordCode = string.Format("{0}.{1}.{2}", PoemSelection.ToString().PadLeft(2, '0'), LineSelection.ToString().PadLeft(2, '0'), WordSelection.ToString().PadLeft(2, '0'));

            // Accounting for the 0-based array indexing of the words (word 1 will be at index 0)
            WordSelection--;

            wordArray = poem[LineSelection];
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
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(POEM1);
                    break;
                case 2:
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(POEM2);
                    break;
                case 3:
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(POEM3);
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
