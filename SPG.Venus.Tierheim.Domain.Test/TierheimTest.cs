using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Infrastructure;
using Xunit.Abstractions;
using SPG.Venus.Tierheim.Domain.Model;

namespace SPG.Venus.Tierheim.Domain.Test;

public class TierheimTest
{
    private readonly ITestOutputHelper output;


    public TierheimTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    private TierheimContext GenerateDb()
    {
        DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlite("Data Source=Tierheim_Test.db");

        TierheimContext db = new TierheimContext(optionsBuilder.Options, output);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        return db;
    }


    [Fact]
    public void SeedDb()
    {
        TierheimContext db = GenerateDb();

        db.Seed();
        Assert.True(true);
    }


    [Fact]
    public void TierInsHeimBringenTest()
    {
        using (var context = GenerateDb())
        {
            output.WriteLine("TierInsHeimBringenTest ------------------");

            // Arrange
            Katze katze = new Katze(Guid.NewGuid(), isAnschmiegsam: true, "Dora", Geschlecht.Frau, alter: 1);
            Hund hund = new Hund(Guid.NewGuid(), isBissig: true, "Belfi", Geschlecht.Mann, alter: 5);
            Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
            Tierheimhaus haus = new Tierheimhaus(Guid.NewGuid(), adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));
            Personal personal = new Personal(Guid.NewGuid(), "susi", "polter", Geschlecht.Frau, "1234567890", haus);

            haus.PersonalAnstellen(personal);
            haus.TierInsHeimBringen(katze);
            haus.TierInsHeimBringen(hund);

            // Act
            context.Tierheimhaeuser.Add(haus);
            context.SaveChanges();

            // Assert
            Tierheimhaus newHaus = context.Tierheimhaeuser
                .Include(h => h.Tiere)
                .First(h => h.Id.Equals(haus.Id));

            Assert.Equal(2, newHaus.Tiere.Count);
        }
    }


    [Fact]
    public void KatzeMitnehmenTest()
    {
        using (var context = GenerateDb())
        {
            output.WriteLine("KatzeMitnehmenTest ------------------");

            // Arrange
            Katze katze = new Katze(Guid.NewGuid(), isAnschmiegsam: true, "Dora", Geschlecht.Frau, alter: 1);

            Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
            Tierheimhaus haus = new Tierheimhaus(Guid.NewGuid(), adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));
            Personal personal = new Personal(Guid.NewGuid(), "susi", "polter", Geschlecht.Frau, "1234567890", haus);

            haus.PersonalAnstellen(personal);
            haus.TierInsHeimBringen(katze);

            context.Tierheimhaeuser.Add(haus);
            context.SaveChanges();

            // Act
            haus.KatzeMitnehmen(maxAlter: 1);
            context.SaveChanges();

            // Assert
            Tierheimhaus newHaus = context.Tierheimhaeuser
                .Include(h => h.Tiere)
                .First(h => h.Id.Equals(haus.Id));

            Assert.Equal(0, newHaus.Tiere.Count);
        }

    }



    [Fact]
    public void HundMitnehmenTest()
    {
        using (var context = GenerateDb())
        {
            // Arrange
            Guid hundGuid = Guid.NewGuid();
            Hund hund = new Hund(hundGuid, isBissig: true, "Belfi", Geschlecht.Mann, alter: 5);

            Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
            Tierheimhaus haus = new Tierheimhaus(Guid.NewGuid(), adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));

            haus.TierInsHeimBringen(hund);
            context.Tierheimhaeuser.Add(haus);
            context.SaveChanges();

            // Act
            haus.HundMitnehmen(maxAlter: 10);
            context.SaveChanges();

            // Assert
            Tierheimhaus newHaus = context.Tierheimhaeuser
                .Include(h => h.Tiere)
                .First(h => h.Id.Equals(haus.Id));

            Assert.Equal(0, newHaus.Tiere.Count);
        }
    }


    [Fact]
    public void AllTiereImpfenLassenTest()
    {
        using (var context = GenerateDb())
        {
            // Arrange
            Hund hund = new Hund(Guid.NewGuid(), isBissig: true, "Belfi", Geschlecht.Mann, alter: 5);
            Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
            Tierheimhaus haus = new Tierheimhaus(Guid.NewGuid(), adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));

            haus.TierInsHeimBringen(hund);
            context.Tierheimhaeuser.Add(haus);
            context.SaveChanges();

            // Act
            haus.AlleTiereImpfenLassen();
            context.SaveChanges();

            // Assert
            Tierheimhaus newHaus = context.Tierheimhaeuser
                .Include(h => h.Tiere)
                .First(h => h.Id.Equals(haus.Id));
            bool alleGeimpft = newHaus.Tiere.All(t => t.IsGeimpft);

            Assert.True(alleGeimpft);
        }
    }


    [Fact]
    public void PersonalAnstellenTest()
    {
        using (var context = GenerateDb())
        {
            // Arrange
            Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
            Tierheimhaus haus = new Tierheimhaus(Guid.NewGuid(), adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));

            Personal personal = new Personal(Guid.NewGuid(), "susi", "polter", Geschlecht.Frau, "1234567890", haus);
            context.Tierheimhaeuser.Add(haus);
            context.SaveChanges();

            // Act
            haus.PersonalAnstellen(personal);
            context.SaveChanges();

            // Assert
            Tierheimhaus newHaus = context.Tierheimhaeuser
                .Include(h => h.Personal)
                .First(h => h.Id.Equals(haus.Id));

            Assert.Single(newHaus.Personal);
        }
    }


    [Fact]
    public void testHoleKatzeAusHeim()
    {
        using (var context = GenerateDb())
        {
            // Arrange
            Katze katze = new Katze(Guid.NewGuid(), isAnschmiegsam: true, "Dora", Geschlecht.Frau, alter: 1);
            Hund hund = new Hund(Guid.NewGuid(), isBissig: true, "Belfi", Geschlecht.Mann, alter: 5);
            Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
            Tierheimhaus haus = new Tierheimhaus(Guid.NewGuid(), adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));

            Addresse adresse2 = new Addresse("andere strasse", "10", "linz", "oesterreich");
            Kunde kunde = new Kunde(Guid.NewGuid(), "Venus", "Harajchi", adresse2, Geschlecht.Frau);

            haus.TierInsHeimBringen(katze);
            haus.TierInsHeimBringen(hund);
            context.Tierheimhaeuser.Add(haus);
            context.Kunden.Add(kunde);
            context.SaveChanges();

            // Act
            kunde.HoleKatzeAusHeim(haus, maxAlter: 1);
            context.SaveChanges();

            // Assert
            Kunde newKunde = context.Kunden
                .Include(k => k.Tiere)
                .First(k => k.Id.Equals(kunde.Id));

            Assert.Equal(1, newKunde.Tiere.Count);
            Assert.Contains(newKunde.Tiere, t => t is Katze);
        }
    }

    [Fact]
    public void testHoleHundAusHeim()
    {
        using (var context = GenerateDb())
        {
            // Arrange
            Katze katze = new Katze(Guid.NewGuid(), isAnschmiegsam: true, "Dora", Geschlecht.Frau, alter: 1);
            Hund hund = new Hund(Guid.NewGuid(), isBissig: true, "Belfi", Geschlecht.Mann, alter: 5);
            Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
            Tierheimhaus haus = new Tierheimhaus(Guid.NewGuid(), adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));

            Addresse adresse2 = new Addresse("andere strasse", "10", "linz", "oesterreich");
            Kunde kunde = new Kunde(Guid.NewGuid(), "Venus", "Harajchi", adresse2, Geschlecht.Frau);

            haus.TierInsHeimBringen(katze);
            haus.TierInsHeimBringen(hund);
            context.Tierheimhaeuser.Add(haus);
            context.Kunden.Add(kunde);
            context.SaveChanges();

            // Act
            kunde.HoleHundAusHeim(haus, maxAlter: 20);
            context.SaveChanges();

            // Assert
            Kunde newKunde = context.Kunden
                .Include(k => k.Tiere)
                .First(k => k.Id.Equals(kunde.Id));

            Assert.Equal(1, newKunde.Tiere.Count);
            Assert.Contains(newKunde.Tiere, t => t is Hund);
        }
    }



    [Fact]
    public void testAlleTierZurueckBringen()
    {
        using (var context = GenerateDb())
        {
            // Arrange
            Katze katze = new Katze(Guid.NewGuid(), isAnschmiegsam: true, "Dora", Geschlecht.Frau, alter: 1);
            Hund hund = new Hund(Guid.NewGuid(), isBissig: true, "Belfi", Geschlecht.Mann, alter: 5);
            Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
            Tierheimhaus haus = new Tierheimhaus(Guid.NewGuid(), adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));

            Addresse adresse2 = new Addresse("andere strasse", "10", "linz", "oesterreich");
            Kunde kunde = new Kunde(Guid.NewGuid(), "Venus", "Harajchi", adresse2, Geschlecht.Frau);

            haus.TierInsHeimBringen(katze);
            haus.TierInsHeimBringen(hund);
            context.Tierheimhaeuser.Add(haus);
            context.Kunden.Add(kunde);
            context.SaveChanges();

            kunde.HoleHundAusHeim(haus, maxAlter: 20);
            context.SaveChanges();

            // Act
            kunde.AlleZurueckInsHeimBringen(haus);
            context.SaveChanges();

            // Assert
            Kunde newKunde = context.Kunden
                .Include(k => k.Tiere)
                .First(k => k.Id.Equals(kunde.Id));

            Tierheimhaus newHaus = context.Tierheimhaeuser
                .Include(h => h.Tiere)
                .First(h => h.Id.Equals(haus.Id));

            Assert.Equal(0, newKunde.Tiere.Count);
            Assert.Equal(2, newHaus.Tiere.Count);
        }
    }



}
