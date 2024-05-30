using System;
using System.Collections.Generic;
using MythWikiBusiness.DTO;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.Models;
using MythWikiBusiness.ErrorHandling;

namespace MythWikiBusiness.Services
{
    public class SubjectService
    {
        private readonly ISubjectRepo _subjectRepository;

        public SubjectService(ISubjectRepo subjectrepo)
        {
            _subjectRepository = subjectrepo;
        }

        // Retrieve all subjects
        public List<Subject> GetAllSubjects()
        {
            List<SubjectDTO> subjectDTOs;
            List<Subject> subjects = new List<Subject>();

            try
            {
                subjectDTOs = _subjectRepository.GetAllSubjects();
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Can't get all subjects due to database error", dbex);
            }

            foreach (var dto in subjectDTOs)
            {
                subjects.Add(new Subject(dto));
            }

            return subjects;
        }

        // Create a new subject
        public Subject CreateSubject(string title, string text, int authorID, string imagelink)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(text))
            {
                throw new SubjectError("Can't create subject: Title and Text are required");
            }

            SubjectDTO newSubjectDTO;

            try
            {
                newSubjectDTO = _subjectRepository.CreateSubject(title, text, authorID, imagelink);
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Can't create new subject due to database error: " + dbex.Message, dbex);
            }

            return new Subject(newSubjectDTO);
        }

        // Retrieve a subject by ID
        public Subject GetSubjectById(int id)
        {
            if (id <= 0)
            {
                throw new SubjectError("Can't get subject: Subject ID must be greater than 0");
            }

            SubjectDTO subjectDTO;

            try
            {
                subjectDTO = _subjectRepository.GetSubjectById(id);
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Can't get subject due to database error", dbex);
            }

            if (subjectDTO == null)
            {
                throw new SubjectError("Can't get subject: Subject not found");
            }

            return new Subject(subjectDTO);
        }

        // Edit an existing subject
        public Subject EditSubject(SubjectDTO subjectDTO)
        {
            if (subjectDTO == null || string.IsNullOrWhiteSpace(subjectDTO.Title) || string.IsNullOrWhiteSpace(subjectDTO.Text))
            {
                throw new SubjectError("Can't edit subject: Title and Text are required");
            }

            bool isUpdated;

            try
            {
                isUpdated = _subjectRepository.EditSubject(subjectDTO);
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Can't edit subject due to database error: " + dbex.Message, dbex);
            }

            if (!isUpdated)
            {
                throw new SubjectError("Can't edit subject: Failed to update the subject");
            }

            return new Subject(subjectDTO);
        }

        // Delete a subject
        public bool DeleteSubject(int subjectID)
        {
            if (subjectID <= 0)
            {
                throw new SubjectError("Can't delete subject: Subject ID must be greater than 0");
            }

            SubjectDTO subject;

            try
            {
                subject = _subjectRepository.GetSubjectById(subjectID);
            }
            catch (DatabaseError dbex)
            {
                throw new DatabaseError("Can't delete subject due to database error", dbex);
            }

            if (subject == null)
            {
                throw new SubjectError("Can't delete subject: Subject doesn't exist");
            }

            return _subjectRepository.DeleteSubject(subjectID);
        }
    }
}