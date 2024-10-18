using System;
using System.Diagnostics;

namespace BookStoreLIB {
    public class UserData {
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public bool LogIn(string loginName, string password) {
            var dbUser = new DALUserInfo();
            if (CheckBlankPw(password) || CheckPwStartsWithNonLetter(password) || CheckPwLength(password)) {
                UserId = -1;
                return false;
            }
            UserId = dbUser.LogIn(loginName, password);
            if (UserId > 0) {
                LoginName = loginName;
                Password = password;
                Debug.WriteLine("Successful login");
                return true;
            } else {
                return false;
            }
        }
        public bool CheckBlankPw(string password) {
            if (string.IsNullOrEmpty(password)) {
                Debug.WriteLine("Password is empty");
                return true;
            }
            return false;
        }
        public bool CheckPwStartsWithNonLetter(string password) {
            if (!char.IsLetter(password[0])) {
                Debug.WriteLine("Password starts with a non-letter");
                return true;
            }
            return false;
        }
        public bool CheckPwLength(string password) {
            if (password.Length < 6) {
                Debug.WriteLine("Password is less than 6 characters");
                return true;
            }
            return false;
        }
    }
}