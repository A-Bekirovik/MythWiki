using System;
using MySql.Data.MySqlClient;
using MythWikiDAL.DTO;

namespace MythWikiDAL.Repository
{
	public class UserRepository
	{


        private string connectionString = "Server=127.0.0.1;Database=DB IP;Uid=root;Pwd=;";

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

