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
                throw new DatabaseError("Cant create new subject due to Database: " + dbex.Message, dbex);
            }
	        catch (ArgumentException argex) 
	        {
                throw new SubjectError("Cant create new subject due to Service: " + argex.Message, argex);
	        }		 
		}

        //ErrorHandling: Can't get an error if it chooses something from within the subjectlist. Cant add restrictions cause it just works.
        public Subject GetSubjectById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Subject ID must be greater than 0.");
            }

            try
            {
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
                throw new DatabaseError("Cant create new subject due to Database: " + dbex.Message, dbex);
            }
            catch (ArgumentException argex)
            {
                throw new SubjectError("Cant create new subject due to Service: " + argex.Message, argex);
            }

            return response;
        }


        // Added Errorhandling and restrictions
        public bool DeleteSubject(int subjectID) // vragen om het beter is om de return in de try te zetten, of erbuiten te zetten zoals hier.
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