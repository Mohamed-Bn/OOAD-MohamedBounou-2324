using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CLFitness.WpfCustomer
{
    public class Person_name
    {
        // Utilisez la chaîne de connexion correcte ici
        private static readonly string connString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=FitnessDB;Integrated Security=True";

        public Person_name()
        {
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static List<Person_name> GetAllPerson()
        {
            List<Person_name> people = new List<Person_name>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT id, firstName, lastName FROM Person", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Person_name person = new Person_name
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("lastName")),
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
    }
}
