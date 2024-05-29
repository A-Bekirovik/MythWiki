using System;
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
            List<SubjectDTO> subjectDTO = new List<SubjectDTO>();
            List<Subject> subjects = new List<Subject>();
            try 
	        {
                subjectDTO = _subjectRepository.GetAllSubjects();
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Cant create new subject due to Database", dbex);
            }

            foreach (var dto in subjectDTO)
            {
                subjects.Add(new Subject(dto));
            }
            return subjects;
        }

        public Subject CreateSubject(string title, string text, int editorid, string imagelink, int authorID)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(text))
            {
                throw new SubjectError("Cant create subject due to Service: Title and Text need to be filled!");
            }

            SubjectDTO newsubjectDTO;
            try
            {
                newsubjectDTO = _subjectRepository.CreateSubject(title, text, editorid, imagelink, authorID);
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Can't create new subject due to Database: " + dbex.Message, dbex);
            }
            Subject newsubject = new Subject(newsubjectDTO);
            return newsubject;
        }


        public Subject GetSubjectById(int id)
        {
            if (id <= 0)
            {
                throw new SubjectError("Cant get subject due to Service: Subject ID must be greater than 0.");
            }

            SubjectDTO subjectDTO;


            try
            {
                subjectDTO = _subjectRepository.GetSubjectById(id);
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Cant get subject due to Database: " +dbex.Message, dbex);
            }

            if (subjectDTO == null)
            {
                throw new SubjectError("Cant get subject due to Service: Subject not found.");
            }

            var foundsubject = new Subject(subjectDTO);
            return foundsubject;
        }

        public Subject EditSubject(SubjectDTO subjectDTO)
        {
            if (subjectDTO == null || string.IsNullOrWhiteSpace(subjectDTO.Title) || string.IsNullOrWhiteSpace(subjectDTO.Text))
            {              
                throw new SubjectError("Cant edit new subject due to Service: Title and Text need to be filled!");
            }
            bool isUpdated = false;

            try
            {
                isUpdated = _subjectRepository.EditSubject(subjectDTO);
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Cant edit new subject due to Database: " + dbex.Message, dbex);
            }

            if (!isUpdated)
            {
                throw new SubjectError("Cant edit new subject due to Service: Failed to update the subject.");
            }

            var updatedSubject = new Subject(subjectDTO);
            return updatedSubject;
            
        }

        public bool DeleteSubject(int subjectID) 
        {
            if (subjectID <= 0)
            {
                throw new SubjectError("Cant delete subject due to Service: Subject ID must be greater than 0.");
            }

            SubjectDTO subject;
            try
            {
                subject = _subjectRepository.GetSubjectById(subjectID);

            }
            catch(DatabaseError dbex) 
	        {
                throw new DatabaseError("Couldnt delete Subject due to Database: " + dbex.Message, dbex);
	        }

            if (subject == null)
            {
                throw new SubjectError("Cant delete subject due to Service: Subject doesn't exist.");
            }

            var isDeleted = _subjectRepository.DeleteSubject(subjectID);
            return isDeleted;
        }
    }    
}