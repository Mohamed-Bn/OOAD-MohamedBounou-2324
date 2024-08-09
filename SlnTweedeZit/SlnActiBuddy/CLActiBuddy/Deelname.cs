using System.Configuration;
using System.Data.SqlClient;

namespace CLActiBuddy
{
    public class Deelname
    {
        public int Id { get; set; }
        public int PersoonId { get; set; }
        public int ActiviteitId { get; set; }
        public Persoon Persoon { get { return Persoon.GetById(PersoonId); } }

        private static string connString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        // alle deelnemers opvragen van een activiteit
        public static List<Persoon> GetDeelnemersByActiviteitId(int id)
        {
            List<Persoon> personen = new ();
            List<Deelname> deelnames = new ();
            using (SqlConnection conn = new (connString))
            {
                // open connectie
                conn.Open();

                // voer SQL commando uit
                SqlCommand comm = new ("SELECT * FROM Deelname WHERE activiteit_id = @id", conn);
                comm.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = comm.ExecuteReader();

                // lees en verwerk resultaten
                while (reader.Read())
                {
                    deelnames.Add(new Deelname()
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        PersoonId = Convert.ToInt32(reader["persoon_id"]),
                        ActiviteitId = Convert.ToInt32(reader["activiteit_id"]),
                    });
                }
            }

            foreach (Deelname deelname in deelnames)
            {
                personen.Add(deelname.Persoon);
            }

            return personen;
        }

        public int InsertInDb()
        {
            using SqlConnection conn = new (connString);
            // open connectie
            conn.Open();

            // voer SQL commando uit
            SqlCommand comm = new (@"INSERT INTO deelname(persoon_id, activiteit_id) 
                                                    output INSERTED.ID VALUES(@persoonId, @activiteitId)", conn);
            comm.Parameters.AddWithValue("@persoonId", PersoonId);
            comm.Parameters.AddWithValue("@activiteitId", ActiviteitId);

            // return de id van het nieuwe record
            Id = (int)comm.ExecuteScalar();
            return Id;
        }

        public void DeleteFromDb()
        {
            // verwijder gerelateerde deelnames
            using SqlConnection conn = new (connString);
            conn.Open();
            SqlCommand comm = new ("DELETE FROM deelname WHERE persoon_id = @persoonId AND activiteit_id = @activiteitId", conn);
            comm.Parameters.AddWithValue("@persoonId", PersoonId);
            comm.Parameters.AddWithValue("@activiteitId", ActiviteitId);
            comm.ExecuteNonQuery();
        }
    }
}