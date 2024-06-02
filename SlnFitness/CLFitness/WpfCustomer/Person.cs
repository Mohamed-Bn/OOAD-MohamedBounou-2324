using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace CLFitness.WpfCustomer
{
    public class Person
    {
        private static string connString = "Data Source=DESKTOP-OU69G6L\\SQLEXPRESS;Initial Catalog=FitnessDB;Integrated Security=True;\r\n\t\t\t Encrypt=False";

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public DateTime RegDate { get; set; }
        public bool IsAdmin { get; set; }



        public static List<Person> GetAllPerson()
        {
            List<Person> people = new List<Person>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM Person", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Person person = new Person
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("lastName")),
                                    Login = reader.GetString(reader.GetOrdinal("login")),
                                    Password = reader.GetString(reader.GetOrdinal("password")),
                                    ProfilePhoto = reader["profilephoto"] as byte[],
                                    RegDate = reader.GetDateTime(reader.GetOrdinal("regdate")),
                                    IsAdmin = reader.GetByte(reader.GetOrdinal("isadmin")) == 1
                                };

                                people.Add(person);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }



            return people;
        }

        public static Person GetPerson(int id_)
        {
            Person person = null;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    // Use parameterized query to prevent SQL injection
                    string query = "SELECT * FROM Person WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add the id parameter
                        command.Parameters.AddWithValue("@id", id_);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                person = new Person
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("lastName")),
                                    Login = reader.GetString(reader.GetOrdinal("login")),
                                    Password = reader.GetString(reader.GetOrdinal("password")),
                                    ProfilePhoto = reader["profilephoto"] as byte[],
                                    RegDate = reader.GetDateTime(reader.GetOrdinal("regdate")),
                                    IsAdmin = reader.GetByte(reader.GetOrdinal("isadmin")) == 1
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return person;
        }


        public static string InsertPersonIntoDatabase(Person person)
        {
            try
            {
                person.RegDate = DateTime.Now;
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    string query = "INSERT INTO Person (firstname, lastname, login, password, isadmin, profilephoto, regdate) " +
                                   "VALUES (@FirstName, @LastName, @Login, @Password, @IsAdmin, @ProfilePhoto, @RegDate)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", person.FirstName);
                        command.Parameters.AddWithValue("@LastName", person.LastName);
                        command.Parameters.AddWithValue("@Login", person.Login);
                        command.Parameters.AddWithValue("@Password", person.Password);
                        command.Parameters.AddWithValue("@IsAdmin", person.IsAdmin);
                        command.Parameters.AddWithValue("@ProfilePhoto", person.ProfilePhoto);
                        command.Parameters.AddWithValue("@RegDate", person.RegDate);
                        command.ExecuteNonQuery();
                    }
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string UpdatePersonInDatabase(Person person)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    string query = "UPDATE Person SET firstname = @FirstName, lastname = @LastName, " +
                                   "login = @Login, password = @Password, isadmin = @IsAdmin, profilephoto = @ProfilePhoto " +
                                   "WHERE id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", person.Id);
                        command.Parameters.AddWithValue("@FirstName", person.FirstName);
                        command.Parameters.AddWithValue("@LastName", person.LastName);
                        command.Parameters.AddWithValue("@Login", person.Login);
                        command.Parameters.AddWithValue("@Password", person.Password);
                        command.Parameters.AddWithValue("@IsAdmin", person.IsAdmin);
                        command.Parameters.AddWithValue("@ProfilePhoto", person.ProfilePhoto);
                        command.ExecuteNonQuery();
                    }
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static string DeleteWorkoutsForPerson(int personId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    string query = "DELETE FROM Workout WHERE customer_id = @PersonId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonId", personId);
                        command.ExecuteNonQuery();
                    }
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string DeletePerson(Person person)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    string query = "DELETE FROM Person WHERE id = @PersonId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonId", person.Id);
                        command.ExecuteNonQuery();
                    }
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
