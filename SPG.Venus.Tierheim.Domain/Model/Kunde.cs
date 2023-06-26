using SPG.Venus.Tierheim.Domain.Interfaces;

namespace SPG.Venus.Tierheim.Domain.Model
{
    public class Kunde : EntityBase, IFindableByGuid
    {

        // PROPERTIES ----------------------------------------------------------

        public Guid Guid { get; private set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public Addresse Adresse { get; set; }

        public Geschlecht Geschlecht { get; set; }


        private List<Haustier> _tiere = new();
        // Lambda function
        public virtual IReadOnlyList<Haustier> Tiere => _tiere;



        // CTOR ---------------------------------------------------------------

        protected Kunde() { }


        public Kunde(Guid guid, string vorname, string nachname, Addresse adresse, Geschlecht geschlecht)
        {
            Guid = guid;
            Vorname = vorname;
            Nachname = nachname;
            Adresse = adresse;
            Geschlecht = geschlecht;
        }



        // METHODEN -----------------------------------------------------------


        // Holt eine Katze vom Heim und added es in seine Haustierliste
        public Katze HoleKatzeAusHeim(Tierheimhaus tierheim, int maxAlter)
        {
            Katze katze = tierheim.KatzeMitnehmen(maxAlter);
            _tiere.Add(katze);
            return katze;
        }

        // Holt einen Hund vom Heim und added es in seine Haustierliste
        public Hund HoleHundAusHeim(Tierheimhaus tierheim, int maxAlter)
        {
            Hund hund = tierheim.HundMitnehmen(maxAlter);
            _tiere.Add(hund);
            return hund;
        }

        // Bringt alle Haustiere in das Heim zurück
        public void AlleZurueckInsHeimBringen(Tierheimhaus tierheim)
        {
            _tiere.ForEach(tier => tierheim.TierInsHeimBringen(tier));
            _tiere.Clear();
        }

        // Wieviele Haustiere hat der Kunde bei sich zu Hause?
        public int TiereZaehlen()
        {
            return _tiere.Count();
        }
    }
}

