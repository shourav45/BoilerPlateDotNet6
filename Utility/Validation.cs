using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class Validation
    {
        public static string UserValidation(UserAccount user)
        {
            if (string.IsNullOrEmpty(user.UserPassword.Trim())) return "password can't be empty.";
            else if (user.UserPassword.Length < 6) return "Minimum password length is six character.";
            else return "";
        }
    }
}
