using System;
using MythWikiBusiness.DTO;
using MythWikiBusiness.ErrorHandling;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.Models;
using static System.Net.Mime.MediaTypeNames;

namespace MythWikiBusiness.Services
{
    public class UserService
    {
        private readonly IUserRepo _userRepository;

        public UserService(IUserRepo userrepo)
        {
            _userRepository = userrepo;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            List<UserDTO> usersDTO = new List<UserDTO>();
            try
            {
                usersDTO = _userRepository.GetAllUsers();
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Cant create new subject due to Database", dbex);
            }
            foreach (var dto in usersDTO)
            {
                users.Add(new User(dto));
            }
            return users;
        }

        public User Register(string username, string password, string email)
        {
            UserDTO existingUser = _userRepository.GetUserByUsername(username);

            if (existingUser != null)
            {
                throw new UserError("Username already exists.");
            }

            UserDTO userdto;

            try
            {	
                userdto = _userRepository.AddUser(username, password, email);
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError(dbex.Message, dbex);
            }

            return new User(userdto);
        }

        public User Authenticate(string username, string password)
        {
            UserDTO userDTO;

            try
            {
                userDTO = _userRepository.GetUserByUsername(username);
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError(dbex.Message, dbex);
            }

            if (userDTO == null || userDTO.Password != password)
            {
                throw new UserError("Invalid username or password.");
            }
            return new User(userDTO);
        }
    }
}
