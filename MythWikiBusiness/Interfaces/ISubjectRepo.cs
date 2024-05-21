using System;
using System.Collections.Generic;
using MythWikiBusiness.Models;
using MythWikiBusiness.DTO;

namespace MythWikiBusiness.IRepository
{
	public interface ISubjectRepo
	{
		List<SubjectDTO> GetAllSubjects();
        SubjectDTO CreateSubject(string title, string text, int editorid, string imagelink, string authorname, DateTime date);

        bool DeleteSubject(int subjectID);
    }
}

