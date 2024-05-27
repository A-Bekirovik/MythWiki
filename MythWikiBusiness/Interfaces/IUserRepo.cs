using System;
using System.Collections.Generic;
using MythWikiBusiness.Models;
using MythWikiBusiness.DTO;

namespace MythWikiBusiness.IRepository
{
    public interface IUserRepo
    {
        List<UserDTO> GetAllUsers();
        UserDTO GetUserByUsername(string username);
        UserDTO GetUserById(int userId);
        void AddUser(UserDTO userDTO);
    }
}

