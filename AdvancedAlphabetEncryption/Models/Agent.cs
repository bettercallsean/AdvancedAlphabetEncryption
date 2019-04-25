using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.Models
{
    public class Agent
    {
        private string _firstName, _lastName, _email;
        private char[] _initials = new char[2];
        //private int _ID;

        public static int globalAgentID;



        public Agent(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public string FirstName
        {
            get => _firstName;

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _firstName = value;
            }
        }

        public string LastName
        {
            get => _lastName;

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _lastName = value;
            }
        }

        public string Initials
        {
            get
            {
                try
                {
                    _initials[0] = char.ToUpper(_firstName[0]);
                    _initials[1] = char.ToUpper(_lastName[0]);

                    return new string(_initials);
                }
                catch(NullReferenceException)
                {
                    return "";
                }

                
            }
        }

        public string Email
        {
            get => _email;

            private set
            {
                try
                {
                    MailAddress mail = new MailAddress(value);
                    _email = mail.Address;
                }
                catch (FormatException) { Console.WriteLine("Invalid Email Format!"); }
                catch (ArgumentException) { Console.WriteLine("Please check your input and try again."); }

            }
        }

        public void DisplayInformation()
        {
            Console.WriteLine("------------------");
            Console.WriteLine("Agent Name: {0} {1}", FirstName, LastName);
            Console.WriteLine("Initials: {0}", Initials);
            Console.WriteLine("Email: {0}", Email);
        }

    }
}
