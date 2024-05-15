using System;
using MythWikiData.DTO;
namespace MythWikiBusiness.Models
{
	public class UserModel
	{
        public int UserID { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public bool Admin { get; private set; }

        public UserModel() 
	    { 
	    }

        public UserModel(UserDTO userdto)
		{
            Name = userdto.Name;
            UserID = userdto.UserID;
		}
	}
}

