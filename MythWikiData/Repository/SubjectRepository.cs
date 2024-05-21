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
        public SubjectDTO CreateSubject(string title, string text) 
	    {
            SubjectDTO newsubject = new SubjectDTO();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Subject (Title, Text) VALUES (@Title, @Text)";
                MySqlCommand command = new MySqlCommand(query, connection);


                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Text", text);


                command.ExecuteNonQuery();


                newsubject.SubjectID = (int)command.LastInsertedId;
                newsubject.Title = title;
                newsubject.Text = text;

                return newsubject;
            }
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

