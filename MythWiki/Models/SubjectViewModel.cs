using System;
namespace MythWiki.Models
{
	public class SubjectViewModel
	{
        public int SubjectID { get;  set; }
        public string Title { get;  set; }
        public string Text { get;  set; }
        public string Image { get;  set; }
        public DateTime Date { get;  set; }
        public string Authors { get;  set; }

        public SubjectViewModel()
        {

		}
	}
}


