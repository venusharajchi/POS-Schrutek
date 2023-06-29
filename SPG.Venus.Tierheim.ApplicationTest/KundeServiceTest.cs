using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Repository;
using SPG.Venus.Tierheim.Infrastructure;
using Xunit;
using SPG.Venus.Tierheim.Domain.Exceptions;
using System.Text;
using System.Linq;
using SPG.Venus.Tierheim.Application;
using SPG.Venus.Tierheim.Domain.Dtos;
using SPG.Venus.Tierheim.Domain.Interfaces;

namespace SPG.Venus.Tierheim.Test.Application
{
    public class KundenServiceTest : IDisposable
    {
        private readonly KundenService _kundenService;
        private readonly TierheimService _tierheimService;
        private readonly TierheimContext _context;

        public KundenServiceTest()
        {
            _context = GenerateDb();
            var kundenRepository = new KundeRepository(_context);
            var tierheimRepository = new TierheimRepository(_context);

            var kundeValidationService = new KundeValidationService(kundenRepository);
            var tierheimValidationService = new TierheimValidationService(tierheimRepository);

            _kundenService = new KundenService(
                kundenRepository, kundeValidationService,
                tierheimRepository, tierheimValidationService);

            _tierheimService = new TierheimService(
                tierheimRepository, tierheimValidationService);
        }


        public TierheimContext GenerateDb()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Data Source=Tierheim_Test.db");

            TierheimContext db = new TierheimContext(optionsBuilder.Options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }


        public void Dispose()
        {
            _context.Dispose();
        }



        [Fact]
        public void NewKunde_AddsNewKundeToDatabase_WhenValidEntityProvided()
        {
            // Act
            var savedKunde = _kundenService.NewKunde(ApplicationTestFixtures.kunde1Dto());

            // Assert
            Assert.NotEqual(0, savedKunde.Id);
            Assert.NotNull(_kundenService.GetOne(savedKunde.Id));
        }



        [Fact]
        public void UpdateKunde_UpdatesKundeInDatabase_WhenValidEntityProvided()
        {

            // Arrange
            var savedKunde = _kundenService.NewKunde(ApplicationTestFixtures.kunde1Dto());

            // Act
            var updatedKunde = _kundenService.UpdateKunde(ApplicationTestFixtures.kunde1UpdateDto());

            // Assert
            Assert.Equal(_kundenService.GetOne(savedKunde.Id).Nachname, ApplicationTestFixtures.kunde1UpdateDto().Nachname);
        }



        [Fact]
        public void HoleHaustierAusHeim_RemovesPetFromHome_WhenValidDtoProvided()
        {
            // Arrange
            var newKunde = _kundenService.NewKunde(ApplicationTestFixtures.kunde1Dto());
            var newTierheim = _tierheimService.NewTierheim(ApplicationTestFixtures.tierheimDto());

            var newHund = _tierheimService.HundInsHeim(ApplicationTestFixtures.hundInsHeimDto(newTierheim.Id));
            var haustierAusHeimDto = ApplicationTestFixtures.haustierAusHeimDto(newKunde.Id, newTierheim.Id);

            // Act
            _kundenService.HoleHaustierAusHeim(haustierAusHeimDto);

            // Assert
            var kundeBack = _kundenService.GetOne(newKunde.Id);
            var tierheimBack = _tierheimService.GetOne(newTierheim.Id);
            Assert.Single(kundeBack.Tiere);
            Assert.Empty(tierheimBack.Tiere);
        }



        [Fact]
        public void AlleTiereZurueckBringen_MovesAllPetsFromCustomerToShelter_WhenValidDtoProvided()
        {
            // Arrange
            var newTierheim = _tierheimService.NewTierheim(ApplicationTestFixtures.tierheimDto());
            var newKunde = _kundenService.NewKunde(ApplicationTestFixtures.kunde1Dto());
            var newHund1 = _tierheimService.HundInsHeim(ApplicationTestFixtures.hundInsHeimDto(newTierheim.Id));
            var newHund2 = _tierheimService.HundInsHeim(ApplicationTestFixtures.hundInsHeimDto(newTierheim.Id));

            _kundenService.HoleHaustierAusHeim(ApplicationTestFixtures.haustierAusHeimDto(newKunde.Id, newTierheim.Id));
            _kundenService.HoleHaustierAusHeim(ApplicationTestFixtures.haustierAusHeimDto(newKunde.Id, newTierheim.Id));

            // Act
            _kundenService.AlleTiereZurueckBringen(ApplicationTestFixtures.alleTiereZurueckBringenDto(newKunde.Id, newTierheim.Id));

            // Assert
            var hund1NachDerAktion = _tierheimService.GetOne(newTierheim.Id);
            var hund2NachDerAktion = _tierheimService.GetOne(newTierheim.Id);
            Assert.NotNull(hund1NachDerAktion);
            Assert.NotNull(hund2NachDerAktion);
        }



        [Fact]
        public void DeleteKunde_RemovesKundeFromDatabase_WhenValidIdProvided()
        {
            // Arrange
            var newKunde = _kundenService.NewKunde(ApplicationTestFixtures.kunde1Dto());

            // Act
            _kundenService.DeleteKunde(newKunde.Id);

            // Assert
            Assert.Throws<ArgumentException>(() => _kundenService.GetOne(newKunde.Id));
        }



        [Fact]
        public void DeleteKunde_ThrowsKundeServiceException_WhenInvalidIdProvided()
        {
            // Arrange
            var invalidId = 999;

            // Act and Assert
            Assert.Throws<KundeServiceException>(() => _kundenService.DeleteKunde(invalidId));
        }


        [Fact]
        public void GetAll_ReturnsAllCustomersSortedAscendingByName_WhenSortParameterIsAscSort()
        {
            // Arrange
            var newKunde1 = _kundenService.NewKunde(ApplicationTestFixtures.kunde1Dto());
            var newKunde2 = _kundenService.NewKunde(ApplicationTestFixtures.kunde2Dto());


            var currentPage = 1;
            var itemsPerPage = 10;
            var sort = "asc_sort";

            // Act
            var result = _kundenService.GetAll(currentPage, itemsPerPage, sort);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Doe", result[0].Nachname);
            Assert.Equal("Xoe", result[1].Nachname);
        }



        [Fact]
        public void GetAll_ReturnsAllCustomersSortedDescendingByName_WhenSortParameterIsDescSort()
        {
            // Arrange
            var newKunde1 = _kundenService.NewKunde(ApplicationTestFixtures.kunde1Dto());
            var newKunde2 = _kundenService.NewKunde(ApplicationTestFixtures.kunde2Dto());

            var currentPage = 1;
            var itemsPerPage = 10;
            var sort = "desc_sort";

            // Act
            var result = _kundenService.GetAll(currentPage, itemsPerPage, sort);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Xoe", result[0].Nachname);
            Assert.Equal("Doe", result[1].Nachname);
        }
        
    }
}
