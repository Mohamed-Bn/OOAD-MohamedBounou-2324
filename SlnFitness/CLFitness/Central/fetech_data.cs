using CLFitness.WpfAdmin;
using CLFitness.WpfCustomer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLFitness.Central
{
    public class fetech_data
    {
        string connectionString = "your_connection_string";

        public List<Person> GetAllPersons()
        {
            List<Person> persons = new List<Person>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM PERSON";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Person person = new Person
                        {
                            Id = (int)reader["id"],
                            FirstName = (string)reader["firstname"],
                            LastName = (string)reader["lastname"],
                            Login = (string)reader["login"],
                            Password = (string)reader["password"],
                            ProfilePhoto = reader["profilephoto"] as byte[],
                            RegDate = (DateTime)reader["regdate"],
                            IsAdmin = (bool)reader["isadmin"]
                        };
                        persons.Add(person);
                    }
                }
            }

            return persons;
        }


        public void AddPerson(Person person)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO PERSON (firstname, lastname, login, password, profilephoto, regdate, isadmin) VALUES (@FirstName, @LastName, @Login, @Password, @ProfilePhoto, @RegDate, @IsAdmin)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@FirstName", person.FirstName);
                command.Parameters.AddWithValue("@LastName", person.LastName);
                command.Parameters.AddWithValue("@Login", person.Login);
                command.Parameters.AddWithValue("@Password", person.Password);
                command.Parameters.AddWithValue("@ProfilePhoto", (object)person.ProfilePhoto ?? DBNull.Value);
                command.Parameters.AddWithValue("@RegDate", person.RegDate);
                command.Parameters.AddWithValue("@IsAdmin", person.IsAdmin);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdatePerson(Person person)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE PERSON SET firstname = @FirstName, lastname = @LastName, login = @Login, password = @Password, profilephoto = @ProfilePhoto, regdate = @RegDate, isadmin = @IsAdmin WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", person.Id);
                command.Parameters.AddWithValue("@FirstName", person.FirstName);
                command.Parameters.AddWithValue("@LastName", person.LastName);
                command.Parameters.AddWithValue("@Login", person.Login);
                command.Parameters.AddWithValue("@Password", person.Password);
                command.Parameters.AddWithValue("@ProfilePhoto", (object)person.ProfilePhoto ?? DBNull.Value);
                command.Parameters.AddWithValue("@RegDate", person.RegDate);
                command.Parameters.AddWithValue("@IsAdmin", person.IsAdmin);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeletePerson(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM PERSON WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    


        public List<Exercise> GetAllExercises()
        {
            List<Exercise> exercises = new List<Exercise>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM EXERCISE";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Exercise exercise = new Exercise
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                            Description = (string)reader["description"],
                            Type = (ExerciseType)(int)reader["type"],
                            Instruction = reader["instruction"] as string,
                            BodyPart = reader["bodypart"] as string,
                            Pose = reader["pose"] as string,
                            Nickname = reader["nickname"] as string,
                            Photo = reader["photo"] as byte[],
                            Points = (int)reader["points"]
                        };
                        exercises.Add(exercise);
                    }
                }
            }

            return exercises;
        }

        public void AddExercise(Exercise exercise)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO EXERCISE (name, description, type, instruction, bodypart, pose, nickname, photo, points) VALUES (@Name, @Description, @Type, @Instruction, @BodyPart, @Pose, @Nickname, @Photo, @Points)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Name", exercise.Name);
                command.Parameters.AddWithValue("@Description", exercise.Description);
                command.Parameters.AddWithValue("@Type", (int)exercise.Type);
                command.Parameters.AddWithValue("@Instruction", (object)exercise.Instruction ?? DBNull.Value);
                command.Parameters.AddWithValue("@BodyPart", (object)exercise.BodyPart ?? DBNull.Value);
                command.Parameters.AddWithValue("@Pose", (object)exercise.Pose ?? DBNull.Value);
                command.Parameters.AddWithValue("@Nickname", (object)exercise.Nickname ?? DBNull.Value);
                command.Parameters.AddWithValue("@Photo", (object)exercise.Photo ?? DBNull.Value);
                command.Parameters.AddWithValue("@Points", exercise.Points);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateExercise(Exercise exercise)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE EXERCISE SET name = @Name, description = @Description, type = @Type, instruction = @Instruction, bodypart = @BodyPart, pose = @Pose, nickname = @Nickname, photo = @Photo, points = @Points WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", exercise.Id);
                command.Parameters.AddWithValue("@Name", exercise.Name);
                command.Parameters.AddWithValue("@Description", exercise.Description);
                command.Parameters.AddWithValue("@Type", (int)exercise.Type);
                command.Parameters.AddWithValue("@Instruction", (object)exercise.Instruction ?? DBNull.Value);
                command.Parameters.AddWithValue("@BodyPart", (object)exercise.BodyPart ?? DBNull.Value);
                command.Parameters.AddWithValue("@Pose", (object)exercise.Pose ?? DBNull.Value);
                command.Parameters.AddWithValue("@Nickname", (object)exercise.Nickname ?? DBNull.Value);
                command.Parameters.AddWithValue("@Photo", (object)exercise.Photo ?? DBNull.Value);
                command.Parameters.AddWithValue("@Points", exercise.Points);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteExercise(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM EXERCISE WHERE id = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }
}
