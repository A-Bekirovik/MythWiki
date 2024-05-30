using System;
using MythWikiBusiness.DTO;

namespace MythWikiBusiness.Models
{
    public class Subject
    {
        public int SubjectID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int AuthorID { get; set; } 
        public int EditorID { get; set; } 
        public string Image { get; set; }
        public DateTime Date { get; set; }
        public string AuthorName { get; set; }  
        public string EditorName { get; set; } 

        public Subject()
        {
        }

        public Subject(SubjectDTO subjectdto)
        {
            SubjectID = subjectdto.SubjectID;
            Title = subjectdto.Title;
            Text = subjectdto.Text;
            AuthorID = subjectdto.AuthorID;
            EditorID = subjectdto.EditorID;
            Image = subjectdto.Image;
            Date = subjectdto.Date;
            AuthorName = subjectdto.AuthorName;
            EditorName = subjectdto.EditorName;
        }
    }
}
