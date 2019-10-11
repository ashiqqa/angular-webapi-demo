using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApi.Models;

namespace TestWebApi.DbContext
{
    public class UserDb
    {
        private static List<User> users = new List<User>()
        {
            new User{Id=1, Name="Ashiq", Email="ashiq@gmail.com", Password="123"},
            new User{Id=2, Name="Shopon", Email="shopon@gmail.com", Password="abc"},
            new User{Id=3, Name="Shakib", Email="shakib@gmail.com", Password="xyz"},
            new User{Id=4, Name="Md Ashiqur Rahman", Email="ars@gmail.com", Password="ars123"}
        };

        public static List<User> GetAll()
        {
            return users;
        }
        public static User GetbyId(int id)
        {
            var user= users.Find(c=>c.Id==id);
            return user;
        }

        public static User Login(string email, string password)
        {
            var user = users.FirstOrDefault(c => c.Email == email && c.Password == password);
            return user;
        }

        public static bool Save(User user)
        {
            if (user == null)
            {
                throw new Exception("Null can't be saved!");
            }
            int preUserCount = users.Count;
            user.Id = users.Count + 1;
            users.Add(user);
            return users.Count > preUserCount;
        }
        public static bool Update(User user)
        {
            if (user == null)
            {
                throw new Exception("Null can't be update!");
            }
            User oldUser = users.Find(c => c.Id == user.Id);
            oldUser.Name = user.Name;
            oldUser.Email = user.Email;
            oldUser.Password = user.Password;
            return true;
        }
        public static bool Delete(int id)
        {
            int prevUserCount = users.Count;
            int index = users.FindIndex(c =>c.Id == id);
            users.RemoveAt(index);
            return prevUserCount > users.Count;
        }
    }
}