using System;
using MythWikiBusiness.Models;

namespace MythWikiBusiness.DTO
{
    public class SubjectDTO
    {
        public int SubjectID { get; set; }
        public int EditorID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}

namespace MythWikiBusiness.DTO
{
	public class SubjectDetailDTO
	{
        public int SubjectID { get; set; }
        public int EditorID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string Authors { get; set; }
        public DateTime Date { get; set; }
	}
}

