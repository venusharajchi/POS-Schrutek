namespace SPG.Venus.Tierheim.Domain.Model
{
    // CHILD CLASS OF HAUSTIER
    public class Katze : Haustier
    {
        public bool IsAnschmiegsam;

        protected Katze() { }

        public Katze(Guid guid, bool isAnschmiegsam, string name, Geschlecht geschlecht, int alter)
            : base(guid, name, geschlecht, alter)
        {
            IsAnschmiegsam = isAnschmiegsam;
        }

        public override string machtGeraeusch()
        {
            return "miiiiaaauuuuuuuu";
        }
    }
}

