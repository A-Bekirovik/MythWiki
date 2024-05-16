using System;
using System.Threading;
using System.Linq;
using MythWikiBusiness.DTO;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.Models;

namespace MythWikiBusiness.Services
{
    public class UserService : IUserRepo
    {
        List<UserDTO> usersDTO = new List<UserDTO>();        

        private readonly IUserRepo _userRepository;

        public UserService(IUserRepo userrepo)
        {
            _userRepository = userrepo;
        }

        public List<UserDTO> GetAllUsers()
        {
            usersDTO = _userRepository.GetAllUsers();
            List<User> userss = new List<User>();

            foreach (var dto in usersDTO)
            {
                userss.Add(new User(dto));
            }
            //List<UserDTO> users = usersDTO.Select(dto => new User(dto)).ToList();
            return userss;
        }
    }
}
