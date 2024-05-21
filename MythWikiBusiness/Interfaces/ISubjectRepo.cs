using System;
using MythWikiBusiness.Models;
using MythWikiBusiness.DTO;
using MythWikiData.DTO;

namespace MythWikiBusiness.IRepository
{
	public interface ISubjectRepo
	{
		List<SubjectDTO> GetAllSubjects();
		SubjectDTO CreateSubject(string title, string text);

    }
}

