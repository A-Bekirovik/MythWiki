using System;
using MythWikiDAL.DTO;
using MythWikiDAL.Repository;

namespace MythWikiLogic.Services
{
    public class UserService 
    {
        List<UserDTO> users = new List<UserDTO>();

        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public List<UserDTO> GetAllUsers()
        {
            users = _userRepository.GetAllUsers();
            return users;
        }
    }
}
