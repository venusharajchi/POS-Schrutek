namespace SPG.Venus.Tierheim.Domain.Model
{
    // ENTITIY
    public class Personal : EntityBase
    {

        // PROPERTIES ---------------------------------------------------------

        public Guid Guid { get; private set; }

        public string Dienstnummer { get; set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public Geschlecht Geschlecht { get; set; }


        // NAVIGATION
        public int TierheimhausNavigationId { get; set; }
        public virtual Tierheimhaus TierheimhausNavigation { get; private set; } = default!;



        // CTOR ----------------------------------------------------------------


        protected Personal() { }


        public Personal(Guid guid, string vorname, string nachname,
            Geschlecht geschlecht, string dienstnummer, Tierheimhaus tierheimhausNavigation)
        {
            Guid = guid;
            Vorname = vorname;
            Nachname = nachname;
            Geschlecht = geschlecht;
            Dienstnummer = dienstnummer;
            TierheimhausNavigation = tierheimhausNavigation;
        }



        // METHODS -------------------------------------------------------------

        // Zwei Personal haben gleiche Dienstnummer
        public override bool Equals(object? obj)
        {
            return obj is Personal personal
                && Dienstnummer == personal.Dienstnummer;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Dienstnummer);
        }
    }
}

