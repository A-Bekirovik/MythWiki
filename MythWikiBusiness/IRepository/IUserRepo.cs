using System;
using MythWikiBusiness.Models;
namespace MythWikiBusiness.IRepository
{
    public interface IUserService
    { 
        List<User> GetAllUsers();
    }

}

