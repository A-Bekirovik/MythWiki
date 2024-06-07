using System;
using MySql.Data.MySqlClient;
using MythWikiBusiness.DTO;
using MythWikiBusiness.ErrorHandling;
using MythWikiBusiness.IRepository;

namespace UnitTest.FakeDAL
{
    public class FakeUserRepo : IUserRepo
    {
        List<UserDTO> users;

        public FakeUserRepo()
        {
            users = new List<UserDTO>();

            UserDTO user = new UserDTO
            {
                UserID = 1,
                Name = "title",
                Password = "text",
                Email = "Boebeh@gmail.com"
            };

            UserDTO user1 = new UserDTO
            {
                UserID = 2,
                Name = "title1",
                Password = "text1",
                Email = "Boebeh1@gmail.com"
            };

            users.Add(user);
            users.Add(user1);
        }

        public List<UserDTO> GetAllUsers()
        {
            return users;
        }

        public UserDTO GetUserByUsername(string username)
        {
            var user = users.FirstOrDefault(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
            return user;
        }

        public UserDTO GetUserById(int userId)
        {
            var user = users.FirstOrDefault(u => u.UserID == userId);
            return user;
        }

        public UserDTO AddUser(string name, string password, string email)
        {
            UserDTO newUser = new UserDTO()
            {
                UserID = 3,
                Name = name,
                Password = password,
                Email = email
            };
            return newUser;
        }
    }
}

