using System;
namespace MythWiki.Models
{
	public class Subject
	{
        public int SubjectID { get;  set; }
        public string Title { get;  set; }
        public string Text { get;  set; }
        public string Image { get;  set; }
        public DateTime Date { get;  set; }
        public List<User> Authors { get;  set; }

        public Subject()
        {

		}
	}
}


