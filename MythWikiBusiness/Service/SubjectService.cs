using System;
using System.Threading;
using System.Linq;
using MythWikiBusiness.DTO;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.Models;
using MythWikiBusiness.ErrorHandling;

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

		public ServiceReponse CreateSubject(string title, string text, int editorid, string imagelink, string authorname)
        {
            ServiceReponse response = new ServiceReponse { Succes = false };

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(text))
            {
                response.ErrorMessage = "Title and Text need to be filled!";
                return response;
            }

            try
            {
                SubjectDTO newsubjectDTO = _subjectRepository.CreateSubject(title, text, editorid, imagelink, authorname);
                Subject newsubject = new Subject(newsubjectDTO);
                response.Succes = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;		 
		}

        public Subject GetSubjectById(int id)
        {
            var subjectDTO = _subjectRepository.GetSubjectById(id);
            if (subjectDTO == null)
            {
                return null;
            }
            return new Subject(subjectDTO);
        }

        public bool DeleteSubject(int subjectID)
        {
            return _subjectRepository.DeleteSubject(subjectID);
        }
    }
}