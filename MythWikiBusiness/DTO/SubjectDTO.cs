using System;

namespace MythWikiBusiness.DTO
{
    public class SubjectDTO
    {
        public int SubjectID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int UserID { get; set; }
        public string Image { get; set; }
        public int AuthorID { get; set; }
        public DateTime Date { get; set; }
        public string AuthorName { get; set; }
        public string EditorName { get; set; } 
    }

}

