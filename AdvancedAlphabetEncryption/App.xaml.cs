using AdvancedAlphabetEncryption.AlphabetEncryptionDbContext;
using AdvancedAlphabetEncryption.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AdvancedAlphabetEncryption
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Agent agent;
        public static Keyword keyword = new Keyword();
        private void StartupTask(object sender, StartupEventArgs e)
        {
            agent = new Agent("sean", "edwards", "seanedwards97@gmail.com");
            keyword.SetBy = agent.Initials;
            SetKeyword();
        }
        private void SetKeyword()
        {
            // Retrieves the last Keyword stored in the datbase
            using (var db = new AdvancedAlphabetEncryptionContext())
            {
                var query = db.Keyword.OrderByDescending(p => p.DaySet).FirstOrDefault();

                // If the query is not a null value and the date is still the same, then the same keyword
                // is used
                if (!(query is null) && query.DaySet.DayOfYear == DateTime.Now.DayOfYear)
                    return;

                keyword.GenerateRandomKeyword();
                db.Keyword.Add(keyword);

                db.SaveChanges();
            }
        }
    }
}
