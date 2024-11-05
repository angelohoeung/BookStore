using System;
using System.Data;
using System.Diagnostics;

namespace BookStoreLIB {
    public class Response
    {
        public string message = "";
        public bool err = false;
        public DataSet data = null;
    }

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
        public bool SignUp(string signUpName, string password, string confirmPassword, string fullName) {
            var dbUser = new DALUserInfo();
            if (CheckBlankUsername(signUpName) || CheckUsernameStartsWithNonLetter(signUpName) || CheckUsernameLength(signUpName)) {
                UserId = -1;
                return false;
            }

            if (CheckBlankPw(password) || CheckPwStartsWithNonLetter(password) || CheckPwLength(password) || CheckMatchingPasswords(password, confirmPassword) || CheckPasswordContainsNonAlphaNumeric(password)) {
                UserId = -1;
                return false;
            }

            if (CheckBlankFullName(fullName) || CheckFullNameStartsWithNonLetter(fullName) || CheckFullNameContainsNonAlphaNumeric(fullName)) {
                UserId = -1;
                return false;
            }

            UserId = dbUser.SignUp(signUpName, password, fullName);
            if(UserId > 0) {
                LoginName = signUpName;
                Password = password;
                Debug.WriteLine("Successful signup");
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
        public bool CheckMatchingPasswords(string password, string confirmPassword) {
            if (password != confirmPassword) {
                Debug.WriteLine("Passwords do not match.");
                return true;
            }
            return false;
        }

        public bool CheckPasswordContainsNonAlphaNumeric(string password) {
            bool hasLetter = false;
            bool hasNumber = false;
            for (int i = 0; i < password.Length; i++) {
                char character = password[i];

                if (IsLetter(character)) {
                    hasLetter = true;
                } else if (IsDigit(character)) {
                    hasNumber = true;
                } else {
                    Debug.WriteLine("Password cannot contain symbols");
                    return true;
                }
            }

            if (!hasLetter || !hasNumber) {
                Debug.WriteLine("A valid password needs to have characters with both letters and numbers.");
                return true;
            }
            return false;
        }

        private bool IsLetter(char character) {
            return (character >= 65 && character <= 90) || (character >= 97 && character <= 122);
        }

        private bool IsDigit(char character) {
            return character >= 48 && character <= 57;
        }

        public bool CheckBlankUsername(string username) {
            if (string.IsNullOrEmpty(username)) {
                Debug.WriteLine("Username is empty.");
                return true;
            }
            return false;
        }
        public bool CheckUsernameStartsWithNonLetter(string username) {
            if (!char.IsLetter(username[0])) {
                Debug.WriteLine("Username starts with a non-letter.");
                return true;
            }
            return false;
        }
        public bool CheckUsernameLength(string username) {
            if (username.Length < 4) {
                Debug.WriteLine("Username is less than 4 characters.");
                return true;
            }
            return false;
        }

        public bool CheckBlankFullName(string fullName) {
            if (string.IsNullOrEmpty(fullName)) {
                Debug.WriteLine("Full name is empty.");
                return true;
            }
            return false;
        }
        public bool CheckFullNameStartsWithNonLetter(string fullName) {
            if (!char.IsLetter(fullName[0])) {
                Debug.WriteLine("Full name starts with a non-letter.");
                return true;
            }
            return false;
        }
        public bool CheckFullNameContainsNonAlphaNumeric(string fullName) {
            for (int i = 0; i < fullName.Length; i++) {
                char character = fullName[i];

                if (!IsLetter(character) && !IsDigit(character) && !(character == ' ')) {
                    Debug.WriteLine("Full name must contain only letters and numbers");
                    return true;
                }
            }
            return false;
        }


        public Response GetAccountInfo (int userId)
        {
            DALAccount dalAccount = new DALAccount();
            DataSet dsAccount = dalAccount.GetAccountInfo(userId);
            if (dsAccount == null || dsAccount.Tables["Accounts"].Rows.Count == 0) {
                return new Response()
                {
                    message = "Some thing has happens during the fetching account data process",
                    err = true
                };
            }
            else
            {
                return new Response()
                {
                    message = "Fetched successfully",
                    err = false,
                    data = dsAccount
                };
            }
        }

        public Response UpdateAccount(int userId, string userName, string password, string fullName, bool isUsernameUpdate) {
            // Check if the user ID exists
            if (new DALAccount().GetAccountInfo(userId)?.Tables["Accounts"].Rows.Count == 0) {
                return new Response() { message = "No account found with the specified UserID", err = true };
            }

            //Check if the username exists
            if (isUsernameUpdate) {
                DataSet dscheck = new DALAccount().GetAccountInfoByName(userName);
                if (dscheck.Tables["Accounts"].Rows.Count > 0) {
                    return new Response() { message = "The username has already existed", err = true };
                }
            }

            if (CheckBlankPw(password) || CheckPwStartsWithNonLetter(password) || CheckPwLength(password) || fullName.Length < 6)
            {
                string msg = "Username and password does not satisfy all conditions";
                return new Response() { message = msg, err = true };
            }
            else
            {
                DALAccount dalAccount = new DALAccount();
                bool error = dalAccount.UpdateAccount(userId, userName, password, fullName);
                return new Response() { message = "Updated successfully", err = false};
            }
        }

        public Response DeletedAccount (int userId)
        {
            try
            {
                DALAccount dlaAccount = new DALAccount();
                bool result = dlaAccount.DeleteAccount(userId);
                if (result == true)
                {
                    return new Response()
                    {
                        message = "Deleted account",
                        err = false,
                    };
            }
                else
                {
                    return new Response()
                    {
                        message = "Something happens during the process",
                        err = true
                    };
                }

            }
            catch (Exception e)
            {
                return new Response()
                {
                    message = $"Error: {e.Message}",
                    err = true
                };
            }
        }
    }

}