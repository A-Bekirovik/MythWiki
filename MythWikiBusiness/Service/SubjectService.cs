using System;
using System.Threading;
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

        //This is a normal connection to the database, Ask if errorhandling is needed
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

        // ErrorHandling: Restrictions on what are needed to Create subject, Created Errorhandling in case Restriction.
		public ServiceResponse CreateSubject(string title, string text, int editorid, string imagelink, string authorname)
        {
            ServiceResponse response = new ServiceResponse { Succes = false };

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
            catch (DatabaseError dbex)
            {
                response.ErrorMessage = dbex.Message;
            }
            return response;		 
		}

        //ErrorHandling: Can't get an error if it chooses something from within the subjectlist. Cant add restrictions cause it just works.
        public ServiceResponse GetSubjectById(int id)
        {
            var response = new ServiceResponse { Succes = false };

            if (id <= 0)
            {
                response.ErrorMessage = "Invalid subject ID.";
                return response;
            }

            try
            {
                var subjectDTO = _subjectRepository.GetSubjectById(id);

                if (subjectDTO != null)
                {
                    var foundSubject = new Subject(subjectDTO);
                    response.Succes = true;
                    response.ErrorMessage = "An error occurred while finding the subject.";
                    response.Data = foundSubject; // View requires a Subject, So i need temp data to transfer the subject.
                }
                else
                {
                    response.ErrorMessage = "Subject not found.";
                }
            }
            catch (DatabaseError dbex)
            {
                response.ErrorMessage = dbex.Message;
            }

            return response;
        }


        // Added Errorhandling and restrictions
        public ServiceResponse EditSubject(SubjectDTO subjectDTO)
        {
            var response = new ServiceResponse { Succes = false };

            if (subjectDTO == null || string.IsNullOrWhiteSpace(subjectDTO.Title) || string.IsNullOrWhiteSpace(subjectDTO.Text))
            {
                response.ErrorMessage = "Title and Text need to be filled!";
                return response;
            }

            try
            {
                var isUpdated = _subjectRepository.EditSubject(subjectDTO);
                if (isUpdated)
                {
                    var updatedSubject = new Subject(subjectDTO);
                    response.Succes = true;
                }
                else
                {
                    response.ErrorMessage = "Failed to update the subject.";
                }
            }
            catch (DatabaseError dbex)
            {
                response.ErrorMessage = dbex.Message;
            }

            return response;
        }


        // Added Errorhandling and restrictions
        public ServiceResponse DeleteSubject(int subjectID)
        {
            var response = new ServiceResponse { Succes = false };

            if (subjectID <= 0)
            {
                response.ErrorMessage = "Invalid subject ID.";
                return response;
            }

            try
            {
                var existingSubject = _subjectRepository.GetSubjectById(subjectID);
                if (existingSubject == null)
                {
                    response.ErrorMessage = "Subject doesn't exist.";
                    return response;
                }

                var isDeleted = _subjectRepository.DeleteSubject(subjectID);
                if (isDeleted)
                {
                    response.Succes = true;
                }
                else
                {
                    response.ErrorMessage = "Failed to delete the subject.";
                }
            }
            catch(DatabaseError dbex) 
	        {
                response.ErrorMessage = dbex.Message;
	        }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while deleting subject: {ex.Message}");
                response.ErrorMessage = "An error occurred while deleting the subject.";
            }

            return response;
        }
    }    
}