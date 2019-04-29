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
        
        private void SetKeyword(object sender, StartupEventArgs e)
        {
            // Retrieves the last Keyword stored in the datbase
            using (var db = new AdvancedAlphabetEncryptionContext())
            {
                var query = db.Keyword.OrderByDescending(p => p.DaySet).FirstOrDefault();

                // If the query is not a null value and the date is still the same, then the same keyword
                // is used
                if (!(query is null) && query.DaySet.DayOfYear == DateTime.Now.DayOfYear)
                    return;

                Keyword keyword = new Keyword();
                keyword.GenerateRandomKeyword();
                db.Keyword.Add(keyword);

                db.SaveChanges();
            }
        }
    }
}
