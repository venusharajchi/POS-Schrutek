using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Dtos;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Repository;
using SPG.Venus.Tierheim.Infrastructure;
using Xunit;
using System;
using System.Linq;
using SPG.Venus.Tierheim.Application;
using SPG.Venus.Tierheim.Domain.Interfaces;

namespace SPG.Venus.Tierheim.Test.Application
{
    public class TierheimServiceTest : IDisposable
    {
        private readonly TierheimService _tierheimService;
        private readonly TierheimContext _context;

        public TierheimServiceTest()
        {
            _context = GenerateDb();
            var tierheimRepository = new TierheimRepository(_context);
            var tierheimValidationService = new TierheimValidationService(tierheimRepository);

            _tierheimService = new TierheimService(tierheimRepository, tierheimValidationService);
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
        public void NewTierheim_CreatesNewTierheim_WhenValidDtoProvided()
        {
            // Act
            var newTierheim = _tierheimService.NewTierheim(ApplicationTestFixtures.tierheimDto());

            // Assert
            Assert.NotNull(newTierheim);
            Assert.Equal(ApplicationTestFixtures.tierheimDto().Name, newTierheim.Name);
        }



        [Fact]
        public void HundInsHeim_AddsDogToShelter_WhenValidDtoProvided()
        {
            // Arrange
            var newTierheim = _tierheimService.NewTierheim(ApplicationTestFixtures.tierheimDto());

            // Act
            var newHund = _tierheimService.HundInsHeim(ApplicationTestFixtures.hundInsHeimDto(newTierheim.Id));

            // Assert
            Assert.NotNull(newHund);
            Assert.Equal(ApplicationTestFixtures.hundInsHeimDto(newTierheim.Id).Name, newHund.Name);
        }



        [Fact]
        public void DeleteTierheim_RemovesTierheimFromDatabase_WhenValidIdProvided()
        {
            // Arrange
            var newTierheim = _tierheimService.NewTierheim(ApplicationTestFixtures.tierheimDto());

            // Act
            _tierheimService.DeleteTierheim(newTierheim.Id);

            // Assert
            Assert.Throws<ArgumentException>(() => _tierheimService.GetOne(newTierheim.Id));
        }



        [Fact]
        public void DeleteTierheim_ThrowsTierheimServiceException_WhenInvalidIdProvided()
        {
            // Arrange
            var invalidId = 999;

            // Act and Assert
            Assert.Throws<TierheimServiceException>(() => _tierheimService.DeleteTierheim(invalidId));
        }



        [Fact]
        public void NewTierheim_ThrowsTierheimServiceException_WhenInvalidDtoProvided()
        {
            // Arrange
            var invalidTierheimDto = ApplicationTestFixtures.invalidTierheimDto();

            // Act and Assert
            Assert.Throws<TierheimServiceException>(() => _tierheimService.NewTierheim(invalidTierheimDto));
        }



        [Fact]
        public void HundInsHeim_ThrowsTierheimServiceException_WhenTierheimDoesNotExist()
        {
            // Arrange
            var nonExistentTierheimId = 999;
            var hundInsHeimDto = ApplicationTestFixtures.hundInsHeimDto(nonExistentTierheimId);

            // Act and Assert
            Assert.Throws<TierheimServiceException>(() => _tierheimService.HundInsHeim(hundInsHeimDto));
        }


        [Fact]
        public void NewTierheim_ThrowsTierheimServiceException_WhenInvalidEntityProvided()
        {
            // Act and Assert
            Assert.Throws<TierheimServiceException>(() => _tierheimService.NewTierheim(ApplicationTestFixtures.invalidTierheimDto()));
        }



        [Fact]
        public void GetAll_ReturnsAllSheltersSortedAscendingByName_WhenSortParameterIsAscSort()
        {
            // Arrange
            var newTierheim1 = _tierheimService.NewTierheim(ApplicationTestFixtures.tierheimDto());
            var newTierheim2 = _tierheimService.NewTierheim(ApplicationTestFixtures.tierheimDto2());

            var currentPage = 1;
            var itemsPerPage = 10;
            var sort = "asc_sort";

            // Act
            var result = _tierheimService.GetAll(currentPage, itemsPerPage, sort);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("TestTierheim", result[0].Name);
            Assert.Equal("XestTierheim", result[1].Name);
        }



        [Fact]
        public void GetAll_ReturnsAllSheltersSortedDescendingByName_WhenSortParameterIsDescSort()
        {
            // Arrange
            var newTierheim1 = _tierheimService.NewTierheim(ApplicationTestFixtures.tierheimDto());
            var newTierheim2 = _tierheimService.NewTierheim(ApplicationTestFixtures.tierheimDto2());

            var currentPage = 1;
            var itemsPerPage = 10;
            var sort = "desc_sort";

            // Act
            var result = _tierheimService.GetAll(currentPage, itemsPerPage, sort);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("XestTierheim", result[0].Name);
            Assert.Equal("TestTierheim", result[1].Name);
        }

    }
}
