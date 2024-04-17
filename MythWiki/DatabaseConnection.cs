using System;
using System.Data.SqlClient;

public class DatabaseConnection 
    {
        private string connectionString = 'Server=127.0.0.1;Database=DB IP;Uid=root;Pwd=;';

        public void DoSomethingWithData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM YourTable", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string data = reader.GetString(0);
                    Console.WriteLine(data);
                }

                reader.Close();
            }
        }        
    }

