using System;
namespace MythWikiLogic.Models
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
	}
}

