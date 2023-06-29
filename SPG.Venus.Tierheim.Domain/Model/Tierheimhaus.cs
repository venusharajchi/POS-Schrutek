using SPG.Venus.Tierheim.Domain.Interfaces;

namespace SPG.Venus.Tierheim.Domain.Model
{
    // ROOT AGGREGATE
    public class Tierheimhaus : EntityBase, IFindableByGuid
    {

        // PROPERTIES ----------------------------------------------------------


        public Guid Guid { get; private set; }

        public Addresse Adresse { get; set; }

        public String Name { get; set; }


        // Wann ist das Heim geöffnet
        private TimeSpan _oeffungszeitVon = TimeSpan.Zero;
        private TimeSpan _oeffungszeitBis = TimeSpan.Zero;

        public TimeSpan OeffungszeitVon
        {
            get { return _oeffungszeitVon; }
            set
            {
                if (_oeffungszeitBis != TimeSpan.Zero && value >= _oeffungszeitBis)
                {
                    throw new ArgumentException("OeffungszeitVon must be before OeffungszeitBis");
                }

                _oeffungszeitVon = value;
            }
        }

        public TimeSpan OeffungszeitBis
        {
            get { return _oeffungszeitBis; }
            set
            {
                if (_oeffungszeitVon != TimeSpan.Zero && value <= _oeffungszeitVon)
                {
                    throw new ArgumentException("OeffungszeitBis must be after OeffungszeitVon");
                }

                _oeffungszeitBis = value;
            }
        }



        // Das Personal des Heims
        private List<Personal> _personal = new();
        public virtual IReadOnlyList<Personal> Personal => _personal;


        // Die Haustiere-Liste im Heim, die von Kunden abgeholt werden
        private List<Haustier> _tiere = new();
        public virtual IReadOnlyList<Haustier> Tiere => _tiere;




        // CTOR ----------------------------------------------------------------

        protected Tierheimhaus() { }


        public Tierheimhaus(Guid guid, String name, Addresse adresse,
            TimeSpan oeffungszeitVon, TimeSpan oeffungszeitBis)
        {
            if (oeffungszeitBis <= oeffungszeitVon)
            {
                throw new ArgumentException("OeffungszeitBis must be after OeffungszeitVon");
            }

            Guid = guid;
            Name = name;
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



        // Nimmt eine Katze aus dem Heim und gibt die Referenz zurück
        public Katze KatzeMitnehmen(int maxAlter)
        {
            Katze? katze = _tiere.FirstOrDefault(tier => tier is Katze && ((Katze)tier).Alter <= maxAlter) as Katze;

            if (katze == null)
                throw new InvalidOperationException("Keine passende Katze im Tierheimhaus gefunden.");

            _tiere.Remove(katze);

            return katze;
        }



        // Nimmt einen Hund aus dem Heim und gibt die Referenz zurück
        public Hund HundMitnehmen(int maxAlter)
        {
            Hund? hund = _tiere.FirstOrDefault(tier => tier is Hund && ((Hund)tier).Alter <= maxAlter) as Hund;

            if (hund == null)
                throw new InvalidOperationException("Keinen passenden Hund im Tierheimhaus gefunden.");

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


        // Stellt ein Personal ins Heim an, 2-seitige Assoziation
        public void PersonalAnstellen(Personal personal)
        {
            // Maximal 10 Personal im Tierhim
            if (_personal.Count < 3)
            {
                // Aber keine zweimal
                if (!_personal.Contains(personal))
                {
                    // Noch Frei
                    if (personal.TierheimNavigation == null)
                    {
                        _personal.Add(personal);
                        personal.TierheimNavigation = this;
                    }
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
                PersonalAnstellen(personal);
        }


        // Wieviel Haustiere lebem im Heim?
        public int TiereZaehlen()
        {
            return _tiere.Count();
        }
    }
}

