using CLFitness.Connection_data;
using CLFitness.WpfCustomer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CLFitness.WpfAdmin
{
    public class Exercise
    {
        private static string connString= Connection.GetConnectionString();

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeNum { get; set; }
        public ExerciseType Type { get; set; }
        public string Instruction { get; set; }
        public string BodyPart { get; set; } 
        public string Pose { get; set; } 
        public string Nickname { get; set; }
        public byte[] Photo { get; set; }
        public int Points { get; set; }

        public Exercise()
        {
        }


        public static List<Exercise> GetAllExercises()
        {
            List<Exercise> exercises = new List<Exercise>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM Exercise";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                Exercise exercise = new Exercise
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    Description = reader.GetString(reader.GetOrdinal("description")),
                                    TypeNum = reader.GetInt32(reader.GetOrdinal("type")),
                                    Instruction = reader.IsDBNull(reader.GetOrdinal("instruction")) ? null : reader.GetString(reader.GetOrdinal("instruction")),
                                    BodyPart = reader.IsDBNull(reader.GetOrdinal("bodypart")) ? null : reader.GetString(reader.GetOrdinal("bodypart")),
                                    Pose = reader.IsDBNull(reader.GetOrdinal("pose")) ? null : reader.GetString(reader.GetOrdinal("pose")),
                                    Nickname = reader.IsDBNull(reader.GetOrdinal("nickname")) ? null : reader.GetString(reader.GetOrdinal("nickname")),
                                    Photo = reader["photo"] as byte[],
                                    Points = reader.GetInt32(reader.GetOrdinal("points"))
                                };

                                exercises.Add(exercise);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return exercises;
        }


        public static Exercise GetExerciseById(int exerciseId)
        {
            Exercise exercise = null;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM Exercise WHERE Id = @ExerciseId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ExerciseId", exerciseId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                exercise = new Exercise
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    Description = reader.GetString(reader.GetOrdinal("description")),
                                    TypeNum = reader.GetInt32(reader.GetOrdinal("type")),
                                    Instruction = reader.IsDBNull(reader.GetOrdinal("instruction")) ? null : reader.GetString(reader.GetOrdinal("instruction")),
                                    BodyPart = reader.IsDBNull(reader.GetOrdinal("bodypart")) ? null : reader.GetString(reader.GetOrdinal("bodypart")),
                                    Pose = reader.IsDBNull(reader.GetOrdinal("pose")) ? null : reader.GetString(reader.GetOrdinal("pose")),
                                    Nickname = reader.IsDBNull(reader.GetOrdinal("nickname")) ? null : reader.GetString(reader.GetOrdinal("nickname")),
                                    Photo = reader["photo"] as byte[],
                                    Points = reader.GetInt32(reader.GetOrdinal("points"))
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

            return exercise;
        }


        public static List<Exercise> GetExercise(int type)
        {
            List<Exercise> exercises = new List<Exercise>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM Exercise WHERE type=@type", connection))
                    {
                        command.Parameters.AddWithValue("@type", type);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Exercise exercise = null;
                                ExerciseType exerciseType = (ExerciseType)Enum.Parse(typeof(ExerciseType), reader["Type"].ToString());

                                switch (exerciseType)
                                {
                                    case ExerciseType.Cardio:
                                        exercise = new CardioExercise();
                                        break;
                                    case ExerciseType.Dumbbell:
                                        exercise = new DumbbellExercise();
                                        break;
                                    case ExerciseType.Yoga:
                                        exercise = new YogaExercise();
                                        break;
                                }

                                if (exercise != null)
                                {
                                    exercise.Id = Convert.ToInt32(reader["Id"]);
                                    exercise.Name = reader["Name"].ToString();
                                    exercise.Description = reader["Description"].ToString();
                                    exercise.Type = exerciseType;
                                    exercise.Photo = reader["Photo"] as byte[];
                                    exercise.Points = Convert.ToInt32(reader["Points"]);

                                    if (exercise is DumbbellExercise dumbbell)
                                    {
                                        dumbbell.Instruction = reader["Instruction"] as string;
                                        dumbbell.BodyPart = reader["BodyPart"] as string;
                                    }
                                    else if (exercise is YogaExercise yoga)
                                    {
                                        yoga.Instruction = reader["Instruction"] as string;
                                        yoga.Pose = reader["Pose"] as string;
                                        yoga.Nickname = reader["Nickname"] as string;
                                    }

                                    exercises.Add(exercise);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            return exercises;
        }



        public string Save()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("UPDATE Exercise SET Name = @Name, Description = @Description, Type = @Type, Instruction = @Instruction, BodyPart = @BodyPart, Pose = @Pose, Nickname = @Nickname, Photo = @Photo, Points = @Points WHERE Id = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Name", Name);
                        command.Parameters.AddWithValue("@Description", Description);
                        command.Parameters.AddWithValue("@Type", (int)Type);
                        command.Parameters.AddWithValue("@Instruction", (object)GetInstruction() ?? DBNull.Value);
                        command.Parameters.AddWithValue("@BodyPart", (object)GetBodyPart() ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Pose", (object)GetPose() ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Nickname", (object)GetNickname() ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Photo", Photo);
                        command.Parameters.AddWithValue("@Points", Points);
                        command.Parameters.AddWithValue("@Id", Id);

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


        public string SaveNew()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("INSERT INTO Exercise (Name, Description, Type, Instruction, BodyPart, Pose, Nickname, Photo, Points) VALUES (@Name, @Description, @Type, @Instruction, @BodyPart, @Pose, @Nickname, @Photo, @Points)", connection))
                    {
                        command.Parameters.AddWithValue("@Name", Name);
                        command.Parameters.AddWithValue("@Description", Description);
                        command.Parameters.AddWithValue("@Type", (int)Type);

                        command.Parameters.AddWithValue("@Instruction", (object)GetInstruction() ?? DBNull.Value);
                        command.Parameters.AddWithValue("@BodyPart", (object)GetBodyPart() ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Pose", (object)GetPose() ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Nickname", (object)GetNickname() ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Photo", Photo);
                        command.Parameters.AddWithValue("@Points", Points);

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

        public string Delete()
        {
            try
            {
                DeleteAssociatedWorkouts();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM Exercise WHERE Id = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return "true"; 
                        }
                        else
                        {
                            return "Exercise not found"; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        private void DeleteAssociatedWorkouts()
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Workout WHERE exercise_id = @ExerciseId", connection))
                {
                    command.Parameters.AddWithValue("@ExerciseId", Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        protected virtual string GetInstruction() => null;
        protected virtual string GetBodyPart() => null;
        protected virtual string GetPose() => null;
        protected virtual string GetNickname() => null;
    }



    public enum ExerciseType
    {
        Cardio = 1,
        Dumbbell = 2,
        Yoga = 3
    }
}
