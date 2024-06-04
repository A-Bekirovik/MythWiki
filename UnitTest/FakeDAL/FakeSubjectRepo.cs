using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MythWikiBusiness.DTO;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.ErrorHandling;

namespace UnitTest.FakeDAL
{
    public class FakeSubjectRepo : ISubjectRepo
    {
        List<SubjectDTO> subjects = new List<SubjectDTO>();

        public FakeSubjectRepo()
        {
        }

        // Get All Subjects
        public List<SubjectDTO> GetAllSubjects()
        {
            try
            {

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
                    SubjectID = 1,
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
            catch (Exception ex)
            {
                throw new DatabaseError("Database got an error", ex);
            }

            return subjects;
        }

        // Create Subject
        public SubjectDTO CreateSubject(string title, string text, int authorID, string imagelink)
        {

            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new DatabaseError("An error occurred while creating the subject.", ex);
            }
        }


        // Generate SubjectID
        private int GenerateNewSubjectID()
        {
            int newID = 1;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT MAX(SubjectID) FROM Subject";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        newID = Convert.ToInt32(result) + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new DatabaseError("Database got an error", ex);
            }
            return newID;
        }

        // Edit Subject
        public bool EditSubject(SubjectDTO subject)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Subject SET Title = @Title, Text = @Text, Image = @Image, EditorID = @EditorID, Date = @Date WHERE SubjectID = @SubjectID";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Title", subject.Title);
                    command.Parameters.AddWithValue("@Text", subject.Text);
                    command.Parameters.AddWithValue("@Image", string.IsNullOrEmpty(subject.Image) ? (object)DBNull.Value : subject.Image);
                    command.Parameters.AddWithValue("@EditorID", subject.EditorID);
                    command.Parameters.AddWithValue("@Date", DateTime.Now);
                    command.Parameters.AddWithValue("@SubjectID", subject.SubjectID);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new DatabaseError("Database got an error", ex);
            }
        }


        // Delete Subject
        public bool DeleteSubject(int subjectID)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string deleteSubjectUsersQuery = "DELETE FROM SubjectUsers WHERE SubjectID = @SubjectID";
                    MySqlCommand deleteSubjectUsersCommand = new MySqlCommand(deleteSubjectUsersQuery, connection);
                    deleteSubjectUsersCommand.Parameters.AddWithValue("@SubjectID", subjectID);
                    deleteSubjectUsersCommand.ExecuteNonQuery();

                    string deleteSubjectQuery = "DELETE FROM Subject WHERE SubjectID = @SubjectID";
                    MySqlCommand deleteSubjectCommand = new MySqlCommand(deleteSubjectQuery, connection);
                    deleteSubjectCommand.Parameters.AddWithValue("@SubjectID", subjectID);

                    int rowsAffected = deleteSubjectCommand.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new DatabaseError("Database got an error", ex);
            }
        }

        // Get Subject by ID
        public SubjectDTO GetSubjectById(int id)
        {
            SubjectDTO subject = null;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT s.SubjectID, s.Title, s.Text, s.Image, s.EditorID, u.Username AS EditorName, s.Date
                FROM Subject s
                INNER JOIN Users u ON s.EditorID = u.UserID
                WHERE s.SubjectID = @SubjectID";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SubjectID", id);

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        subject = new SubjectDTO
                        {
                            SubjectID = Convert.ToInt32(reader["SubjectID"]),
                            Title = reader["Title"].ToString(),
                            Text = reader["Text"].ToString(),
                            Image = reader["Image"].ToString(),
                            EditorID = Convert.ToInt32(reader["EditorID"]),
                            EditorName = reader["EditorName"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"])
                        };
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new DatabaseError("Database got an error", ex);
            }
            return subject;
        }
    }
}
