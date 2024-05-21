using System;
using System.Threading;
using System.Linq;
using MythWikiBusiness.DTO;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.Models;

namespace MythWikiBusiness.Services
{
	public class SubjectService
	{
		List<SubjectDTO> subjectDTO = new List<SubjectDTO>();

		private readonly ISubjectRepo _subjectRepository;

		public SubjectService(ISubjectRepo subjectrepo) 
		{
			_subjectRepository = subjectrepo; 
		}

		public List<Subject> GetAllSubjects() 
		{
			List<SubjectDTO> subjectDTO = new List<SubjectDTO>();
			List<Subject> subjects = new List<Subject>();
			subjectDTO = _subjectRepository.GetAllSubjects();
            foreach (var dto in subjectDTO)
            {
                subjects.Add(new Subject(dto));
            }
            return subjects;
		}

		public Subject CreateSubject(string title, string text) 
		{
			SubjectDTO newsubjectDTO = _subjectRepository.CreateSubject(title, text);
			Subject newsubject = new Subject(newsubjectDTO);
			return newsubject;			 
		}
	}
}

