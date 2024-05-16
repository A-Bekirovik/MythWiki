using System;
using System.Collections.Generic;
using MythWikiBusiness.Models;
using MythWikiBusiness.DTO;

namespace MythWikiBusiness.IRepository
{
    public interface IUserRepo
    {
        List<UserDTO> GetAllUsers();
    }
}

