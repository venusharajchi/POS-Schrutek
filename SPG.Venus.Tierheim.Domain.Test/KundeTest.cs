using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Infrastructure;
using Xunit;

namespace SPG.Venus.Tierheim.Domain.Test
{
    public class KundeTest
    {
        private TierheimContext GenerateDb()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Data Source=Tierheim_Test.db");

            TierheimContext db = new TierheimContext(optionsBuilder.Options);
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
        public void Kunde_HoleKatzeAusHeim_Adds_Katze_To_Tiere_List()
        {
            // Arrange
            Tierheimhaus tierheimhaus = new Tierheimhaus(Guid.NewGuid(), "Test Tierheim", new Addresse("Musterstraße", "123", "Musterstadt", "Musterland"), TimeSpan.FromHours(8), TimeSpan.FromHours(17));
            Kunde kunde = new Kunde(Guid.NewGuid(), "John", "Doe", new Addresse("Teststraße", "456", "Teststadt", "Testland"), Geschlecht.Mann);
            Katze katze = new Katze(Guid.NewGuid(), true, "Minka", Geschlecht.Frau, 3);
            tierheimhaus.TierInsHeimBringen(katze);

            // Act
            Katze adoptedKatze = kunde.HoleKatzeAusHeim(tierheimhaus, 3);

            // Assert
            Assert.Equal(katze, adoptedKatze);
            Assert.Contains(katze, kunde.Tiere);
            Assert.DoesNotContain(katze, tierheimhaus.Tiere);
        }



        [Fact]
        public void Kunde_HoleHundAusHeim_Adds_Hund_To_Tiere_List()
        {
            // Arrange
            Tierheimhaus tierheimhaus = new Tierheimhaus(Guid.NewGuid(), "Test Tierheim", new Addresse("Musterstraße", "123", "Musterstadt", "Musterland"), TimeSpan.FromHours(8), TimeSpan.FromHours(17));
            Kunde kunde = new Kunde(Guid.NewGuid(), "John", "Doe", new Addresse("Teststraße", "456", "Teststadt", "Testland"), Geschlecht.Mann);
            Hund hund = new Hund(Guid.NewGuid(), true, "Bello", Geschlecht.Mann, 5);
            tierheimhaus.TierInsHeimBringen(hund);

            // Act
            Hund adoptedHund = kunde.HoleHundAusHeim(tierheimhaus, 5);

            // Assert
            Assert.Equal(hund, adoptedHund);
            Assert.Contains(hund, kunde.Tiere);
            Assert.DoesNotContain(hund, tierheimhaus.Tiere);
        }



        [Fact]
        public void Kunde_AlleZurueckInsHeimBringen_Clears_Tiere_List()
        {
            // Arrange
            Tierheimhaus tierheimhaus = new Tierheimhaus(Guid.NewGuid(), "Test Tierheim", new Addresse("Musterstraße", "123", "Musterstadt", "Musterland"), TimeSpan.FromHours(8), TimeSpan.FromHours(17));
            Kunde kunde = new Kunde(Guid.NewGuid(), "John", "Doe", new Addresse("Teststraße", "456", "Teststadt", "Testland"), Geschlecht.Mann);

            Katze katze = new Katze(Guid.NewGuid(), true, "Minka", Geschlecht.Frau, 5);
            Hund hund = new Hund(Guid.NewGuid(), true, "Bello", Geschlecht.Mann, 5);

            tierheimhaus.TierInsHeimBringen(katze);
            tierheimhaus.TierInsHeimBringen(hund);

            kunde.HoleKatzeAusHeim(tierheimhaus, 5);
            kunde.HoleHundAusHeim(tierheimhaus, 10);

            // Act
            kunde.AlleZurueckInsHeimBringen(tierheimhaus);

            // Assert
            Assert.Empty(kunde.Tiere);
        }



        [Fact]
        public void Kunde_TiereZaehlen_Returns_Correct_Number_Of_Tiere()
        {
            // Arrange
            Tierheimhaus tierheimhaus = new Tierheimhaus(Guid.NewGuid(), "Test Tierheim", new Addresse("Musterstraße", "123", "Musterstadt", "Musterland"), TimeSpan.FromHours(8), TimeSpan.FromHours(17));
            Kunde kunde = new Kunde(Guid.NewGuid(), "John", "Doe", new Addresse("Teststraße", "456", "Teststadt", "Testland"), Geschlecht.Mann);

            Katze katze1 = new Katze(Guid.NewGuid(), true, "Minka", Geschlecht.Frau, 3);
            Katze katze2 = new Katze(Guid.NewGuid(), true, "Luna", Geschlecht.Frau, 4);
            Hund hund = new Hund(Guid.NewGuid(), true, "Bello", Geschlecht.Mann, 5);

            tierheimhaus.TierInsHeimBringen(katze1);
            tierheimhaus.TierInsHeimBringen(katze2);
            tierheimhaus.TierInsHeimBringen(hund);

            kunde.HoleKatzeAusHeim(tierheimhaus, 5);
            kunde.HoleKatzeAusHeim(tierheimhaus, 5);
            kunde.HoleHundAusHeim(tierheimhaus, 10);

            // Act
            int count = kunde.TiereZaehlen();

            // Assert
            Assert.Equal(3, count);
        }



        [Fact]
        public void Kunde_HoleKatzeAusHeim_Throws_Exception_When_No_Suitable_Cat_Found()
        {
            // Arrange
            Tierheimhaus tierheimhaus = new Tierheimhaus(
                Guid.NewGuid(), "Test Tierheim", new Addresse("Musterstraße", "123", "Musterstadt", "Musterland"),
                TimeSpan.FromHours(8), TimeSpan.FromHours(17));

            Kunde kunde = new Kunde(Guid.NewGuid(), "John", "Doe",
                new Addresse("Teststraße", "456", "Teststadt", "Testland"), Geschlecht.Mann);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => kunde.HoleKatzeAusHeim(tierheimhaus, 1));
        }



        [Fact]
        public void Kunde_HoleHundAusHeim_Throws_Exception_When_No_Suitable_Dog_Found()
        {
            // Arrange
            Tierheimhaus tierheimhaus = new Tierheimhaus(
                Guid.NewGuid(), "Test Tierheim", new Addresse("Musterstraße", "123", "Musterstadt", "Musterland"),
                TimeSpan.FromHours(8), TimeSpan.FromHours(17));

            Kunde kunde = new Kunde(Guid.NewGuid(), "John", "Doe",
                new Addresse("Teststraße", "456", "Teststadt", "Testland"), Geschlecht.Mann);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => kunde.HoleHundAusHeim(tierheimhaus, 1));
        }


    }
}