using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudyMate.Helpers
{
    public static class RegexValidator
    {
        // Alphanumeric, between 3 and 15 characters
        public static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            string pattern = @"^[a-zA-Z0-9]{3,15}$";
            return Regex.IsMatch(username, pattern);
        }

        // 8+ chars, 1 uppercase, 1 lowercase, 1 number, 1 special char (@$!%*?&#)
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$";
            return Regex.IsMatch(password, pattern);
        }
    }
}
