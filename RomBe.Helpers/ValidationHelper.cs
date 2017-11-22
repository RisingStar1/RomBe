using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RomBe.Helpers
{
    public class ValidationHelper
    {
        bool invalid = false;

        public bool IsValidEmail(string email)
        {
            invalid = false;
            if (String.IsNullOrEmpty(email))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(email,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public bool IsValidPassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[0-9])(?=.*[a-zA-Z])\w{8,20}$");
            return regex.IsMatch(password);
        }

        //TODO: CHANGE THE NAME OF THE FUNCTION
        public string GetLatestValue(string oldString, string newString)
        {
            if (!newString.IsNull())
            {
                return newString;
            }
            else
            {
                return oldString;
            }

        }

        public int GetLatestValue(int oldInt, int newInt)
        {
            if (newInt!=0)
            {
                return newInt;
            }
            else
            {
                return oldInt;
            }

        }

        public DateTime GetLatestValue(DateTime oldDate, DateTime newDate)
        {
            if (!newDate.IsNull() && newDate != DateTime.MinValue)
            {
                return newDate;
            }
            else
            {
                return oldDate;
            }

        }
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}
