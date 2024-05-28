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
            try 
	        {
                List<SubjectDTO> subjectDTO = _subjectRepository.GetAllSubjects();
                List<Subject> subjects = new List<Subject>();
                foreach (var dto in subjectDTO)
                {
                    subjects.Add(new Subject(dto));
                }
                return subjects;
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Cant create new subject due to Database", dbex);
            }
            catch (ArgumentException argex)
            {
                throw new SubjectError("Cant create new subject due to Service", argex);
            }
		}

        public Subject CreateSubject(string title, string text, int editorid, string imagelink, int authorID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(text))
                {
                    throw new ArgumentException("Title and Text need to be filled!");
                }

                SubjectDTO newsubjectDTO = _subjectRepository.CreateSubject(title, text, editorid, imagelink, authorID);
                Subject newsubject = new Subject(newsubjectDTO);
                return newsubject;
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Can't create new subject due to Database: " + dbex.Message, dbex);
            }
            catch (ArgumentException argex)
            {
                throw new SubjectError("Can't create new subject due to Service: " + argex.Message, argex);
            }
        }


        public Subject GetSubjectById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Subject ID must be greater than 0.");
                }

                var subjectDTO = _subjectRepository.GetSubjectById(id);

                if (subjectDTO == null)
                {
                    throw new ArgumentException("Subject not found.");
                }

                var foundsubject = new Subject(subjectDTO);
                return foundsubject;
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Cant create new subject due to Database: " +dbex.Message, dbex);
            }
            catch (ArgumentException argex)
            {
                throw new SubjectError("Cant create new subject due to Service: " + argex.Message, argex);
            }
        }

        public Subject EditSubject(SubjectDTO subjectDTO)
        {
            try
            {
                if (subjectDTO == null || string.IsNullOrWhiteSpace(subjectDTO.Title) || string.IsNullOrWhiteSpace(subjectDTO.Text))
                {
                    throw new ArgumentException("Title and Text need to be filled!");
                }

                var isUpdated = _subjectRepository.EditSubject(subjectDTO);
                if (!isUpdated)
                {
                    throw new ArgumentException("Failed to update the subject.");
                }

                var updatedSubject = new Subject(subjectDTO);
                return updatedSubject;
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Cant edit new subject due to Database: " + dbex.Message, dbex);
            }
            catch (ArgumentException argex)
            {
                throw new SubjectError("Cant edit new subject due to Service: " + argex.Message, argex);
            }
        }

        public bool DeleteSubject(int subjectID) 
        {
            try
            {
                if (subjectID <= 0)
                {
                    throw new ArgumentException("Subject ID must be greater than 0.");
                }

                var subject = _subjectRepository.GetSubjectById(subjectID);
                if (subject == null)
                {
                    throw new ArgumentException("Subject doesn't exist.");
                }

                var isDeleted = _subjectRepository.DeleteSubject(subjectID);
                return isDeleted;
            }
            catch(DatabaseError dbex) 
	        {
                throw new DatabaseError("Couldnt delete Subject due to Database: " + dbex.Message, dbex);
	        }
            catch (ArgumentException argex)
            {
                throw new SubjectError("Couldnt delete Subject due to Service: " + argex.Message, argex);
            }
        }
    }    
}