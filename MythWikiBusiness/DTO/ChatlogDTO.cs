using System;
namespace MythWikiData.DTO
{
	public class ChatlogDTO
	{
		public int SubjectID { get; set; }
		public int UserID { get; set; }
		public int ReplyID { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
		public DateTime Date { get; set; }


		public ChatlogDTO()
		{
		}
	}
}

