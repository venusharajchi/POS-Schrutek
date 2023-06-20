namespace SPG.Venus.Tierheim.Domain.Model
{
    // ENTITIY
    public abstract class Haustier : EntityBase
    {

        // PROPERTIES ----------------------------------------------------------

        public Guid Guid { get; private set; }

        public string Name { get; set; }

        public Geschlecht Geschlecht { get; set; }

        public int Alter { get; set; }


        // Kein Haustier ist am Anfang geimpft
        public bool IsGeimpft { get; set; } = false;



        // CTOR ----------------------------------------------------------------

        protected Haustier() { }

        public Haustier(Guid guid, string name, Geschlecht geschlecht, int alter)
        {
            Guid = guid;
            Name = name;
            Geschlecht = geschlecht;
            Alter = alter;
        }



        // METHODS -------------------------------------------------------------

        public abstract string machtGeraeusch();
    }
}

