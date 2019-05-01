using AdvancedAlphabetEncryption.AlphabetEncryptionDbContext;
using AdvancedAlphabetEncryption.Models;
using AdvancedAlphabetEncryption.ViewModels;
using AdvancedAlphabetEncryption.View;
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
        public static Agent agent = new Agent();
        public static Keyword keyword = new Keyword();
        

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SetKeyword();
        }

        private void SetKeyword()
        {
            // Retrieves the last Keyword stored in the datbase
            using (var db = new AdvancedAlphabetEncryptionContext())
            {
                keyword = db.Keyword.OrderByDescending(p => p.DaySet).FirstOrDefault();

                // If the query is not a null value and the date is still the same, then the same keyword
                // is used
                if (!(keyword is null) && keyword.DaySet.DayOfYear == DateTime.Now.DayOfYear)
                    return;

                keyword.GenerateRandomKeyword();
                keyword.SetBy = agent.Initials;
                db.Keyword.Add(keyword);

                db.SaveChanges();
            }
        }
    }
}
