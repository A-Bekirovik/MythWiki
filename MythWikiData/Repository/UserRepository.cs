using System;
using MySql.Data.MySqlClient;
using MythWikiData.DTO;

namespace MythWikiData.Repository
{
	public class UserRepository
	{

        public string connectionString = "server=localhost;uid=root;pwd=;database=MythWikiDB";

        public UserRepository()
		{
		}

        public List<UserDTO> GetAllUsers()
        {
            List<UserDTO> users = new List<UserDTO>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SELECT * FROM User", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UserDTO user = new UserDTO
                    {
                        Name = reader["Name"].ToString(),
                        UserID = Convert.ToInt32(reader["UserID"])
                    };
                    users.Add(user);
                }
                reader.Close();
            }
            return users;
        }
    }
}

