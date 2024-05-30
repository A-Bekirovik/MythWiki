using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MythWikiBusiness.DTO;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.ErrorHandling;

namespace MythWikiData.Repository
{
    public class SubjectRepository : ISubjectRepo
    {
        private readonly string _connectionString;

        public SubjectRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Get All Subjects
        // Get All Subjects
        public List<SubjectDTO> GetAllSubjects()
        {
            List<SubjectDTO> subjects = new List<SubjectDTO>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // Select the most recent editor for each subject
                    string query = @"
                SELECT s.SubjectID, s.Title, s.Text, s.Image, s.EditorID AS UserID, s.Date, u.Username as EditorName
                FROM Subject s
                INNER JOIN Users u ON s.EditorID = u.UserID";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SubjectDTO subject = new SubjectDTO
                        {
                            SubjectID = Convert.ToInt32(reader["SubjectID"]),
                            Title = reader["Title"].ToString(),
                            Text = reader["Text"].ToString(),
                            EditorID = Convert.ToInt32(reader["UserID"]),
                            Image = reader["Image"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            EditorName = reader["EditorName"].ToString()
                        };
                        subjects.Add(subject);
                    }
                    reader.Close();
                }
            }
            catch (MySqlException ex)
            {
                throw new DatabaseError("Database got an error", ex);
            }

            return subjects;
        }

        // Create Subject
        public SubjectDTO CreateSubject(string title, string text, int authorID, string imagelink)
        {
            SubjectDTO newSubject = new SubjectDTO();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                MySqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    // Check if AuthorID exists in Users table
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE UserID = @AuthorID";
                    MySqlCommand checkUserCommand = new MySqlCommand(checkUserQuery, connection, transaction);
                    checkUserCommand.Parameters.AddWithValue("@AuthorID", authorID);
                    int userExists = Convert.ToInt32(checkUserCommand.ExecuteScalar());

                    if (userExists == 0)
                    {
                        throw new Exception("AuthorID does not exist in Users table.");
                    }

                    newSubject.SubjectID = GenerateNewSubjectID();
                    newSubject.AuthorID = authorID;  
                    newSubject.EditorID = authorID;  
                    newSubject.Title = title;
                    newSubject.Text = text;
                    newSubject.Image = imagelink;
                    newSubject.Date = DateTime.Now;

                    // Insert into Subject table
                    string insertSubjectQuery = @"
                INSERT INTO Subject (SubjectID, Title, Text, Image, EditorID, Date)
                VALUES (@SubjectID, @Title, @Text, @Image, @AuthorID, @Date)";

                    MySqlCommand insertSubjectCommand = new MySqlCommand(insertSubjectQuery, connection, transaction);
                    insertSubjectCommand.Parameters.AddWithValue("@SubjectID", newSubject.SubjectID);
                    insertSubjectCommand.Parameters.AddWithValue("@Title", title);
                    insertSubjectCommand.Parameters.AddWithValue("@Text", text);
                    insertSubjectCommand.Parameters.AddWithValue("@Image", string.IsNullOrEmpty(imagelink) ? (object)DBNull.Value : imagelink);
                    insertSubjectCommand.Parameters.AddWithValue("@AuthorID", authorID);
                    insertSubjectCommand.Parameters.AddWithValue("@EditorID", authorID);  // Initial editor is the author
                    insertSubjectCommand.Parameters.AddWithValue("@Date", newSubject.Date);

                    insertSubjectCommand.ExecuteNonQuery();

                    // Insert into SubjectUsers table
                    string insertSubjectUsersQuery = @"
                INSERT INTO SubjectUsers (UserID, SubjectID)
                VALUES (@UserID, @SubjectID)";

                    MySqlCommand insertSubjectUsersCommand = new MySqlCommand(insertSubjectUsersQuery, connection, transaction);
                    insertSubjectUsersCommand.Parameters.AddWithValue("@UserID", authorID);
                    insertSubjectUsersCommand.Parameters.AddWithValue("@SubjectID", newSubject.SubjectID);

                    int rowsAffected = insertSubjectUsersCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Insert into SubjectUsers table failed.");
                    }

                    transaction.Commit();
                }
                catch (MySqlException ex)
                {
                    transaction?.Rollback();
                    Console.WriteLine(ex.Message);
                    throw new DatabaseError("There's something wrong with the Database.: " + ex.Message, ex);
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    Console.WriteLine(ex.Message);
                    throw new DatabaseError("An error occurred while creating the subject.", ex);
                }
            }
            return newSubject;
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
            catch (MySqlException ex)
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
            catch (MySqlException ex)
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
            catch (MySqlException ex)
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
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw new DatabaseError("Database got an error", ex);
            }
            return subject;
        }
    }
}
