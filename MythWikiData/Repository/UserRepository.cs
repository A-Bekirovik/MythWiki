using System;
using MySql.Data.MySqlClient;
using MythWikiBusiness.DTO;
using MythWikiBusiness.ErrorHandling;
using MythWikiBusiness.IRepository;

namespace MythWikiData.Repository
{
	public class UserRepository : IUserRepo
	{
        private string connectionString = "server=localhost;uid=root;pwd=;database=MythWikiDB";

        public List<UserDTO> GetAllUsers()
        {
            List<UserDTO> users = new List<UserDTO>();
            try 
	        {
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
            }
            catch (MySqlException ex)
            {
                throw new DatabaseError("Database got an error", ex);
            }
            return users;
        }
    }
}

