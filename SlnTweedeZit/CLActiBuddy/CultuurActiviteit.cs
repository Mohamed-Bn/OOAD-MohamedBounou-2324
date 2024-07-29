namespace CLActiBuddy
{
    public class CultuurActiviteit : Activiteit
    {
        public ActiviteitSector? Sector { get; set; }
    }

    public enum ActiviteitSector
    {
        Nvt,
        Muziek,
        Theater,
        Dans,
        Andere
    }
}
