using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CLActiBuddy
{
    public class Persoon
    {
        public int Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Login { get; set; }
        public string Paswoord { get; set; }
        public byte[] Profielfoto { get; set; }
        public DateTime RegDatum { get; set; }
        public bool IsAdmin { get; set; }
        public override string ToString()
        {
            return $"{Id}. {Voornaam} {Achternaam}";
        }

        private static string connString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        // wordt gebruikt in Activiteit en Deelname klasses
        public static Persoon GetById(int organisatorId)
        {
            using SqlConnection conn = new (connString);
            conn.Open();

            string sql = "SELECT * FROM Persoon WHERE id = @organisatorId";

            using SqlCommand comm = new (sql, conn);
            comm.Parameters.AddWithValue("@organisatorId", organisatorId);

            using SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                return new Persoon()
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Voornaam = Convert.ToString(reader["voornaam"]),
                    Achternaam = Convert.ToString(reader["achternaam"]),
                    Login = Convert.ToString(reader["login"]),
                    Paswoord = Convert.ToString(reader["paswoord"]),
                    Profielfoto = reader["profielfoto"] == DBNull.Value ? null : (byte[])reader["profielfoto"],
                    RegDatum = Convert.ToDateTime(reader["regdatum"]),
                    IsAdmin = Convert.ToBoolean(reader["isadmin"])
                };
            }
            else
            {
                return null;
            }
        }

        // voor login
        public static Persoon GetByGebruikersnaamEnPaswoord(string gebruikersnaam, string paswoord)
        {
            using SqlConnection conn = new (connString);
            conn.Open();

            string sql = "SELECT * FROM Persoon WHERE login = @parGebruikersnaam AND paswoord = @parPaswoord";

            using SqlCommand comm = new (sql, conn);
            comm.Parameters.AddWithValue("@parGebruikersnaam", gebruikersnaam);
            comm.Parameters.AddWithValue("@parPaswoord", paswoord);

            using SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                return new Persoon()
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Voornaam = Convert.ToString(reader["voornaam"]),
                    Achternaam = Convert.ToString(reader["achternaam"]),
                    Login = Convert.ToString(reader["login"]),
                    Paswoord = Convert.ToString(reader["paswoord"]),
                    Profielfoto = reader["profielfoto"] == DBNull.Value ? null : (byte[])reader["profielfoto"],
                    RegDatum = Convert.ToDateTime(reader["regdatum"]),
                    IsAdmin = Convert.ToBoolean(reader["isadmin"])
                };
            }
            else
            {
                return null;
            }
        }

        // personen overzicht page
        public static List<Persoon> GetAllPersonen()
        {
            List<Persoon> personen = new ();
            using (SqlConnection conn = new (connString))
            {
                // open connectie
                conn.Open();

                // voer SQL commando uit
                SqlCommand comm = new ("SELECT * FROM Persoon", conn);
                SqlDataReader reader = comm.ExecuteReader();

                // lees en verwerk resultaten
                while (reader.Read())
                {
                    personen.Add(new Persoon()
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Voornaam = Convert.ToString(reader["voornaam"]),
                        Achternaam = Convert.ToString(reader["achternaam"]),
                        Login = Convert.ToString(reader["login"]),
                        Paswoord = Convert.ToString(reader["paswoord"]),
                        Profielfoto = reader["profielfoto"] == DBNull.Value ? null : (byte[])reader["profielfoto"],
                        RegDatum = Convert.ToDateTime(reader["regdatum"]),
                        IsAdmin = Convert.ToBoolean(reader["isadmin"])
                    });
                }
            }
            return personen;
        }

        public int InsertInDb()
        {
            using SqlConnection conn = new (connString);
            // open connectie
            conn.Open();

            // voer SQL commando uit
            SqlCommand comm = new (@"INSERT INTO Persoon(voornaam, achternaam, login, paswoord, profielfoto, regdatum, isadmin) 
                                                    output INSERTED.ID VALUES(@voornaam, @achternaam, @login, @paswoord, @profielfoto, @regdatum, @isadmin)", conn);
            comm.Parameters.AddWithValue("@voornaam", Voornaam);
            comm.Parameters.AddWithValue("@achternaam", Achternaam);
            comm.Parameters.AddWithValue("@login", Login);
            comm.Parameters.AddWithValue("@paswoord", Paswoord);
            comm.Parameters.AddWithValue("@regdatum", RegDatum);
            comm.Parameters.AddWithValue("@isadmin", IsAdmin);

            // https://www.codeproject.com/Questions/327665/Operand-type-clash-nvarchar-is-incompatible-with-i
            if (Profielfoto == null)
            {
                SqlParameter imageParameter = new ("@profielfoto", SqlDbType.Image)
                {
                    Value = DBNull.Value
                };
                comm.Parameters.Add(imageParameter);
            }
            else
            {
                comm.Parameters.AddWithValue("@profielfoto", Profielfoto);
            }

            // return de id van het nieuwe record
            Id = (int)comm.ExecuteScalar();
            return Id;
        }

        public void DeleteFromDb()
        {
            // verwijder uit Deelname
            using (SqlConnection conn = new (connString))
            {
                conn.Open();
                SqlCommand comm = 
                    new ("DELETE FROM Deelname " +
                                    "WHERE persoon_id = @parID " +
                                    "OR activiteit_id IN (SELECT id FROM Activiteit WHERE organisator_id = @parID)",
                                    conn);

                comm.Parameters.AddWithValue("@parID", Id);
                comm.ExecuteNonQuery();
            }

            // verwijder uit Activiteit
            using (SqlConnection conn = new (connString))
            {
                conn.Open();
                SqlCommand comm = new ("DELETE FROM Activiteit WHERE organisator_id = @parID", conn);
                comm.Parameters.AddWithValue("@parID", Id);
                comm.ExecuteNonQuery();
            }

            // verwijder Persoon
            using (SqlConnection conn = new (connString))
            {
                conn.Open();
                SqlCommand comm = new ("DELETE FROM Persoon WHERE id = @parID", conn);
                comm.Parameters.AddWithValue("@parID", Id);
                comm.ExecuteNonQuery();
            }
        }

        public void UpdateInDb()
        {
            using SqlConnection conn = new (connString);
            // open connectie
            conn.Open();

            // voer SQL commando uit
            SqlCommand comm = new (
               @"UPDATE Persoon
                       SET voornaam = @voornaam,
                            achternaam = @achternaam,
                            login = @login,
                            paswoord = @paswoord,
                            profielfoto = @profielfoto,
                            isadmin = @isadmin
                       WHERE ID = @parID", conn);
            comm.Parameters.AddWithValue("@voornaam", Voornaam);
            comm.Parameters.AddWithValue("@achternaam", Achternaam);
            comm.Parameters.AddWithValue("@login", Login);
            comm.Parameters.AddWithValue("@paswoord", Paswoord);
            if (Profielfoto == null)
            {
                SqlParameter imageParameter = new ("@profielfoto", SqlDbType.Image)
                {
                    Value = DBNull.Value
                };
                comm.Parameters.Add(imageParameter);
            }
            else
            {
                comm.Parameters.AddWithValue("@profielfoto", Profielfoto);
            }

            comm.Parameters.AddWithValue("@isadmin", IsAdmin);
            comm.Parameters.AddWithValue("@parID", Id);
            comm.ExecuteNonQuery();
        }
    }
}