using System;
namespace MythWiki.Models
{
	public class Subject
	{
        public int SubjectID { get; private set; }
        public string Title { get; private set; }
        public string Text { get; private set; }
        public string Image { get; private set; }
        public DateTime Date { get; private set; }
        public List<User> Authors { get; private set; }

        public Subject()
        {

		}
	}
}


