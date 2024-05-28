using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MythWikiBusiness.DTO;
using MythWikiBusiness.ErrorHandling;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.Models;

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
            try
            {
                List<UserDTO> usersDTO = _userRepository.GetAllUsers();
                foreach (var dto in usersDTO)
                {
                    users.Add(new User(dto));
                }
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Cant create new subject due to Database", dbex);
            }
            catch (ArgumentException argex)
            {
                throw new UserError("Cant create new subject due to Service", argex);
            }

            return users;
        }

        public User Register(string username, string password, string email)
        {
            try
            {
                var existingUser = _userRepository.GetUserByUsername(username);
                if (existingUser != null)
                {
                    throw new ArgumentException("Username already exists.");
                }

                var userDTO = new UserDTO
                {
                    Name = username,
                    Password = password, // Store plain text password
                    Email = email
                };

                _userRepository.AddUser(userDTO);

                return new User(userDTO);
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError(dbex.Message, dbex);
            }
            catch (ArgumentException argex)
            {
                throw new UserError(argex.Message, argex);
            }
        }

        public User Authenticate(string username, string password)
        {
            try
            {
                var userDTO = _userRepository.GetUserByUsername(username);
                if (userDTO == null || userDTO.Password != password) // Compare plain text password
                {
                    throw new UnauthorizedAccessException("Invalid username or password.");
                }

                return new User(userDTO);
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError(dbex.Message, dbex);
            }
            catch (UnauthorizedAccessException UAex)
            {
                throw new UserError(UAex.Message, UAex);
            }
        }
    }
}
