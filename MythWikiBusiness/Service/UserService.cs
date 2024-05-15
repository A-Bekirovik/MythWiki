using System;
using System.Threading;
using System.Linq;
using MythWikiData.DTO;
using MythWikiData.Repository;
using MythWikiBusiness.Models;

namespace MythWikiBusiness.Services
{
    public class UserService 
    {
        List<UserDTO> usersDTO = new List<UserDTO>();

        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public List<User> GetAllUsers()
        {
            usersDTO = _userRepository.GetAllUsers();
            List<User> users = usersDTO.Select(dto => new User(dto)).ToList();
            return users;
        }
    }
}
