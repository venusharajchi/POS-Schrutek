namespace SPG.Venus.Tierheim.Domain.Model
{
    public class Tierheimhaus : EntityBase
    {

        // PROPERTIES ----------------------------------------------------------


        public Guid Guid { get; private set; }

        public Addresse Adresse { get; set; }


        // Wann ist das Heim geöffnet
        public DateTime OeffungszeitVon { get; set; }
        public DateTime OeffungszeitBis { get; set; }


        // Das Personal des Heims
        private List<Personal> _personal = new();
        public virtual IReadOnlyList<Personal> Personal => _personal;


        // Die Haustiere-Liste im Heim, die von Kunden abgeholt werden
        private List<Haustier> _tiere = new();
        public virtual IReadOnlyList<Haustier> Tiere => _tiere;



        // CTOR ----------------------------------------------------------------

        protected Tierheimhaus() { }


        public Tierheimhaus(Guid guid, Addresse adresse,
            DateTime oeffungszeitVon, DateTime oeffungszeitBis)
        {
            Guid = guid;
            Adresse = adresse;
            OeffungszeitVon = oeffungszeitVon;
            OeffungszeitBis = oeffungszeitBis;
        }



        // METHODEN ------------------------------------------------------------

        // Bringt ein Haustier ins Heim
        public void TierInsHeimBringen(Haustier tier)
        {
            _tiere.Add(tier);
        }


        // Nimmt eine Katze aus dem Heim und retourniert die Referenz
        public Katze KatzeMitnehmen(int maxAlter)
        {
            // Bitte Katze suchen gehen
            Katze katze = (Katze)_tiere.First(tier => tier is Katze && ((Katze)tier).Alter <= maxAlter);
            _tiere.Remove(katze);
            return katze;
        }

        // Nimmt einen Hund aus dem Heim und retourniert die Referenz
        public Hund HundMitnehmen(int maxAlter)
        {
            // Bitte Hund suchen gehen
            Hund hund = (Hund)_tiere.First(tier => tier is Hund && ((Hund)tier).Alter <= maxAlter);
            _tiere.Remove(hund);
            return hund;
        }

        // Impft alle Haustiere im Heim
        public void AlleTiereImpfenLassen()
        {
            _tiere.ForEach(tier =>
            {
                // Ist das Tier noch nicht geimpft?
                if (!tier.IsGeimpft)
                {
                    tier.IsGeimpft = true;
                }
            });

        }

        // Stellt ein Personal ins Heim an
        public void PersonalAnstellen(Personal personal)
        {
            // Maximal 3 Personal im Tierhim
            if (_personal.Count < 3)
            {
                // Aber keine zweimal
                if (!_personal.Contains(personal))
                {
                    _personal.Add(personal);
                }
                else
                {
                    throw new ArgumentException("Personal schon angestellt");
                }
            }
            else
            {
                throw new ArgumentException("Schon zuviel Personal schon angestellt");
            }
        }


        // Stellt mehrere Personal ins Heim an
        public void PersonalAnstellen(List<Personal> personalListe)
        {
            foreach (var personal in personalListe)
            {
                try
                {
                    PersonalAnstellen(personal);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        // Wieviel Haustiere lebem im Heim?
        public int TiereZaehlen()
        {
            return _tiere.Count();
        }
    }
}

