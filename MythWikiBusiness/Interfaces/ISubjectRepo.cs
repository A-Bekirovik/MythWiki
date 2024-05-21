using System;
using System.Collections.Generic;
using MythWikiBusiness.Models;
using MythWikiBusiness.DTO;

namespace MythWikiBusiness.IRepository
{
	public interface ISubjectRepo
	{
		List<SubjectDTO> GetAllSubjects();
		SubjectDTO CreateSubject(string title, string text);
		bool DeleteSubject(int subjectID);
    }
}

