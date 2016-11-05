using System;

namespace Known.Core
{
    public interface IUserService
    {
        User GetUser(string account);
        Result<User> Login(string account, string password);
        Result<User> Register(string account, string password);
        Result UpdateAccount(string userId, string newAccount);
        Result UpdatePassword(string userId, string oldPassword, string newPassword);
    }

    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public int LoginCount { get; set; }
        public bool Enabled { get; set; }
    }
}
