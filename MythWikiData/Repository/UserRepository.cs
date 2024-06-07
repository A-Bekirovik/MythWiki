using System;
using MySql.Data.MySqlClient;
using MythWikiBusiness.DTO;
using MythWikiBusiness.ErrorHandling;
using MythWikiBusiness.IRepository;

namespace MythWikiData.Repository
{
	public class UserRepository : IUserRepo
	{
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<UserDTO> GetAllUsers()
        {
            List<UserDTO> users = new List<UserDTO>();
            try 
	        {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
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

        public UserDTO AddUser(string name, string password, string email)
        {
            UserDTO newUser = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Users (Username, Password, Email, CreatedDate) " +
                                   "VALUES (@Username, @Password, @Email, @CreatedDate)";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Username", name);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                    command.ExecuteNonQuery();

                    newUser = new UserDTO
                    {
                        Name = name,
                        Password = password,
                        Email = email
                    };
                }
            }
            catch (MySqlException ex)
            {
                throw new DatabaseError("Database encountered an error while adding the user.", ex);
            }

            return newUser;
        }
    }
}

