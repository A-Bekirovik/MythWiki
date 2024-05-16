using System;
using System.Threading;
using System.Linq;
using MythWikiBusiness.DTO;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.Models;

namespace MythWikiBusiness.Services
{
    public class UserService
    {
        List<UserDTO> usersDTO = new List<UserDTO>();        

        private readonly IUserRepo _userRepository;

        public UserService(IUserRepo userrepo)
        {
            _userRepository = userrepo;
        }

        public List<User> GetAllUsers()
        {
            List<UserDTO> usersDTO = new List<UserDTO>();
            List<User> users = new List<User>();
            usersDTO = _userRepository.GetAllUsers();	   
            foreach (var dto in usersDTO)
            {
                users.Add(new User(dto));
            }

            return users;
        }
    }
}
