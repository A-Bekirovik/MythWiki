using System;
namespace MythWikiDAL.DTO
{
	public class PersonalchatlogsDTO
	{
		public int UserID { get; set; }
		public int SubjectID { get; set; }
		public string Text { get; set; }

		public PersonalchatlogsDTO()
		{
		}
	}
}

