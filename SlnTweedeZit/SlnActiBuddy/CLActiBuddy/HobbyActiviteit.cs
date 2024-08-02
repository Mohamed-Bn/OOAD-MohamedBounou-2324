namespace CLActiBuddy
{
    public class HobbyActiviteit : Activiteit
    {
        public ActiviteitNiveau? Niveau { get; set; }
    }

    public enum ActiviteitNiveau
    {
        Nvt,
        Beginner,
        Halfgevorderd,
        Gevorderd
    }
}