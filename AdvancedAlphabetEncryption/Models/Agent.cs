using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedAlphabetEncryption.Models
{
    public class Agent
    {
        private string _firstName, _lastName, _email;
        readonly private char[] _initials = new char[2];


        public Agent(string firstName, string lastName, string email, string password, byte[] image = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            ProfilePicture = image;
        }

        public Agent()
        {

        }

        [Key]
        public int AgentID { get; set; }

        public string FirstName
        {
            get => _firstName;

            set => _firstName = value;
            
        }

        public string LastName
        {
            get => _lastName;

            set => _lastName = value;
            
        }

        public string Initials
        {
            get
            {
                    _initials[0] = char.ToUpper(_firstName[0]);
                    _initials[1] = char.ToUpper(_lastName[0]);

                    return new string(_initials);
            }
        }

        public string Email
        {
            get => _email;

            set
            {
                try
                {
                    MailAddress mail = new MailAddress(value);
                    _email = mail.Address;
                }
                catch (FormatException) { throw new FormatException("Invalid Email Format!"); }
                catch (ArgumentException) { throw new ArgumentException("Please check your input and try again."); }
            }
        }

        // Stores the picture as a byte array to make it easier to store in a database
        public byte[] ProfilePicture { get; set; }

        public virtual string PasswordStored
        {
            get;
            set;
        }

        [NotMapped]
        public string Password
        {
            set => PasswordStored = Hashing.HashPassword(value); 
        }

        public bool ValidPassword(string password)
        {
            return Hashing.ValidatePassword(password, PasswordStored);
        }

    }

    
}
