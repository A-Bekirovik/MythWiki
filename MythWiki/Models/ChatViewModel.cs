using System;
namespace MythWiki.Models
{
	public class Chat
	{
		public UserViewModel user { get; private set; }
		public string Title { get; private set; }
		public string Text { get; private set; }
		public DateTime Date { get; private set; }

		public Chat()
		{
		}
	}
}

