using Bogus;
using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Model;
using Xunit.Abstractions;


namespace SPG.Venus.Tierheim.Infrastructure
{
    public class TierheimContext : DbContext
    {
        private readonly ITestOutputHelper output;

        public DbSet<Addresse> Adressen => Set<Addresse>();
        public DbSet<Haustier> Haustiere => Set<Haustier>();
        public DbSet<Hund> Hunde => Set<Hund>();
        public DbSet<Katze> Katzen => Set<Katze>();
        public DbSet<Kunde> Kunden => Set<Kunde>();
        public DbSet<Personal> Personal => Set<Personal>();
        public DbSet<Tierheimhaus> Tierheimhaeuser => Set<Tierheimhaus>();

        public TierheimContext(ITestOutputHelper output)
        {
            this.output = output;
        }

        public TierheimContext(DbContextOptions options, ITestOutputHelper output)
            : base(options)
        {
            this.output = output;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Value Objects
            modelBuilder.Entity<Kunde>().OwnsOne(k => k.Adresse);
            modelBuilder.Entity<Tierheimhaus>().OwnsOne(th => th.Adresse);

            // Table-Per-Hierarchy
            //modelBuilder.Entity<Haustier>()
            //    .HasDiscriminator<string>("HaustierType")
            //    .HasValue<Hund>("Hund")
            //    .HasValue<Katze>("Katze");
        }


        public void Seed()
        {
            Randomizer.Seed = new Random(181025);


            // Seeder für Haustier (Katze and Hund) ----------------------------

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

            Haustiere.AddRange(haustiere);
            SaveChanges();


            output.WriteLine("1- haustiere ------------------");
            output.WriteLine(haustiere.ToString());




            // Seeder for Kunden -----------------------------------------------

            
            List<Kunde> kunden = new Faker<Kunde>("de")
                .CustomInstantiator(f => new Kunde(
                    f.Random.Guid(),
                    f.Name.FirstName(),
                    f.Name.LastName(),
                    new Addresse(
                        f.Address.StreetName(),
                        f.Address.BuildingNumber(),
                        f.Address.City(),
                        f.Address.Country()),
                    f.PickRandom<Geschlecht>()
                ))
                .RuleFor(c => c.Tiere, f => f.Random.ListItems(haustiere, f.Random.Int(1, 3)).ToList())
                .Generate(50);

            Kunden.AddRange(kunden);
            SaveChanges();


            output.WriteLine("2- Kunden ------------------");
            output.WriteLine(kunden.ToString());




            // Seeder für Tierheimhäuser ---------------------------------------

            List<Tierheimhaus> tierheimhauser = new Faker<Tierheimhaus>("de")
                .CustomInstantiator(f => new Tierheimhaus(
                    f.Random.Guid(),
                    new Addresse(
                    f.Address.StreetName(),
                    f.Address.BuildingNumber(),
                    f.Address.City(),
                    f.Address.Country()),
                f.Date.Between(DateTime.Now.AddHours(-8), DateTime.Now.AddHours(-1)),
                f.Date.Between(DateTime.Now.AddHours(1), DateTime.Now.AddHours(8))
            ))
            .RuleFor(t => t.Tiere, f => f.Random.ListItems(haustiere, f.Random.Int(1, 10)).ToList())
            .Generate(10);

            Tierheimhaeuser.AddRange(tierheimhauser);
            SaveChanges();


            output.WriteLine("3- Tierheimhäuser ------------------");
            output.WriteLine(tierheimhauser.ToString());




            // Seeder für Personal ---------------------------------------------

            List<Personal> personal = new Faker<Personal>("de")
                .CustomInstantiator(f => new Personal(
                    f.Random.Guid(),
                    f.Name.FirstName(),
                    f.Name.LastName(),
                    f.PickRandom<Geschlecht>(),
                    f.Random.AlphaNumeric(10),
                    f.Random.ListItem(tierheimhauser)
                ))
                .Generate(50);

            Personal.AddRange(personal);
            SaveChanges();


            output.WriteLine("4- Personal ------------------");
            output.WriteLine(personal.ToString());




            // Seeder for Tierheimhaus -----------------------------------------

            tierheimhauser.ForEach(tierheim =>
            {
                var zugewiesenesPersonal = personal.Where(p => p.TierheimhausNavigationId == tierheim.Id).ToList();
                tierheim.PersonalAnstellen(zugewiesenesPersonal);
            });


            SaveChanges();
            output.WriteLine("5- ALLES OKAY ------------------");

        }
    }
}

