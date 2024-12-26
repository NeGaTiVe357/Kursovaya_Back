using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Kursovoy_project_electronic_shop.Interfaces;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Kursovoy_project_electronic_shop.Services
{
    public class UserService : IUserService
    {
        private readonly ElectronicShopDbContext _electronicShopDbContext;

        public UserService(ElectronicShopDbContext electronicShopDbContext)
        {
            _electronicShopDbContext = electronicShopDbContext;
        }
        public Guid Register(Contracts.UserRegisterCredentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.Name) ||
                string.IsNullOrEmpty(credentials.Login) ||
                string.IsNullOrEmpty(credentials.Password))
            {
                throw new ArgumentException("Name, Login, and Password cannot be null or empty.");
            }

            var user = new User
            {
                UserUid = Guid.NewGuid(),
                Name = credentials.Name,
                Login = credentials.Login,
                Password = GetHash(credentials.Password),
                IsAdmin = false,
            };

            _electronicShopDbContext.Add(user);
            _electronicShopDbContext.SaveChanges();
            return user.UserUid;
        }


        public Guid? Login(Contracts.UserLoginCredentials credentials)
        {
            var hashedPassword = GetHash(credentials.Password);

            var user = _electronicShopDbContext.Set<User>().SingleOrDefault(x => x.Login == credentials.Login && x.Password == hashedPassword);

            return user?.UserUid;
        }
        private string GetHash(string password)
        {
            using var sha = SHA512.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToHexString(bytes);
        }
        public List<Contracts.User>? GetAllUsers()
        {
            var users = _electronicShopDbContext.Set<User>().ToList();

            if (users.Count == 0) { return null; }

            return users.Select(user => new Contracts.User
            {
                UserUid = user.UserUid,
                Name = user.Name,
                Login = user.Login,
                Password = GetHash(user.Password),
                Email = user.Email,
                IsAdmin = user.IsAdmin,
            }).ToList();
        }
        public Contracts.User? GetSingleUser(Guid userUid)
        {
            var user = _electronicShopDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return null; }

            return new Contracts.User
            {
                UserUid = user.UserUid,
                Name = user.Name,
                Login = user.Login,
                Email = user.Email,
                Password = GetHash(user.Password),
                IsAdmin = user.IsAdmin,
            };
        }
        public bool DeleteUser(Guid userUid)
        {
            var user = _electronicShopDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return false; }

            _electronicShopDbContext.Remove(user);

            return _electronicShopDbContext.SaveChanges() > 0;
        }
        public bool UpdateUser(Guid userUid, Contracts.UserUpdate userUpdate)
        {
            var user = _electronicShopDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return false; }

            user.Name = userUpdate.Name;
            user.Login = userUpdate.Login;
            user.Password = GetHash(userUpdate.Password);
            user.Email = userUpdate.Email;

            return _electronicShopDbContext.SaveChanges() > 0;
        }
        public Contracts.UserInfo? GetUserInfo(Guid userUid)
        {
            var user = _electronicShopDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return null; }

            return new Contracts.UserInfo
            {
                Name = user.Name,
                Login = user.Login,
                Password = user.Password,
                Email = user.Email
            };
        }
        public bool CheckLogin(string login)
        {
            var user = _electronicShopDbContext.Set<User>().SingleOrDefault(x => x.Login == login);

            if (user == null) { return false; }

            return true;
        }

        public string? GetLogin(Guid userUid)
        {
            var user = _electronicShopDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return null; }

            return user.Login;
        }

        public bool IsAdmin(Guid userUid)
        {
            var user = _electronicShopDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return false; }

            if (user.IsAdmin == true)
            {
                return true;
            }

            return false;
        }

        public bool IsUserExists(Guid? userUid)
        {
            var user = _electronicShopDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return false; }

            return true;
        }

        public bool CheckLoginRegex(string login)
        {
            var regex = new Regex(@"^[a-zA-Z0-9][\w]{4,}$");

            if (!regex.IsMatch(login))
            {
                return false;
            }

            return true;
        }

        public bool CheckEmailRegex(string email)
        {
            var regex = new Regex(@"^[\w-.]+@([\w -]+.)+[\w-]{2,4}$");

            if (!regex.IsMatch(email))
            {
                return false;
            }

            return true;
        }

    }
}
