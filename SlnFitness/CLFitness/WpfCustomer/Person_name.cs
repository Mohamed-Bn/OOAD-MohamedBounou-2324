using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CLFitness.WpfCustomer
{
    public class Person_name
    {
        // connection string
        private static readonly string connString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=FitnessDB;Integrated Security=True";

        // Constructor en eigenschappen van de persoon.

        public Person_name()
        {
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Methode om alle personen op te halen.

        public static List<Person_name> GetAllPerson()
        {
            List<Person_name> people = new List<Person_name>();

            // Verbinding maken met de database en een SQL-commando uitvoeren.

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    // SQL-commando om alle personen te selecteren.

                    using (SqlCommand command = new SqlCommand("SELECT id, firstName, lastName FROM Person", connection))
                    {
                        // Lezen van de resultaten en toevoegen aan de lijst.

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
    // https://stackoverflow.com/questions/1345508/how-do-i-connect-to-a-sql-database-from-c
}
