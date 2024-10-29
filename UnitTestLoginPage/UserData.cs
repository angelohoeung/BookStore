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

        public Response GetAccountInfo (int userId)
        {
            DALAccount dalAccount = new DALAccount();
            DataSet dsAccount = dalAccount.GetAccountInfo(userId);
            if(dsAccount==null)
            {
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

        public Response UpdateAccount(int userId, string userName, string password, string fullName) {
            //Check if the username exists
            DataSet dscheck = new DALAccount().GetAccountInfoByName(userName);

            if (dscheck.Tables["Accounts"].Rows.Count > 0)
            {
                return new Response() { message = "The username has already existed", err = true};
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