using System;
using MythWikiBusiness.DTO;
namespace MythWikiBusiness.Models
{
	public class User
	{
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }

        public User() 
	    { 
	    }

        public User(UserDTO userdto)
		{
            Name = userdto.Name;
            UserID = userdto.UserID;
            Password = userdto.Password;
            Email = userdto.Email;
		}
	}
}
