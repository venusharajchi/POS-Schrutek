using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Application;
using SPG.Venus.Tierheim.Domain.Dtos;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Infrastructure;
using SPG.Venus.Tierheim.Repository;
using SPG.Venus.Tierheim.RepositoryTest.Helpers;

namespace SPG.Venus.Tierheim.ApplicationTest;

public class TierheimServiceTest
{
    private TierheimService InitUnitToTest(TierheimContext db)
    {
        return new TierheimService
        (
            new TierheimRepository(db),
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

    /*
    [Fact]
    public void NewTierheim_Success_Test()
    {
        // Arrange
        using (TierheimContext db = new TierheimContext(GenerateDbOptions()))
        {
            // Setup DB
            TierheimService unitToTest = InitUnitToTest(db);
            DatabaseUtilities.InitializeDatabase(db);

            // Setup Dto
            NewTierheimDto entity = new NewTierheimDto()
            {
                Name = "Test Tierheim",
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(10),
                Street = "Test Street",
                Number = "123",
                City = "Test City",
                Country = "Test Country"
            };

            // Act
            unitToTest.NewTierheim(entity);

            // Assert
            var createdTierheim = db.Tierheimhaeuser.FirstOrDefault(t => t.Name == "Test Tierheim");
            Assert.NotNull(createdTierheim);
            Assert.Equal("Test Tierheim", createdTierheim.Name);
            Assert.Equal("Test Street", createdTierheim.Adresse.Strasse);
            Assert.Equal("123", createdTierheim.Adresse.Hausnummer);
            Assert.Equal("Test City", createdTierheim.Adresse.Stadt);
            Assert.Equal("Test Country", createdTierheim.Adresse.Land);
        }
    }


    [Fact]
    public void NewTierheim_StartDateInFuture_ThrowsException_Test()
    {
        // Arrange
        using (TierheimContext db = new TierheimContext(GenerateDbOptions()))
        {
            TierheimService unitToTest = InitUnitToTest(db);

            NewTierheimDto entity = new NewTierheimDto()
            {
                Name = "Test Tierheim",
                StartDate = DateTime.Now.AddDays(10), // StartDate in the future
                EndDate = DateTime.Now.AddDays(20),
                Street = "Test Street",
                Number = "123",
                City = "Test City",
                Country = "Test Country"
            };

            // Act + Assert
            ServiceException ex = Assert.Throws<ServiceException>(() => unitToTest.NewTierheim(entity));
            Assert.Equal("Start date muss in Vergangheit sein!", ex.Message);
        }
    }



    [Fact]
    public void NewTierheim_EndDateInPast_ThrowsException_Test()
    {
        // Arrange
        using (TierheimContext db = new TierheimContext(GenerateDbOptions()))
        {
            TierheimService unitToTest = InitUnitToTest(db);

            NewTierheimDto entity = new NewTierheimDto()
            {
                Name = "Test Tierheim",
                StartDate = DateTime.Now.AddDays(-20),
                EndDate = DateTime.Now.AddDays(-10), // EndDate in the past
                Street = "Test Street",
                Number = "123",
                City = "Test City",
                Country = "Test Country"
            };

            // Act + Assert
            ServiceException ex = Assert.Throws<ServiceException>(() => unitToTest.NewTierheim(entity));
            Assert.Equal("End date muss in Zukunft sein!", ex.Message);
        }
    }



    [Fact]
    public void HundInsHeim_Success_Test()
    {
        // Arrange
        using (TierheimContext db = new TierheimContext(GenerateDbOptions()))
        {
            // Setup DB
            TierheimService unitToTest = InitUnitToTest(db);
            DatabaseUtilities.InitializeDatabase(db);

            // Assuming there is already a Tierheimhaus in the database
            var tierheimhaus = db.Tierheimhaeuser.First();

            // Set up Hund DTO
            HundInsHeimDto entity = new HundInsHeimDto()
            {
                TierhausName = tierheimhaus.Name,
                Name = "Test Hund",
                IsBissig = false,
                Geschlecht = Geschlecht.Mann,
                Alter = 5
            };

            // Act
            unitToTest.HundInsHeim(entity);

            // Assert
            var hund = db.Haustiere.OfType<Hund>().FirstOrDefault(h => h.Name == "Test Hund");
            Assert.NotNull(hund);
            Assert.Equal("Test Hund", hund.Name);
            Assert.Equal(Geschlecht.Mann, hund.Geschlecht);
            Assert.Equal(5, hund.Alter);
            Assert.False(hund.IsBissig);
            Assert.Contains(hund, tierheimhaus.Tiere);
        }
    }



    [Fact]
    public void KatzeInsHeim_Success_Test()
    {
        // Arrange
        using (TierheimContext db = new TierheimContext(GenerateDbOptions()))
        {
            // Setup DB
            TierheimService unitToTest = InitUnitToTest(db);
            DatabaseUtilities.InitializeDatabase(db);

            // Assuming there is already a Tierheimhaus in the database
            var tierheimhaus = db.Tierheimhaeuser.First();

            // Set up Katze DTO
            KatzeInsHeimDto entity = new KatzeInsHeimDto()
            {
                TierhausName = tierheimhaus.Name,
                Name = "Test Katze",
                IsAnschmiegsam = true,
                Geschlecht = Geschlecht.Frau,
                Alter = 3
            };

            // Act
            unitToTest.KatzeInsHeim(entity);

            // Assert
            var katze = db.Haustiere.OfType<Katze>().FirstOrDefault(k => k.Name == "Test Katze");
            Assert.NotNull(katze);
            Assert.Equal("Test Katze", katze.Name);
            Assert.Equal(Geschlecht.Frau, katze.Geschlecht);
            Assert.Equal(3, katze.Alter);
            Assert.True(katze.IsAnschmiegsam);
            Assert.Contains(katze, tierheimhaus.Tiere);
        }
    }



    [Fact]
    public void HundInsHeim_TierheimhausNotFound_Test()
    {
        // Arrange
        using (TierheimContext db = new TierheimContext(GenerateDbOptions()))
        {
            // Setup DB and Service
            TierheimService unitToTest = InitUnitToTest(db);
            DatabaseUtilities.InitializeDatabase(db);

            // Set up Hund DTO
            HundInsHeimDto entity = new HundInsHeimDto()
            {
                TierhausName = "NonExistentTierheim",
                Name = "Test Hund",
                IsBissig = false,
                Geschlecht = Geschlecht.Mann,
                Alter = 5
            };

            // Assert
            var ex = Assert.Throws<ServiceException>(() => unitToTest.HundInsHeim(entity));
            Assert.Equal("HundInsHeim ist fehlgeschlagen, der arme Hund!", ex.Message);
        }
    }



    [Fact]
    public void KatzeInsHeim_TierheimhausNotFound_Test()
    {
        // Arrange
        using (TierheimContext db = new TierheimContext(GenerateDbOptions()))
        {
            // Setup DB and Service
            TierheimService unitToTest = InitUnitToTest(db);
            DatabaseUtilities.InitializeDatabase(db);

            // Set up Katze DTO
            KatzeInsHeimDto entity = new KatzeInsHeimDto()
            {
                TierhausName = "NonExistentTierheim",
                Name = "Test Katze",
                IsAnschmiegsam = true,
                Geschlecht = Geschlecht.Frau,
                Alter = 3
            };

            // Assert
            var ex = Assert.Throws<ServiceException>(() => unitToTest.KatzeInsHeim(entity));
            Assert.Equal("KatzeInsHeim ist fehlgeschlagen, die arme Katze!", ex.Message);
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
            TierheimService unitToTest = InitUnitToTest(db);

            // Act
            List<Tierheimhaus> result = unitToTest.GetAll(1, 3, "asc_sort");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.True(string.Compare(result[0].Name, result[1].Name) < 0);
            Assert.True(string.Compare(result[1].Name, result[2].Name) < 0);
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
            TierheimService unitToTest = InitUnitToTest(db);

            // Act
            List<Tierheimhaus> result = unitToTest.GetAll(1, 3, "desc_sort");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.True(string.Compare(result[0].Name, result[1].Name) > 0);
            Assert.True(string.Compare(result[1].Name, result[2].Name) > 0);
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
            TierheimService unitToTest = InitUnitToTest(db);

            // Act
            var result = unitToTest.GetAll(100, 3, "asc_sort");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
    */

}
