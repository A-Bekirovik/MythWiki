using System;
using MythWikiBusiness.DTO;
namespace MythWikiBusiness.Models
{
	public class User
	{
        public int UserID { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; } // Store hashed passwords, not plain text
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }

        public User() 
	    { 
	    }

        public User(UserDTO userdto)
		{
            Name = userdto.Name;
            UserID = userdto.UserID;
		}
	}
}
