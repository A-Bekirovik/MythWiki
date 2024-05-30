using System;
using System.Collections.Generic;
using MythWikiBusiness.Models;
using MythWikiBusiness.DTO;
using MythWikiBusiness.ErrorHandling;

namespace MythWikiBusiness.IRepository
{
	public interface ISubjectRepo
	{
		List<SubjectDTO> GetAllSubjects();
        SubjectDTO CreateSubject(string title, string text, int editorid, string imagelink);
        SubjectDTO GetSubjectById(int id);
        bool DeleteSubject(int subjectID);
        bool EditSubject(SubjectDTO subject);
    }
}

