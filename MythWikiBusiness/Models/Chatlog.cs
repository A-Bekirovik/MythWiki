using System;
namespace MythWikiBusiness.Models
{
	public class Chatlog
	{
        public int SubjectID { get; set; }
        public int UserID { get; set; }
        public int ReplyID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Chatlog()
		{
		}
	}
}

