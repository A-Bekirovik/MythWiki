using System;
using MythWikiLogic.Models;
namespace MythWikiLogic.IRepository
{
    public interface IUserService
    { 
        List<UserModel> GetAllUsers();
    }

}

