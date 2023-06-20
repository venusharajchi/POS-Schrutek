namespace SPG.Venus.Tierheim.Domain.Model
{
    // VALUE OBJECT
    public class Addresse
    {
        public String Strasse { get; private set; }

        public String Hausnummer { get; private set; }

        public String Stadt { get; private set; }

        public String Land { get; private set; }

        protected Addresse() { }

        public Addresse(string strasse, String hausnummer, string stadt, string land)
        {
            Strasse = strasse;
            Hausnummer = hausnummer;
            Stadt = stadt;
            Land = land;
        }
    }
}

