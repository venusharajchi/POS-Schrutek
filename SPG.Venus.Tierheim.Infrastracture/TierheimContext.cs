using Bogus;
using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Model;
using Xunit.Abstractions;


namespace SPG.Venus.Tierheim.Infrastructure
{
    public class TierheimContext : DbContext
    {
        public DbSet<Haustier> Haustiere => Set<Haustier>();
        public DbSet<Hund> Hunde => Set<Hund>();
        public DbSet<Katze> Katzen => Set<Katze>();
        public DbSet<Kunde> Kunden => Set<Kunde>();
        public DbSet<Personal> Personal => Set<Personal>();
        public DbSet<Tierheimhaus> Tierheimhaeuser => Set<Tierheimhaus>();

        public TierheimContext() { }

        public TierheimContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Tierheimhaus>().HasKey(h => h.Name);

            // Value Objects
            modelBuilder.Entity<Kunde>().OwnsOne(k => k.Adresse);
            modelBuilder.Entity<Tierheimhaus>().OwnsOne(th => th.Adresse);
        }


        public void Seed()
        {
            Randomizer.Seed = new Random(181025);



            // Seeder for Haustier (Katze and Hund) ----------------------------

            List<Haustier> haustiere = new Faker<Haustier>()
                .CustomInstantiator(f =>
                {
                    var isKatze = f.Random.Bool();
                    var guid = f.Random.Guid();
                    var name = f.Name.FirstName();
                    var geschlecht = f.PickRandom<Geschlecht>();
                    var alter = f.Random.Int(1, 15);
                    if (isKatze)
                    {
                        var isAnschmiegsam = f.Random.Bool();
                        return new Katze(guid, isAnschmiegsam, name, geschlecht, alter);
                    }
                    else
                    {
                        var isBissig = f.Random.Bool();
                        return new Hund(guid, isBissig, name, geschlecht, alter);
                    }
                })
                .Generate(100);

            //Haustiere.AddRange(haustiere);
            SaveChanges();



            // Seeder for Kunden -----------------------------------------------

            var kunden = new Faker<Kunde>("de")
            .CustomInstantiator(f =>
            {
                var kunde = new Kunde(
                    f.Random.Guid(),
                    f.Name.FirstName(),
                    f.Name.LastName(),
                    new Addresse(
                        f.Address.StreetName(),
                        f.Address.BuildingNumber(),
                        f.Address.City(),
                        f.Address.Country()),
                    f.PickRandom<Geschlecht>()
                );

                var haustiereL = f.Random.ListItems(haustiere, f.Random.Int(5, 10)).ToList();

                foreach (var haustier in haustiere)
                {
                    kunde.AddHaustier(haustier);

                    Haustiere.Add(haustier); // Add Haustier to the context
                }

                Kunden.Add(kunde); // Add Kunde to the context

                return kunde;
            })
            .Generate(50);

            SaveChanges(); // Save changes to both Kunden and Haustiere





            // Seeder for Tierheimhaus
            List<Tierheimhaus> tierheimhauser = new Faker<Tierheimhaus>("de")
                .CustomInstantiator(f => new Tierheimhaus(
                    f.Random.Guid(),
                    f.Company.CompanyName(),
                    new Addresse(
                        f.Address.StreetName(),
                        f.Address.BuildingNumber(),
                        f.Address.City(),
                        f.Address.Country()),
                    TimeSpan.FromHours(f.Random.Int(1, 8)),
                    TimeSpan.FromHours(f.Random.Int(9, 17))
                ))
                //.RuleFor(t => t.Tiere, f => f.Random.ListItems(haustiere, f.Random.Int(5, 10)).ToList())
                .Generate(50);

            Tierheimhaeuser.AddRange(tierheimhauser);
            SaveChanges();


            int i = 0;
            List<Personal> personal = new Faker<Personal>("de")
                .CustomInstantiator(f => {
                    var tierheim = tierheimhauser[i++];
                    var newPersonal = new Personal(
                        f.Random.Guid(),
                        f.Name.FirstName(),
                        f.Name.LastName(),
                        f.PickRandom<Geschlecht>(),
                        f.Random.AlphaNumeric(10)
                    );
                    tierheim.PersonalAnstellen(newPersonal);
                    return newPersonal;
                })
                .Generate(50);

            Personal.AddRange(personal);
            SaveChanges();




        }




    }
}

