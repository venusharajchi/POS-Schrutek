using System;
using SPG.Venus.Tierheim.Application;
using SPG.Venus.Tierheim.Domain.Dtos;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Infrastructure;
using SPG.Venus.Tierheim.Repository;

public static class ApplicationTestFixtures
{
    public static NewKundeDto kunde1Dto() =>
        new NewKundeDto
        {
            Vorname = "John",
            Nachname = "Doe",
            Street = "Teststraße",
            Number = "123",
            City = "Teststadt",
            Country = "Testland",
            Geschlecht = Geschlecht.Mann
        };


    public static NewKundeDto kunde2Dto() =>
        new NewKundeDto
        {
            Vorname = "Jane",
            Nachname = "Xoe",
            Street = "Teststraße",
            Number = "456",
            City = "Teststadt",
            Country = "Testland",
            Geschlecht = Geschlecht.Frau
        };

    public static UpdateKundeDto kunde1UpdateDto() =>
            new UpdateKundeDto
            {
                KundeId = 1,
                Vorname = "Updated",
                Nachname = "Doe",
                Street = "Updatedstraße",
                Number = "456",
                City = "Updatedstadt",
                Country = "Updatedland",
                Geschlecht = Geschlecht.Frau
            };


    public static HundInsHeimDto hundInsHeimDto(int tierheimId) =>
        new HundInsHeimDto
        {
            TierheimId = tierheimId,
            IsBissig = true,
            Name = "Max",
            Geschlecht = Geschlecht.Mann,
            Alter = 5
        };

    public static HaustierAusHeimDto haustierAusHeimDto(int kundeId, int tierheimId) =>
        new HaustierAusHeimDto
        {
            KundeId = kundeId,
            TierheimId = tierheimId,
            MaxAlter = 10,
            Tierart = Tierart.Hund,
        };


    public static NewTierheimDto tierheimDto() =>
        new NewTierheimDto
        {
            Name = "TestTierheim",
            Street = "Teststraße",
            Number = "123",
            City = "Teststadt",
            Country = "Testland",
            StartDate = TimeSpan.FromHours(8),
            EndDate = TimeSpan.FromHours(17),
        };

    public static NewTierheimDto tierheimDto2() =>
        new NewTierheimDto
        {
            Name = "XestTierheim",
            Street = "Teststraße",
            Number = "123",
            City = "Teststadt",
            Country = "Testland",
            StartDate = TimeSpan.FromHours(8),
            EndDate = TimeSpan.FromHours(17),
        };

    public static AlleTiereZurueckBringenDto alleTiereZurueckBringenDto(int kundeId, int tierheimId) =>
        new AlleTiereZurueckBringenDto
        {
            KundeId = kundeId,
            TierheimId = tierheimId
        };

    public static NewTierheimDto invalidTierheimDto() =>
        new NewTierheimDto
        {
            Name = "", // empty name is invalid
            Street = null, // null street is invalid
            Number = "123",
            City = "Berlin",
            Country = "Germany",
            StartDate = TimeSpan.FromHours(8),
            EndDate = TimeSpan.FromHours(7),
        };

    public static HundInsHeimDto invalidHundInsHeimDto(int tierheimId) =>
        new HundInsHeimDto
        {
            TierheimId = tierheimId,
            IsBissig = true,
            Name = "",
            Geschlecht = Geschlecht.Mann,
            Alter = -1
        };

}