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
            Poems = new List<string>() { "Poem 1", "Poem 2", "Poem 3" };
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
        private bool _validKeyword = false;
        public bool ValidKeyword
        {
            get => _validKeyword;
            set { _validKeyword = value; OnPropertyChanged(); }
        }

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

        // Used to fill the poem combobox
        public List<string> Poems { get; set; }
        private string _poemSelection;
        public string PoemSelection
        {
            get => _poemSelection;
            set
            {
                _poemSelection = value.ToString();
                OnPropertyChanged();
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

        public ICommand GenerateKeywordCommand { get => new RelayCommand(o => GenerateKeyword()); }

        public void GenerateKeyword()
        {
            Dictionary<int, string[]> poem = new Dictionary<int, string[]>();
            string[] wordArray;

            switch (PoemSelection)
            {
                case "Poem 1":
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(POEM1);
                    PoemSelection = "1";
                    break;
                case "Poem 2":
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(POEM2);
                    PoemSelection = "2";
                    break;
                case "Poem 3":
                    poem = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(POEM3);
                    PoemSelection = "3";
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
            KeywordCode = string.Format("{0}.{1}.{2}", PoemSelection.PadLeft(2, '0'), LineSelection.ToString().PadLeft(2, '0'), WordSelection.ToString().PadLeft(2, '0'));

            // Accounting for the 0-based array indexing of the words (word 1 will be at index 0)
            WordSelection--;

            wordArray = poem[LineSelection];
            KeywordString = wordArray[WordSelection];
            KeywordSetBy = App.agent.Initials;
            ValidKeyword = true;
            KeywordDaySet = DateTime.Now;

            SetKeyword();
        }

        public void GenerateRandomKeyword()
        {
            Random random = new Random();
            Dictionary<int, string[]> poem = new Dictionary<int, string[]>();
            string[] wordArray;

            int poemSelection = random.Next(1, 3);

            switch (poemSelection)
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

            int lineSelection = random.Next(1, poem.Count);

            wordArray = poem[lineSelection];
            int wordSelection = random.Next(0, wordArray.Length);

            KeywordString = wordArray[wordSelection];
            wordSelection++;
            KeywordCode = string.Format("{0}.{1}.{2}", poemSelection.ToString().PadLeft(2, '0'), lineSelection.ToString().PadLeft(2, '0'), wordSelection.ToString().PadLeft(2, '0'));
            KeywordSetBy = App.agent.Initials;
            ValidKeyword = true;
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
