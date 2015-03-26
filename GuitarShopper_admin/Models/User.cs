using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GuitarShopper_admin.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Gelieve een emailadres op te geven")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gelieve een paswoord op te geven")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public partial class user
    {
        public static user CheckAuth(UserLoginModel ulm)
        {
            DataClassesDataContext dc = new DataClassesDataContext();

            user u = dc.users.FirstOrDefault(usr => usr.email == ulm.Email);
            if (u == null) return null;

            string dbPass = u.password;
            string passwEnc = sha256(ulm.Password);
            if (passwEnc == dbPass) return u;

            return null;
        }

        //http://stackoverflow.com/a/14709940
        private static string sha256(string password)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            foreach (byte bit in crypto)
            {
                hash += bit.ToString("x2");
            }
            return hash;
        }

        public static bool UserExists(int userId)
        {
            DataClassesDataContext dc = new DataClassesDataContext();
            return dc.users.FirstOrDefault(u => u.id == userId) != null;
        }

        public static bool UserExists(string email)
        {
            DataClassesDataContext dc = new DataClassesDataContext();

            return dc.users.FirstOrDefault(u => u.email == email) != null;
        }
    }
}