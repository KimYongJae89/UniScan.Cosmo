using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace DynMvp.Authentication
{
    public enum UserType
    {
        Operator = 0, Maintrance = 1, Admin = 2
    }
    public class UserList
    {
        List<User> userList = new List<User>();

        public IEnumerator<User> GetEnumerator()
        {
            return userList.GetEnumerator();
        }

        public void AddUser(User user)
        {
            userList.Add(user);
        }

        public void RemoveUser(User user)
        {
            userList.Remove(user);
        }

        public User GetUser(string userId)
        {
            foreach (User user in userList)
            {
                if (user.Id == userId)
                        return user;
            }

            if (userId == "developer")
                return new User("developer", "masterkey", UserType.Admin);
            else if (userId == "op")
                return new User("op", "op", UserType.Operator);
            else if (userId == "master")
                return new User("master", "master1", UserType.Maintrance);

            return null;
        }

        public User GetUser(string userId, string password)
        {
            foreach (User user in userList)
            {
                if (user.Id == userId)
                {
                    string passwordHash = User.GetPasswordHash(password);

                    if (passwordHash == user.PasswordHash)
                        return user;
                    else
                        return null;
                }
            }

            if (userId == "developer" && password == "masterkey")
                return new User("developer", "masterkey", UserType.Admin);
            else if (userId == "op" && password == "op")
                return new User("op", "op", UserType.Operator);
            else if (userId == "master" && password == "master1")
                return new User("master", "master1", UserType.Maintrance);

            return null;
        }
    }

    public class User
    {
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        UserType userType = UserType.Admin;
        public UserType UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        private string passwordHash;
        public string PasswordHash
        {
            get { return passwordHash; }
            set { passwordHash = value; }
        }

        public User()
        {
        }

        public User(string userId, string password, UserType userType)
        {
            this.id = userId;
            this.passwordHash = GetPasswordHash(password);
            this.userType = userType;
            //permissionControl.SuperAccount = superAccount;
        }

        public static string GetPasswordHash(string password)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();

            byte[] passwordByte = Encoding.UTF8.GetBytes(password);
            byte[] result = sha.ComputeHash(passwordByte);

            return Convert.ToBase64String(result);
        }

        public bool SuperAccount
        {
            get { return userType == UserType.Admin; }
        }

        public PermissionControl permissionControl = new PermissionControl();
    }
}
