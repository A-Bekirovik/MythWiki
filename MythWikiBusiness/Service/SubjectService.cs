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

        // ErrorHandling: Restrictions on what are needed to Create subject, Created Errorhandling in case Restriction.
		public Subject CreateSubject(string title, string text, int editorid, string imagelink, string authorname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(text))
                {
                    throw new ArgumentException("Title and Text need to be Filled!");
                }

                SubjectDTO newsubjectDTO = _subjectRepository.CreateSubject(title, text, editorid, imagelink, authorname);
                Subject newsubject = new Subject(newsubjectDTO);
                return newsubject;
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
                throw new DatabaseError("Cant create new subject due to Database", dbex);
            }
            catch (ArgumentException argex)
            {
                throw new SubjectError("Cant create new subject due to Service", argex);
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
                throw new DatabaseError("Cant create new subject due to Database", dbex);
            }
            catch (ArgumentException argex)
            {
                throw new SubjectError("Cant create new subject due to Service", argex);
            }

            return response;
        }


        // Added Errorhandling and restrictions
        public bool DeleteSubject(int subjectID) // vragen om het beter is om de return in de try te zetten, of erbuiten te zetten zoals hier.
        {
            var isDeleted = _subjectRepository.DeleteSubject(subjectID);

            try
            {
                if (subjectID <= 0)
                {
                    throw new ArgumentException("Subject is null or 0");
                }

                var subjectid = _subjectRepository.GetSubjectById(subjectID);
                if (subjectid == null)
                {
                    throw new ArgumentException("Subject doesn't exist.");
                }
            }
            catch(DatabaseError dbex) 
	        {
                throw new DatabaseError("Couldnt delete Subject due to Database", dbex);
	        }
            catch (ArgumentException argex)
            {
                throw new SubjectError("Couldnt delete Subject due to Service", argex);
            }

            return isDeleted;
        }
    }    
}