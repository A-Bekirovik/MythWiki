using System;
using MySql.Data.MySqlClient;
using MythWikiBusiness.DTO;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.ErrorHandling;

namespace MythWikiData.Repository
{
	public class SubjectRepository : ISubjectRepo
	{
        private string connectionString = "server=localhost;uid=root;pwd=;database=MythWikiDB";

        //Get All Subjects
        public List<SubjectDTO> GetAllSubjects()
        {
            List<SubjectDTO> subjects = new List<SubjectDTO>();

            try 
	        {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand("SELECT * FROM Subject", connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SubjectDTO subject = new SubjectDTO
                        {
                            SubjectID = Convert.ToInt16(reader["SubjectID"]),
                            Title = reader["Title"].ToString(),
                            Text = reader["Text"].ToString()
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

        //Create Subject
        
        public SubjectDTO CreateSubject(string title, string text, int editorid, string imagelink, string authorname)
        {
            SubjectDTO newSubject = new SubjectDTO();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    newSubject.SubjectID = GenerateNewSubjectID();
                    newSubject.EditorID = editorid;
                    newSubject.Title = title;
                    newSubject.Text = text;
                    newSubject.Image = imagelink;
                    newSubject.Author = authorname;
                    newSubject.Date = DateTime.Now;

                    string query = "INSERT INTO Subject (SubjectID, Title, Text, EditorID, Image, Author, Date) " +
                                   "VALUES (@SubjectID, @Title, @Text, @EditorID, @Image, @Author, @Date)";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@SubjectID", newSubject.SubjectID);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Text", text);
                    command.Parameters.AddWithValue("@EditorID", editorid);
                    command.Parameters.AddWithValue("@Image", string.IsNullOrEmpty(imagelink) ? (object)DBNull.Value : imagelink);
                    command.Parameters.AddWithValue("@Author", authorname);
                    command.Parameters.AddWithValue("@Date", newSubject.Date);

                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw new DatabaseError("There's something wrong with the Database.", ex);
            }
            return newSubject;
        }

        // Generate SubjectID
        private int GenerateNewSubjectID()
        {
            int newID = 1; 

            try 
	        {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
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

        //Edit Subject
        public bool EditSubject(SubjectDTO subject)
        {
            try 
	        {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Subject SET Title = @Title, Text = @Text, EditorID = @EditorID, Image = @Image, Author = @Author, Date = @Date WHERE SubjectID = @SubjectID";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Title", subject.Title);
                    command.Parameters.AddWithValue("@Text", subject.Text);
                    command.Parameters.AddWithValue("@EditorID", subject.EditorID);
                    command.Parameters.AddWithValue("@Image", string.IsNullOrEmpty(subject.Image) ? (object)DBNull.Value : subject.Image);
                    command.Parameters.AddWithValue("@Author", subject.Author);
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

        //Delete Subject
        public bool DeleteSubject(int subjectID)
        {
            try 
	        {
                bool isDeleted = false;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Subject WHERE SubjectID = @SubjectID";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@SubjectID", subjectID);

                    int rowsAffected = command.ExecuteNonQuery();

                    isDeleted = rowsAffected > 0;
                }
                return isDeleted;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw new DatabaseError("Database got an error", ex);
            }            
        }

        //Get subject by id
        public SubjectDTO GetSubjectById(int id)
        {
            SubjectDTO subject = null;
            try 
	        {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Subject WHERE SubjectID = @SubjectID";
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
                            EditorID = Convert.ToInt32(reader["EditorID"]),
                            Image = reader["Image"].ToString(),
                            Author = reader["Author"].ToString(),
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