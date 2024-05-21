using System;
using MySql.Data.MySqlClient;
using MythWikiBusiness.DTO;
using MythWikiBusiness.IRepository;

namespace MythWikiData.Repository
{
	public class SubjectRepository : ISubjectRepo
	{
        private string connectionString = "server=localhost;uid=root;pwd=;database=MythWikiDB";

        public List<SubjectDTO> GetAllSubjects()
        {
            List<SubjectDTO> subjects = new List<SubjectDTO>();

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
            return subjects;
        }

        //Create Subject
        public SubjectDTO CreateSubject(string title, string text, int editorid, string imagelink, string authorname, DateTime date)
        {
            Console.WriteLine($"Repository - Title: {title}");
            Console.WriteLine($"Repository - Text: {text}");
            Console.WriteLine($"Repository - EditorID: {editorid}");
            Console.WriteLine($"Repository - Image Link: {imagelink}");
            Console.WriteLine($"Repository - Author Name: {authorname}");
            Console.WriteLine($"Repository - Date: {date}");

            if (string.IsNullOrEmpty(title)) throw new ArgumentException("Title cannot be null or empty");
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Text cannot be null or empty");         

            SubjectDTO newSubject = new SubjectDTO();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                newSubject.SubjectID = GenerateNewSubjectID();
                newSubject.EditorID = editorid;
                newSubject.Title = title;
                newSubject.Text = text;
                newSubject.Image = imagelink;
                newSubject.Author = authorname;
                newSubject.Date = date;

                string query = "INSERT INTO Subject (SubjectID, Title, Text, EditorID, Image, Author, Date) " +
                               "VALUES (@SubjectID, @Title, @Text, @EditorID, @Image, @Author, @Date)";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@SubjectID", newSubject.SubjectID);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Text", text);
                command.Parameters.AddWithValue("@EditorID", editorid);
                command.Parameters.AddWithValue("@Image", string.IsNullOrEmpty(imagelink) ? (object)DBNull.Value : imagelink);
                command.Parameters.AddWithValue("@Author", authorname);
                command.Parameters.AddWithValue("@Date", date);

                command.ExecuteNonQuery();
            }

            return newSubject;
        }

        // Generate SubjectID
        private int GenerateNewSubjectID()
        {
            int newID = 1; 

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

            return newID;
        }

        //Delete Subject
        public bool DeleteSubject(int subjectID)
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
    }
}

