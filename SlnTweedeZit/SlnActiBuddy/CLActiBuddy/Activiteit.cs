using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CLActiBuddy
{
    public abstract class Activiteit
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string Beschrijving { get; set; }
        public DateTime DatumTijd { get; set; }

        // ik heb surpress gedaan want []? zegt dat ik spatie moet zetten, en [] ? zegt dat er geen spatie mag
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
        public byte[]? Icoon { get; set; }
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int MaxPersonen { get; set; }
        public ActiviteitSoort Soort { get; set; }
        public int Leeftijdsgroep { get; set; }
        public int OrganisatorId { get; set; }
        public Persoon Organisator { get { return Persoon.GetById(OrganisatorId); } }
        public List<Persoon> Deelnemers { get { return Deelname.GetDeelnemersByActiviteitId(Id); } }

        private static readonly string ConnString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        // voor organiseer pagina, om enkel jou activiteiten op te vragen. in plaats van alles opvragen en filteren vragen we enkel eigen activiteiten op.
        public static List<Activiteit> GetActiviteitenByPersoonId(int personId)
        {
            var activiteiten = new List<Activiteit>();
            using (SqlConnection conn = new(ConnString))
            {
                conn.Open();
                using SqlCommand cmd = new("SELECT * FROM Activiteit WHERE organisator_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", personId);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ActiviteitSoort soort = (ActiviteitSoort)(int)reader["soort"];
                    if (soort == ActiviteitSoort.Sport)
                    {
                        activiteiten.Add(
                            new SportActiviteit
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Titel = Convert.ToString(reader["titel"]),
                                Beschrijving = Convert.ToString(reader["beschrijving"]),
                                DatumTijd = Convert.ToDateTime(reader["datumtijd"]),
                                Icoon = reader["icoon"] == DBNull.Value ? null : (byte[])reader["icoon"],
                                Longitude = Convert.ToDecimal(reader["longitude"]),
                                Latitude = Convert.ToDecimal(reader["latitude"]),
                                MaxPersonen = Convert.ToInt32(reader["maxpersonen"]),
                                Soort = (ActiviteitSoort)Convert.ToInt32(reader["soort"]),
                                Leeftijdsgroep = Convert.ToInt32(reader["leeftijdsgroep"]),
                                Moeilijkheid = reader["moeilijkheid"] == DBNull.Value ? null : (ActiviteitMoeilijkheid)Convert.ToInt32(reader["moeilijkheid"]),
                                OrganisatorId = Convert.ToInt32(reader["organisator_id"]),
                            });
                    }
                    else if (soort == ActiviteitSoort.Hobby)
                    {
                        activiteiten.Add(
                            new HobbyActiviteit
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Titel = Convert.ToString(reader["titel"]),
                                Beschrijving = Convert.ToString(reader["beschrijving"]),
                                DatumTijd = Convert.ToDateTime(reader["datumtijd"]),
                                Icoon = reader["icoon"] == DBNull.Value ? null : (byte[])reader["icoon"],
                                Longitude = Convert.ToDecimal(reader["longitude"]),
                                Latitude = Convert.ToDecimal(reader["latitude"]),
                                MaxPersonen = Convert.ToInt32(reader["maxpersonen"]),
                                Soort = (ActiviteitSoort)Convert.ToInt32(reader["soort"]),
                                Leeftijdsgroep = Convert.ToInt32(reader["leeftijdsgroep"]),
                                Niveau = reader["niveau"] == DBNull.Value ? null : (ActiviteitNiveau)Convert.ToInt32(reader["niveau"]),
                                OrganisatorId = Convert.ToInt32(reader["organisator_id"]),
                            });
                    }
                    else if (soort == ActiviteitSoort.Cultuur)
                    {
                        activiteiten.Add(
                            new CultuurActiviteit
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Titel = Convert.ToString(reader["titel"]),
                                Beschrijving = Convert.ToString(reader["beschrijving"]),
                                DatumTijd = Convert.ToDateTime(reader["datumtijd"]),
                                Icoon = reader["icoon"] == DBNull.Value ? null : (byte[])reader["icoon"],
                                Longitude = Convert.ToDecimal(reader["longitude"]),
                                Latitude = Convert.ToDecimal(reader["latitude"]),
                                MaxPersonen = Convert.ToInt32(reader["maxpersonen"]),
                                Soort = (ActiviteitSoort)Convert.ToInt32(reader["soort"]),
                                Leeftijdsgroep = Convert.ToInt32(reader["leeftijdsgroep"]),
                                Sector = reader["sector"] == DBNull.Value ? null : (ActiviteitSector)Convert.ToInt32(reader["sector"]),
                                OrganisatorId = Convert.ToInt32(reader["organisator_id"]),
                            });
                    }
                }
            }
            return activiteiten;
        }

        public static List<Activiteit> GetAllActiviteiten()
        {
            var activiteiten = new List<Activiteit>();
            using (SqlConnection conn = new(ConnString))
            {
                conn.Open();
                using SqlCommand cmd = new("SELECT * FROM Activiteit", conn);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ActiviteitSoort soort = (ActiviteitSoort)(int)reader["soort"];
                    if (soort == ActiviteitSoort.Sport)
                    {
                        activiteiten.Add(
                            new SportActiviteit
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Titel = Convert.ToString(reader["titel"]),
                                Beschrijving = Convert.ToString(reader["beschrijving"]),
                                DatumTijd = Convert.ToDateTime(reader["datumtijd"]),
                                Icoon = reader["icoon"] == DBNull.Value ? null : (byte[])reader["icoon"],
                                Longitude = Convert.ToDecimal(reader["longitude"]),
                                Latitude = Convert.ToDecimal(reader["latitude"]),
                                MaxPersonen = Convert.ToInt32(reader["maxpersonen"]),
                                Soort = (ActiviteitSoort)Convert.ToInt32(reader["soort"]),
                                Leeftijdsgroep = Convert.ToInt32(reader["leeftijdsgroep"]),
                                Moeilijkheid = reader["moeilijkheid"] == DBNull.Value ? null : (ActiviteitMoeilijkheid)Convert.ToInt32(reader["moeilijkheid"]),
                                OrganisatorId = Convert.ToInt32(reader["organisator_id"]),
                            });
                    }
                    else if (soort == ActiviteitSoort.Hobby)
                    {
                        activiteiten.Add(
                            new HobbyActiviteit
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Titel = Convert.ToString(reader["titel"]),
                                Beschrijving = Convert.ToString(reader["beschrijving"]),
                                DatumTijd = Convert.ToDateTime(reader["datumtijd"]),
                                Icoon = reader["icoon"] == DBNull.Value ? null : (byte[])reader["icoon"],
                                Longitude = Convert.ToDecimal(reader["longitude"]),
                                Latitude = Convert.ToDecimal(reader["latitude"]),
                                MaxPersonen = Convert.ToInt32(reader["maxpersonen"]),
                                Soort = (ActiviteitSoort)Convert.ToInt32(reader["soort"]),
                                Leeftijdsgroep = Convert.ToInt32(reader["leeftijdsgroep"]),
                                Niveau = reader["niveau"] == DBNull.Value ? null : (ActiviteitNiveau)Convert.ToInt32(reader["niveau"]),
                                OrganisatorId = Convert.ToInt32(reader["organisator_id"]),
                            });
                    }
                    else if (soort == ActiviteitSoort.Cultuur)
                    {
                        activiteiten.Add(
                            new CultuurActiviteit
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Titel = Convert.ToString(reader["titel"]),
                                Beschrijving = Convert.ToString(reader["beschrijving"]),
                                DatumTijd = Convert.ToDateTime(reader["datumtijd"]),
                                Icoon = reader["icoon"] == DBNull.Value ? null : (byte[])reader["icoon"],
                                Longitude = Convert.ToDecimal(reader["longitude"]),
                                Latitude = Convert.ToDecimal(reader["latitude"]),
                                MaxPersonen = Convert.ToInt32(reader["maxpersonen"]),
                                Soort = (ActiviteitSoort)Convert.ToInt32(reader["soort"]),
                                Leeftijdsgroep = Convert.ToInt32(reader["leeftijdsgroep"]),
                                Sector = reader["sector"] == DBNull.Value ? null : (ActiviteitSector)Convert.ToInt32(reader["sector"]),
                                OrganisatorId = Convert.ToInt32(reader["organisator_id"]),
                            });
                    }
                }
            }
            return activiteiten;
        }

        public void DeleteFromDb()
        {
            // verwijder uit Deelname
            using (SqlConnection conn = new(ConnString))
            {
                conn.Open();
                SqlCommand comm = new("DELETE FROM Deelname WHERE activiteit_id = @parID ", conn);

                comm.Parameters.AddWithValue("@parID", Id);
                comm.ExecuteNonQuery();
            }

            // verwijder uit Activiteit
            using (SqlConnection conn = new(ConnString))
            {
                conn.Open();
                SqlCommand comm = new("DELETE FROM Activiteit WHERE id = @parID", conn);
                comm.Parameters.AddWithValue("@parID", Id);
                comm.ExecuteNonQuery();
            }
        }

        public void InsertInDb()
        {
            using SqlConnection conn = new(ConnString);
            conn.Open();
            string sql = "";

            if (this is CultuurActiviteit cultuurActiviteit)
            {
                sql = "INSERT INTO activiteit (Titel, Beschrijving, DatumTijd, Icoon, Longitude, Latitude, MaxPersonen, Soort, Leeftijdsgroep, Organisator_Id, Sector) " +
                      "VALUES (@Titel, @Beschrijving, @DatumTijd, @Icoon, @Longitude, @Latitude, @MaxPersonen, @Soort, @Leeftijdsgroep, @OrganisatorId, @Sector)";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@Titel", cultuurActiviteit.Titel);
                cmd.Parameters.AddWithValue("@Beschrijving", cultuurActiviteit.Beschrijving);
                cmd.Parameters.AddWithValue("@DatumTijd", cultuurActiviteit.DatumTijd);
                if (cultuurActiviteit.Icoon == null)
                {
                    SqlParameter imageParameter = new("@Icoon", SqlDbType.Image)
                    {
                        Value = DBNull.Value
                    };
                    cmd.Parameters.Add(imageParameter);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Icoon", cultuurActiviteit.Icoon);
                }
                cmd.Parameters.AddWithValue("@Longitude", cultuurActiviteit.Longitude);
                cmd.Parameters.AddWithValue("@Latitude", cultuurActiviteit.Latitude);
                cmd.Parameters.AddWithValue("@MaxPersonen", cultuurActiviteit.MaxPersonen);
                cmd.Parameters.AddWithValue("@Soort", (int)cultuurActiviteit.Soort);
                cmd.Parameters.AddWithValue("@Leeftijdsgroep", cultuurActiviteit.Leeftijdsgroep);
                cmd.Parameters.AddWithValue("@OrganisatorId", cultuurActiviteit.OrganisatorId);
                cmd.Parameters.AddWithValue("@Sector", cultuurActiviteit.Sector == null ? DBNull.Value : (int)cultuurActiviteit.Sector);
                cmd.ExecuteNonQuery();
            }
            else if (this is SportActiviteit sportActiviteit)
            {
                sql = "INSERT INTO activiteit (Titel, Beschrijving, DatumTijd, Icoon, Longitude, Latitude, MaxPersonen, Soort, Leeftijdsgroep, Organisator_Id, Moeilijkheid) " +
                      "VALUES (@Titel, @Beschrijving, @DatumTijd, @Icoon, @Longitude, @Latitude, @MaxPersonen, @Soort, @Leeftijdsgroep, @OrganisatorId, @Moeilijkheid)";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@Titel", sportActiviteit.Titel);
                cmd.Parameters.AddWithValue("@Beschrijving", sportActiviteit.Beschrijving);
                cmd.Parameters.AddWithValue("@DatumTijd", sportActiviteit.DatumTijd);
                if (sportActiviteit.Icoon == null)
                {
                    SqlParameter imageParameter = new("@Icoon", SqlDbType.Image)
                    {
                        Value = DBNull.Value
                    };
                    cmd.Parameters.Add(imageParameter);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Icoon", sportActiviteit.Icoon);
                }
                cmd.Parameters.AddWithValue("@Longitude", sportActiviteit.Longitude);
                cmd.Parameters.AddWithValue("@Latitude", sportActiviteit.Latitude);
                cmd.Parameters.AddWithValue("@MaxPersonen", sportActiviteit.MaxPersonen);
                cmd.Parameters.AddWithValue("@Soort", (int)sportActiviteit.Soort);
                cmd.Parameters.AddWithValue("@Leeftijdsgroep", sportActiviteit.Leeftijdsgroep);
                cmd.Parameters.AddWithValue("@OrganisatorId", sportActiviteit.OrganisatorId);
                cmd.Parameters.AddWithValue("@Moeilijkheid", sportActiviteit.Moeilijkheid == null ? DBNull.Value : (int)sportActiviteit.Moeilijkheid);
                cmd.ExecuteNonQuery();
            }
            else if (this is HobbyActiviteit hobbyActiviteit)
            {
                sql = "INSERT INTO activiteit (Titel, Beschrijving, DatumTijd, Icoon, Longitude, Latitude, MaxPersonen, Soort, Leeftijdsgroep, Organisator_Id, Niveau) " +
                      "VALUES (@Titel, @Beschrijving, @DatumTijd, @Icoon, @Longitude, @Latitude, @MaxPersonen, @Soort, @Leeftijdsgroep, @OrganisatorId, @Niveau)";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@Titel", hobbyActiviteit.Titel);
                cmd.Parameters.AddWithValue("@Beschrijving", hobbyActiviteit.Beschrijving);
                cmd.Parameters.AddWithValue("@DatumTijd", hobbyActiviteit.DatumTijd);
                if (hobbyActiviteit.Icoon == null)
                {
                    SqlParameter imageParameter = new("@Icoon", SqlDbType.Image)
                    {
                        Value = DBNull.Value
                    };
                    cmd.Parameters.Add(imageParameter);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Icoon", hobbyActiviteit.Icoon);
                }
                cmd.Parameters.AddWithValue("@Longitude", hobbyActiviteit.Longitude);
                cmd.Parameters.AddWithValue("@Latitude", hobbyActiviteit.Latitude);
                cmd.Parameters.AddWithValue("@MaxPersonen", hobbyActiviteit.MaxPersonen);
                cmd.Parameters.AddWithValue("@Soort", (int)hobbyActiviteit.Soort);
                cmd.Parameters.AddWithValue("@Leeftijdsgroep", hobbyActiviteit.Leeftijdsgroep);
                cmd.Parameters.AddWithValue("@OrganisatorId", hobbyActiviteit.OrganisatorId);
                cmd.Parameters.AddWithValue("@Niveau", hobbyActiviteit.Niveau == null ? DBNull.Value : (int)hobbyActiviteit.Niveau);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public enum ActiviteitSoort
    {
        Sport = 1,
        Cultuur = 2,
        Hobby = 3
    }
}