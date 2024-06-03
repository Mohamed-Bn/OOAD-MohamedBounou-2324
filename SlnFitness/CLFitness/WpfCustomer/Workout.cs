using CLFitness.Connection_data;
using CLFitness.WpfAdmin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CLFitness.WpfCustomer
{
    public class Workout
    {
        private static string connString= Connection.GetConnectionString();

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float? Distance { get; set; }
        public int CustomerId { get; set; }
        public int ExerciseId { get; set; }
        public Person Customer { get; set; }
        public Exercise Exercise { get; set; }

        public Workout()
        {

        }
        public static List<Workout> GetAllWorkouts()
        {
            List<Workout> workouts = new List<Workout>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM Workout", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Workout workout = new Workout
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("date")),
                                    Distance = reader.IsDBNull(reader.GetOrdinal("distance")) ? (float?)null : reader.GetFloat(reader.GetOrdinal("distance")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                    ExerciseId = reader.GetInt32(reader.GetOrdinal("exercise_id")),
                                    Exercise = Exercise.GetExerciseById(reader.GetInt32(reader.GetOrdinal("exercise_id")))
                                };


                                workouts.Add(workout);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return workouts;
        }

        public static List<Workout> GetPersonWorkout(int customerId)
        {
            List<Workout> workouts = new List<Workout>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM Workout WHERE customer_id = @CustomerId", connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Workout workout = new Workout
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("date")),
                                    Distance = reader.IsDBNull(reader.GetOrdinal("distance")) ? (float?)null : reader.GetFloat(reader.GetOrdinal("distance")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                    ExerciseId = reader.GetInt32(reader.GetOrdinal("exercise_id")),
                                    Exercise = Exercise.GetExerciseById(reader.GetInt32(reader.GetOrdinal("exercise_id")))
                                };


                                workouts.Add(workout);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return workouts;
        }

        public static void AddWorkout(Workout workout)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("INSERT INTO Workout (date, customer_id, exercise_id, distance) VALUES (@Date, @CustomerId, @ExerciseId, @Distance)", connection))
                    {
                        command.Parameters.AddWithValue("@Date", workout.Date);
                        command.Parameters.AddWithValue("@CustomerId", workout.CustomerId);
                        command.Parameters.AddWithValue("@ExerciseId", workout.ExerciseId);
                        command.Parameters.AddWithValue("@Distance", (object)workout.Distance ?? DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static List<Workout> GetPersonWorkoutsByDate(int customerId, DateTime date)
        {
            List<Workout> workouts = new List<Workout>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM Workout WHERE customer_id = @CustomerId AND CONVERT(date, date) = @Date", connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@Date", date.Date);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Workout workout = new Workout
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("date")),
                                    Distance = reader.IsDBNull(reader.GetOrdinal("distance")) ? (float?)null : reader.GetFloat(reader.GetOrdinal("distance")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                    ExerciseId = reader.GetInt32(reader.GetOrdinal("exercise_id")),
                                    Exercise = Exercise.GetExerciseById(reader.GetInt32(reader.GetOrdinal("exercise_id")))
                                };

                                workouts.Add(workout);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return workouts;
        }


        public static bool RemoveWorkout(Workout workout)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM Workout WHERE id = @WorkoutId", connection))
                    {
                        command.Parameters.AddWithValue("@WorkoutId", workout.Id);
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                   return false;
                }
            }

            return false;
        }
    }
}
