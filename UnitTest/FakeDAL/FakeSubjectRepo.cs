using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MythWikiBusiness.DTO;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.ErrorHandling;
using MythWikiBusiness.Models;

namespace UnitTest.FakeDAL
{
    public class FakeSubjectRepo : ISubjectRepo
    {
        List<SubjectDTO> subjects;

        public FakeSubjectRepo()
        {
            subjects = new List<SubjectDTO>();

            SubjectDTO subject = new SubjectDTO
            {
                SubjectID = 1,
                Title = "title",
                Text = "text",
                EditorID = 1,
                Image = "",
                Date = DateTime.Now,
                EditorName = "Boebeh"
            };

            SubjectDTO subject1 = new SubjectDTO
            {
                SubjectID = 2,
                Title = "title",
                Text = "text",
                EditorID = 1,
                Image = "",
                Date = DateTime.Now,
                EditorName = "Boebeh"
            };

            subjects.Add(subject);
            subjects.Add(subject1);
        }

        // Get All Subjects
        public List<SubjectDTO> GetAllSubjects()
        {
            return subjects;
        }

        // Create Subject
        public SubjectDTO CreateSubject(string title, string text, int authorID, string imagelink)
        {
            SubjectDTO newSubject = new SubjectDTO()
            {
                Title = title,
                Text = text,
                AuthorID = authorID,
                Image = imagelink
            };

            return newSubject;
        }


        // Generate SubjectID
        public int GenerateNewSubjectID()
        {
            if (subjects.Count == 0)
                return 1;
            else
                return subjects.Max(s => s.SubjectID) + 1;
        }

        // Edit Subject
        public bool EditSubject(SubjectDTO subject)
        {
            var existingSubject = subjects.FirstOrDefault(s => s.SubjectID == subject.SubjectID);
            if (existingSubject != null)
            {
                existingSubject.Title = subject.Title;
                existingSubject.Text = subject.Text;
                existingSubject.Image = subject.Image;
                existingSubject.EditorID = subject.EditorID;
                existingSubject.Date = subject.Date;
                return true;
            }
            return false;
        }


        // Delete Subject
        public bool DeleteSubject(int subjectID)
        {
            var subject = subjects.FirstOrDefault(s => s.SubjectID == subjectID);
            if (subject != null)
            {
                subjects.Remove(subject);
                return true;
            }
            return false;
        }

        // Get Subject By ID
        public SubjectDTO GetSubjectById(int id)
        {
            var subject = subjects.FirstOrDefault(s => s.SubjectID == id);
            return subject;
        }
    }
}
