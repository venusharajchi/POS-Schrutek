using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Application;
using SPG.Venus.Tierheim.Domain.Dtos;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Interfaces;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Infrastructure;
using SPG.Venus.Tierheim.Repository;
using SPG.Venus.Tierheim.RepositoryTest.Helpers;

namespace SPG.Venus.Tierheim.ApplicationTest
{
    public class KundeServiceTest
    {
        private KundeService InitUnitToTest(TierheimContext db)
        {
            return new KundeService
            (
                new KundeRepository(db),
                new TierheimRepository(db),
                new RepositoryBase<Kunde>(db),
                new RepositoryBase<Tierheimhaus>(db)
            );
        }


        private DbContextOptions GenerateDbOptions()
        {
            SqliteConnection connection = new SqliteConnection("Data Source = :memory:");
            connection.Open();

            DbContextOptionsBuilder options = new DbContextOptionsBuilder();
            options.UseSqlite(connection);
            return options.Options;
        }



        [Fact]
        public void NewKunde_Success_Test()
        {
            // Arrange
            using (TierheimContext db = new TierheimContext(GenerateDbOptions()))
            {
                // Setup DB
                KundeService unitToTest = InitUnitToTest(db);
                DatabaseUtilities.InitializeDatabase(db);

                // Setup Dto
                NewKundeDto entity = new NewKundeDto()
                {
                    Vorname = "Test Vorname",
                    Nachname = "Test Nachname",
                    Street = "Test Street",
                    Number = "123",
                    City = "Test City",
                    Country = "Test Country",
                    Geschlecht = Geschlecht.Frau
                };

                // Act
                unitToTest.NewKunde(entity);

                // Assert
                var createdKunde = db.Kunden.FirstOrDefault(t => t.Vorname == "Test Vorname" && t.Nachname == "Test Nachname");
                Assert.NotNull(createdKunde);
                Assert.Equal("Test Vorname", createdKunde.Vorname);
                Assert.Equal("Test Nachname", createdKunde.Nachname);
                Assert.Equal("Test Street", createdKunde.Adresse.Strasse);
                Assert.Equal("123", createdKunde.Adresse.Hausnummer);
                Assert.Equal("Test City", createdKunde.Adresse.Stadt);
                Assert.Equal("Test Country", createdKunde.Adresse.Land);
                Assert.Equal(Geschlecht.Frau, createdKunde.Geschlecht);
            }
        }




        [Fact]
        public void GetAll_AscendingSortAndPaging_Test()
        {
            // Arrange
            using (TierheimContext db = new TierheimContext(GenerateDbOptions()))
            {
                // Setup DB
                DatabaseUtilities.InitializeDatabase(db);
                KundeService unitToTest = InitUnitToTest(db);

                // Act
                var result = unitToTest.GetAll(1, 3, "asc_sort");

                // Assert
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);
                Assert.True(string.Compare(result[0].Nachname, result[1].Nachname) < 0);
                Assert.True(string.Compare(result[1].Nachname, result[2].Nachname) < 0);
            }
        }



        [Fact]
        public void GetAll_DescendingSortAndPaging_Test()
        {
            // Arrange
            using (TierheimContext db = new TierheimContext(GenerateDbOptions()))
            {
                // Setup DB
                DatabaseUtilities.InitializeDatabase(db);
                KundeService unitToTest = InitUnitToTest(db);

                // Act
                var result = unitToTest.GetAll(1, 3, "desc_sort");

                // Assert
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);
                Assert.True(string.Compare(result[0].Nachname, result[1].Nachname) > 0);
                Assert.True(string.Compare(result[1].Nachname, result[2].Nachname) > 0);
            }
        }



        [Fact]
        public void GetAll_PageOutOfBounds_Test()
        {
            // Arrange
            using (TierheimContext db = new TierheimContext(GenerateDbOptions()))
            {
                // Setup DB
                DatabaseUtilities.InitializeDatabase(db);
                KundeService unitToTest = InitUnitToTest(db);

                // Act
                var result = unitToTest.GetAll(100, 3, "asc_sort");

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }



        [Fact]
        public void HoleHundAusHeim_Success_Test()
        {
            // Arrange
            using (var db = new TierheimContext(GenerateDbOptions()))
            {
                    // Setup DB
                KundeService unitToTest = InitUnitToTest(db);
                DatabaseUtilities.InitializeDatabase(db);

                    // Get the first Kunde, Tierheim, and Hund in the database
                Guid kundenGuid = db.Kunden.First().Guid;
                string tierheimName = db.Tierheimhaeuser.First().Name;

                Tierheimhaus tierheim = db.Tierheimhaeuser.First(h => h.Name.Equals(tierheimName));
                Kunde kunde = db.Kunden.First(k => k.Guid.Equals(kundenGuid));

                    // Add Hund to Tierheim
                Hund hund = new Hund(Guid.NewGuid(), isBissig: true, "Belfi", Geschlecht.Mann, alter: 5);
                tierheim.TierInsHeimBringen(hund);
                db.SaveChanges();


                // Act
                    // Hunde vorher?
                int initialHundCountInTierheimhaus = db.Tierheimhaeuser.First(h => h.Name.Equals(tierheimName)).TiereZaehlen();
                int initialHundCountInKunde = db.Kunden.First(k => k.Guid.Equals(kundenGuid)).TiereZaehlen();
                    // Hole Hund aus heim
                unitToTest.HoleHundAusHeim(kundenGuid, tierheimName, 5);
                    // Hunde nacher?
                int finalHundCountInTierheimhaus = tierheim.TiereZaehlen();
                int finalHundCountInKunde = kunde.TiereZaehlen();

                // Assert
                Assert.Equal(initialHundCountInTierheimhaus - 1, finalHundCountInTierheimhaus);
                Assert.Equal(initialHundCountInKunde + 1, finalHundCountInKunde);
            }
        }



    }
}

