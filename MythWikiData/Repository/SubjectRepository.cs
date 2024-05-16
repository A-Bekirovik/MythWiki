using System;
using MySql.Data.MySqlClient;
using MythWikiBusiness.IRepository;
using MythWikiData.DTO;

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
    }
}

