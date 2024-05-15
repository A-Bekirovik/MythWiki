using System;
using MySql.Data.MySqlClient;

namespace MythWikiDAL.Repository
{
	public class SubjectRepository
	{
        private string connectionString = "Server=127.0.0.1;Database=DB IP;Uid=root;Pwd=;";

        public void GetSubjects()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SELECT * FROM Subject", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string data = reader.GetString(0);
                    Console.WriteLine(data);
                    Console.WriteLine(reader["SubjectID"]);
                }

                reader.Close();
            }
        }

        public void GetAllSubjects()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SELECT * FROM Subject", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string data = reader.GetString(0);
                    Console.WriteLine(data);
                    Console.WriteLine(reader["UserID"]);
                }

                reader.Close();
            }
        }
    }
}

