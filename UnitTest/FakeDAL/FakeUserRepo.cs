using System;
using MySql.Data.MySqlClient;
using MythWikiBusiness.DTO;
using MythWikiBusiness.ErrorHandling;
using MythWikiBusiness.IRepository;
using static System.Net.Mime.MediaTypeNames;

namespace UnitTest.FakeDAL
{
    public class FakeUserRepo : IUserRepo
    {
        List<UserDTO> users;

        public UserRepository()
        {
            users = new List<UserDTO>();

            UserDTO user = new UserDTO
            {
                UserID = 1,
                Name = "title",
                Password = "text",
                Email = "Boebeh@gmail.com"
            };

            UserDTO user1 = new UserDTO
            {
                UserID = 2,
                Name = "title",
                Password = "text",
                Email = "Boebeh1@gmail.com"
            };

            users.Add(user);
            users.Add(user1);
        }

        public List<UserDTO> GetAllUsers()
        {
            return users;
        }

        public UserDTO GetUserByUsername(string username)
        {
            UserDTO user = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Users WHERE Username = @Username";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserDTO
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Name = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                Email = reader["Email"].ToString()
                            };
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new DatabaseError("Database encountered an error while fetching the user.", ex);
            }

            return user;
        }

        public UserDTO GetUserById(int userId)
        {
            UserDTO user = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Users WHERE UserID = @UserID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserID", userId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserDTO
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Name = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                Email = reader["Email"].ToString()
                            };
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new DatabaseError("Database encountered an error while fetching the user.", ex);
            }
            return user;
        }

        public void AddUser(UserDTO userDTO)
        {
            UserDTO newUser = new UserDTO
            {
                UserID = 3,
                Name = "title",
                Password = "text",
                Email = "Boebeh2@gmail.com"
            };
        }
    }
}

