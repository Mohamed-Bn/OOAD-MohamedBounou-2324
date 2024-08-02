namespace CLActiBuddy
{
    public class SportActiviteit : Activiteit
    {
        public ActiviteitMoeilijkheid? Moeilijkheid { get; set; }
    }

    public enum ActiviteitMoeilijkheid
    {
        Nvt,
        Makkelijk,
        Gemiddeld,
        Zwaar
    }
}
