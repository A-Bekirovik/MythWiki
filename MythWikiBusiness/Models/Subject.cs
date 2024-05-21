using System;
using MythWikiBusiness.DTO;
namespace MythWikiBusiness.Models
{
	public class Subject
	{
        public int SubjectID { get; set; }
        public int EditorID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string Authors { get; set; }
        public DateTime Date { get; set; }

        public Subject()
		{
		}

        public Subject(SubjectDTO subjectdto) 
	    {
            SubjectID = subjectdto.SubjectID;
            EditorID = subjectdto.EditorID;
            Title = subjectdto.Title;            
            Text = subjectdto.Text;
	    }
	}
}