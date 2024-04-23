using System;
namespace MythWiki.Models
{
	public class User
	{
		public int UserID { get; private set; }
		public string Name { get; private set; }
		public string Password { get; private set; }
		public string Email { get; private set; }
		public bool Admin { get; private set; }
		public List<Subject> Favorites { get; private set; }
		public List<Subject> EditedPages { get; private set; }

		public User()
		{

		}
	}
}

