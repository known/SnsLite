using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

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

    public class UserService : ServiceBase, IUserService
    {
        public User GetUser(string account)
        {
            var sql = "select * from K_Users where Account=@Account";
            return connection.QuerySingleOrDefault<User>(sql, new { Account = account });
        }

        public Result<User> Login(string account, string password)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(account))
                error += "账号不能为空！";
            if (string.IsNullOrEmpty(password))
                error += "密码不能为空！";

            if (!string.IsNullOrEmpty(error))
                return Result.Error<User>(error);

            var user = GetUserByAccount(account);
            if (user == null)
                return Result.Error<User>("用户不存在！");

            if (!user.Enabled)
                return Result.Error<User>("用户已禁用！");

            if (user.Password != password)
                return Result.Error<User>("密码不正确！");

            user.LastLoginTime = DateTime.Now;
            user.LoginCount = user.LoginCount + 1;

            var sql = "update K_Users set LastLoginTime=@LastLoginTime,LoginCount=@LoginCount where Id=@Id";
            connection.Execute(sql, user);

            return Result.Success("登录成功!", user);
        }

        public Result<User> Register(string account, string password)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(account))
                error += "账号不能为空！";
            if (string.IsNullOrEmpty(password))
                error += "密码不能为空！";

            if (!string.IsNullOrEmpty(error))
                return Result.Error<User>(error);

            var user = GetUserByAccount(account);
            if (user != null)
                return Result.Error<User>("账号已存在！");

            user = new User
            {
                Id = Utils.NewGuid,
                Name = account.ToUpper(),
                Account = account,
                Password = password,
                RegisterTime = DateTime.Now,
                LastLoginTime = DateTime.Now,
                LoginCount = 1
            };

            if (Validator.IsEmail(account))
                user.Email = account;
            if (Validator.IsMobile(account))
                user.Mobile = account;

            var sql = "insert into K_Users(Id,Name,Account,Password,Email,Mobile,RegisterTime,LastLoginTime,LoginCount) values(@Id,@Name,@Account,@Password,@Email,@Mobile,@RegisterTime,@LastLoginTime,@LoginCount)";
            connection.Execute(sql, user);

            return Result.Success("注册成功!", user);
        }

        public Result UpdateAccount(string userId, string newAccount)
        {
            if (!ExistsUser(connection, null, userId))
                return Result.Error("用户不存在！");

            var sql = "select count(*) from K_Users where Account=@account";
            var count = connection.ExecuteScalar<int>(sql, new { account = newAccount });
            if (count > 0)
                return Result.Error("账号已存在！");

            sql = "update K_Users set Account=@Account where Id=@Id";
            connection.Execute(sql, new { Account = newAccount, Id = userId });
            return Result.Success("修改成功！");
        }

        public Result UpdatePassword(string userId, string oldPassword, string newPassword)
        {
            if (!ExistsUser(connection, null, userId))
                return Result.Error("用户不存在！");

            if (string.IsNullOrWhiteSpace(newPassword))
                return Result.Error("新密码不能为空！");

            var sql = "select Password from K_Users where Id=@Id";
            var password = connection.ExecuteScalar<string>(sql, new { Id = userId });
            if (password != oldPassword)
                return Result.Error("原密码不正确！");

            sql = "update K_Users set Password=@Password where Id=@Id";
            connection.Execute(sql, new { Password = newPassword, Id = userId });
            return Result.Success("修改成功！");
        }

        private User GetUserByAccount(string account)
        {
            var sql = "select * from K_Users where Account=@account or Email=@account or Mobile=@account";
            return connection.QuerySingleOrDefault<User>(sql, new { account = account });
        }

        protected bool ExistsUser(IDbConnection conn, IDbTransaction trans, string userId)
        {
            var sql = "select count(*) from K_Users where Id=@Id";
            var count = conn.ExecuteScalar<int>(sql, new { Id = userId }, trans);
            return count > 0;
        }

        protected void UpdateUser(IDbConnection conn, IDbTransaction trans, User user)
        {
            if (user == null)
                Check.Throw("User对象为空！");

            if (!ExistsUser(conn, trans, user.Id))
                Check.Throw("用户不存在！");

            var errors = new List<string>();
            if (!string.IsNullOrWhiteSpace(user.Email) && !Validator.IsEmail(user.Email))
                errors.Add("Email格式不正确！");
            if (!string.IsNullOrWhiteSpace(user.Mobile) && !Validator.IsMobile(user.Mobile))
                errors.Add("手机号码不正确！");

            if (errors.Count > 0)
                Check.Throw(string.Join(Environment.NewLine, errors));

            var sql = "update K_Users set Name=@Name,Email=@Email,Mobile=@Mobile where Id=@Id";
            conn.Execute(sql, user, trans);
        }
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
