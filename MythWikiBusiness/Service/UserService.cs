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

        private readonly IUserRepo interfacerepo;



        public UserService(IUserRepo userrepo)
        {
            _userRepository = userrepo;
        }

        public List<UserDTO> GetAllUsers()
        {
            usersDTO = _userRepository.GetAllUsers();
            List<UserDTO> users = new List<UserDTO>();

            foreach (var dto in usersDTO)
            {
                users.Add(dto);
            }

            return users;
        }
    }
}
