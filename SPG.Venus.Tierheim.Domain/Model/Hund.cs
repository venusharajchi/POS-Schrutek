namespace SPG.Venus.Tierheim.Domain.Model
{
    // CHILD CLASS OF HAUSTIER
    public class Hund : Haustier
    {
        public bool IsBissig;

        protected Hund() { }

        public Hund(Guid guid, bool isBissig, string name, Geschlecht geschlecht, int alter)
            : base(guid, name, geschlecht, alter)
        {
            IsBissig = isBissig;
        }

        public override string machtGeraeusch()
        {
            return "waauuuuuuuu";
        }

    }
}

