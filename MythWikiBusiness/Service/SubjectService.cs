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
			List<SubjectDTO> subjectDTO = _subjectRepository.GetAllSubjects();
            List<Subject> subjects = new List<Subject>();		
            foreach (var dto in subjectDTO)
            {
                subjects.Add(new Subject(dto));
            }
            return subjects;
		}

		public Subject CreateSubject(string title, string text, int editorid, string imagelink, string authorname, DateTime date)
        {
            Console.WriteLine($"Service - Title: {title}");
            Console.WriteLine($"Service - Text: {text}");
            Console.WriteLine($"Service - EditorID: {editorid}");
            Console.WriteLine($"Service - Image Link: {imagelink}");
            Console.WriteLine($"Service - Author Name: {authorname}");
            Console.WriteLine($"Service - Date: {date}");

            SubjectDTO newsubjectDTO = _subjectRepository.CreateSubject(title, text, editorid, imagelink, authorname, date);
			Subject newsubject = new Subject(newsubjectDTO);
			return newsubject;			 
		}
	}
}

