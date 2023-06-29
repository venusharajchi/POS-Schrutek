using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Infrastructure;
using Xunit.Abstractions;
using SPG.Venus.Tierheim.Domain.Model;
using Bogus.DataSets;

namespace SPG.Venus.Tierheim.Domain.Test;

public class TierheimTest
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
    public void Tierheimhaus_Constructor_Creates_With_Valid_Inputs()
    {
        // Arrange
        string name = "Test Tierheim";
        var adresse = new Addresse("Musterstraße", "123", "Musterstadt", "Musterland");
        var oeffungszeitVon = TimeSpan.FromHours(8);
        var oeffungszeitBis = TimeSpan.FromHours(17);

        // Act
        var tierheimhaus = new Tierheimhaus(Guid.NewGuid(), name, adresse, oeffungszeitVon, oeffungszeitBis);

        // Assert
        Assert.NotNull(tierheimhaus);
    }



    [Fact]
    public void Tierheimhaus_Constructor_Throws_When_OeffungszeitBis_Is_Before_OeffungszeitVon()
    {
        // Arrange
        string name = "Test Tierheim";
        var adresse = new Addresse("Musterstraße", "123", "Musterstadt", "Musterland");
        var oeffungszeitVon = TimeSpan.FromHours(17);
        var oeffungszeitBis = TimeSpan.FromHours(8);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Tierheimhaus(Guid.NewGuid(), name, adresse, oeffungszeitVon, oeffungszeitBis));
    }



    [Fact]
    public void Tierheimhaus_TierInsHeimBringen_Adds_Hund_To_Tiere_List()
    {
        // Arrange
        var guid = Guid.NewGuid();
        string name = "Test Tierheim";
        var adresse = new Addresse("Musterstraße", "123", "Musterstadt", "Musterland");
        var oeffungszeitVon = TimeSpan.FromHours(8);
        var oeffungszeitBis = TimeSpan.FromHours(17);

        var tierheimhaus = new Tierheimhaus(guid, name, adresse, oeffungszeitVon, oeffungszeitBis);
        var hund = new Hund(Guid.NewGuid(), isBissig: true, "Belfi", Geschlecht.Mann, alter: 5);

        // Act
        tierheimhaus.TierInsHeimBringen(hund);

        // Assert
        Assert.Contains(hund, tierheimhaus.Tiere);
    }



    [Fact]
    public void Tierheimhaus_TierInsHeimBringen_Adds_Katze_To_Tiere_List()
    {
        // Arrange
        string name = "Test Tierheim";
        var adresse = new Addresse("Musterstraße", "123", "Musterstadt", "Musterland");
        var oeffungszeitVon = TimeSpan.FromHours(8);
        var oeffungszeitBis = TimeSpan.FromHours(17);

        var tierheimhaus = new Tierheimhaus(Guid.NewGuid(), name, adresse, oeffungszeitVon, oeffungszeitBis);
        var katze = new Katze(Guid.NewGuid(), isAnschmiegsam: true, "Dora", Geschlecht.Frau, alter: 1);

        // Act
        tierheimhaus.TierInsHeimBringen(katze);

        // Assert
        Assert.Contains(katze, tierheimhaus.Tiere);
    }



    [Fact]
    public void Tierheimhaus_KatzeMitnehmen_Removes_Katze_From_Tiere_List()
    {
        // Arrange
        string name = "Test Tierheim";
        var adresse = new Addresse("Musterstraße", "123", "Musterstadt", "Musterland");
        var oeffungszeitVon = TimeSpan.FromHours(8);
        var oeffungszeitBis = TimeSpan.FromHours(17);

        var tierheimhaus = new Tierheimhaus(Guid.NewGuid(), name, adresse, oeffungszeitVon, oeffungszeitBis);
        var katze = new Katze(Guid.NewGuid(), isAnschmiegsam: true, "Dora", Geschlecht.Frau, alter: 1);
        tierheimhaus.TierInsHeimBringen(katze);

        // Act
        var returnedKatze = tierheimhaus.KatzeMitnehmen(2);

        // Assert
        Assert.DoesNotContain(returnedKatze, tierheimhaus.Tiere);
    }



    [Fact]
    public void Tierheimhaus_HundMitnehmen_Removes_Hund_From_Tiere_List()
    {
        // Arrange
        string name = "Test Tierheim";
        var adresse = new Addresse("Musterstraße", "123", "Musterstadt", "Musterland");
        var oeffungszeitVon = TimeSpan.FromHours(8);
        var oeffungszeitBis = TimeSpan.FromHours(17);

        var tierheimhaus = new Tierheimhaus(Guid.NewGuid(), name, adresse, oeffungszeitVon, oeffungszeitBis);
        var hund = new Hund(Guid.NewGuid(), isBissig: true, "Belfi", Geschlecht.Mann, alter: 5);
        tierheimhaus.TierInsHeimBringen(hund);

        // Act
        var returnedHund = tierheimhaus.HundMitnehmen(5);

        // Assert
        Assert.DoesNotContain(returnedHund, tierheimhaus.Tiere);
    }



    [Fact]
    public void Tierheimhaus_AlleTiereImpfenLassen_Sets_IsGeimpft_To_True_For_All_Tiere()
    {
        // Arrange
        string name = "Test Tierheim";
        var adresse = new Addresse("Musterstraße", "123", "Musterstadt", "Musterland");
        var oeffungszeitVon = TimeSpan.FromHours(8);
        var oeffungszeitBis = TimeSpan.FromHours(17);

        var tierheimhaus = new Tierheimhaus(Guid.NewGuid(), name, adresse, oeffungszeitVon, oeffungszeitBis);
        var hund = new Katze(Guid.NewGuid(), isAnschmiegsam: true, "Dora", Geschlecht.Frau, alter: 1);
        var katze = new Katze(Guid.NewGuid(), isAnschmiegsam: true, "Dora", Geschlecht.Frau, alter: 1);
        tierheimhaus.TierInsHeimBringen(hund);
        tierheimhaus.TierInsHeimBringen(katze);

        // Act
        tierheimhaus.AlleTiereImpfenLassen();

        // Assert
        Assert.True(hund.IsGeimpft);
        Assert.True(katze.IsGeimpft);
    }



    [Fact]
    public void Tierheimhaus_Oeffungszeit_Sets_Correctly_With_Valid_Inputs()
    {
        // Arrange
        string name = "Test Tierheim";
        var adresse = new Addresse("Musterstraße", "123", "Musterstadt", "Musterland");
        var oeffungszeitVon = TimeSpan.FromHours(8);
        var oeffungszeitBis = TimeSpan.FromHours(17);

        var tierheimhaus = new Tierheimhaus(Guid.NewGuid(), name, adresse, oeffungszeitVon, oeffungszeitBis);

        // Act
        var newOeffungszeitVon = TimeSpan.FromHours(7);
        var newOeffungszeitBis = TimeSpan.FromHours(18);
        tierheimhaus.OeffungszeitVon = newOeffungszeitVon;
        tierheimhaus.OeffungszeitBis = newOeffungszeitBis;

        // Assert
        Assert.Equal(newOeffungszeitVon, tierheimhaus.OeffungszeitVon);
        Assert.Equal(newOeffungszeitBis, tierheimhaus.OeffungszeitBis);
    }



    [Fact]
    public void Tierheimhaus_Oeffungszeit_Throws_When_OeffungszeitBis_Is_Before_OeffungszeitVon()
    {
        // Arrange
        string name = "Test Tierheim";
        var adresse = new Addresse("Musterstraße", "123", "Musterstadt", "Musterland");
        var oeffungszeitVon = TimeSpan.FromHours(8);
        var oeffungszeitBis = TimeSpan.FromHours(17);

        var tierheimhaus = new Tierheimhaus(Guid.NewGuid(), name, adresse, oeffungszeitVon, oeffungszeitBis);

        // Act & Assert
        var newOeffungszeitVon = TimeSpan.FromHours(17);
        var newOeffungszeitBis = TimeSpan.FromHours(8);
        Assert.Throws<ArgumentException>(() => tierheimhaus.OeffungszeitVon = newOeffungszeitVon);
        Assert.Throws<ArgumentException>(() => tierheimhaus.OeffungszeitBis = newOeffungszeitBis);
    }



    [Fact]
    public void Tierheimhaus_TiereZaehlen_Returns_Correct_Count()
    {
        // Arrange
        Tierheimhaus tierheimhaus = new Tierheimhaus(Guid.NewGuid(), "Test Tierheim",
            new Addresse("Musterstraße", "123", "Musterstadt", "Musterland"),
            TimeSpan.FromHours(8), TimeSpan.FromHours(17));

        Hund hund1 = new Hund(Guid.NewGuid(), isBissig: true, "Belfi", Geschlecht.Mann, alter: 5);
        Hund hund2 = new Hund(Guid.NewGuid(), isBissig: true, "Rex", Geschlecht.Mann, alter: 3);
        Katze katze1 = new Katze(Guid.NewGuid(), isAnschmiegsam: true, "Dora", Geschlecht.Frau, alter: 2);
        Katze katze2 = new Katze(Guid.NewGuid(), isAnschmiegsam: true, "Whiskers", Geschlecht.Mann, alter: 4);

        tierheimhaus.TierInsHeimBringen(hund1);
        tierheimhaus.TierInsHeimBringen(hund2);
        tierheimhaus.TierInsHeimBringen(katze1);
        tierheimhaus.TierInsHeimBringen(katze2);

        // Act
        int count = tierheimhaus.TiereZaehlen();

        // Assert
        Assert.Equal(4, count);
    }



    [Fact]
    public void Tierheimhaus_PersonalAnstellen_Adds_Personal_To_Personal_List()
    {
        // Arrange
        Tierheimhaus tierheimhaus = new Tierheimhaus(Guid.NewGuid(), "Test Tierheim", new Addresse("Musterstraße", "123", "Musterstadt", "Musterland"), TimeSpan.FromHours(8), TimeSpan.FromHours(17));

        Personal personal = new Personal(Guid.NewGuid(), "John", "Doe", Geschlecht.Mann, "1234567890");

        // Act
        tierheimhaus.PersonalAnstellen(personal);

        // Assert
        Assert.Contains(personal, tierheimhaus.Personal);
        Assert.Equal(1, tierheimhaus.Personal.Count);
        Assert.Equal(personal.TierheimNavigation, tierheimhaus); // Beidseitige ?
    }



    [Fact]
    public void Tierheimhaus_PersonalAnstellen_Throws_Exception_When_Personal_Already_Employed()
    {
        // Arrange
        Tierheimhaus tierheimhaus = new Tierheimhaus(Guid.NewGuid(),
            "Test Tierheim", new Addresse("Musterstraße", "123", "Musterstadt", "Musterland"),
            TimeSpan.FromHours(8), TimeSpan.FromHours(17));

        Personal personal1 = new Personal(Guid.NewGuid(), "John", "Doe", Geschlecht.Mann, "1234567890");
        Personal personal2 = new Personal(Guid.NewGuid(), "Jane", "Smith", Geschlecht.Frau, "0987654321");

        tierheimhaus.PersonalAnstellen(personal1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => tierheimhaus.PersonalAnstellen(personal1)); // Dieselbe Person
        Assert.Throws<ArgumentException>(() => tierheimhaus.PersonalAnstellen(personal2)); // Gleiche ID
    }



    [Fact]
    public void Tierheimhaus_PersonalAnstellen_Throws_Exception_When_Too_Many_Personal_Employed()
    {
        // Arrange
        Tierheimhaus tierheimhaus = new Tierheimhaus(Guid.NewGuid(),
            "Test Tierheim", new Addresse("Musterstraße", "123", "Musterstadt", "Musterland"),
            TimeSpan.FromHours(8), TimeSpan.FromHours(17));

        for (int i = 0; i < 3; i++)
        {
            Personal personal = new Personal(Guid.NewGuid(), $"Person {i}", $"Lastname {i}", Geschlecht.Mann, $"{i}");
            tierheimhaus.PersonalAnstellen(personal);
        }

        Personal extraPersonal = new Personal(Guid.NewGuid(), "Extra", "Person", Geschlecht.Frau, "Extra123");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => tierheimhaus.PersonalAnstellen(extraPersonal));
    }

}
