using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Repository;
using SPG.Venus.Tierheim.Infrastructure;
using Xunit;
using SPG.Venus.Tierheim.Domain.Exceptions;
using System.Text;
using System.Linq;

namespace SPG.Venus.Tierheim.Test.Repository
{
    public class TierheimRepositoryTest
    {
        private TierheimContext GenerateDb()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Data Source=Tierheim_Test.db");

            TierheimContext db = new TierheimContext(optionsBuilder.Options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Seed();
            return db;
        }



        [Fact]
        public void GetById_ReturnsTierheimById()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var tierheimId = 1;
                var repository = new TierheimRepository(context);

                // Act
                Tierheimhaus? tierheimBack = repository.GetById(tierheimId).SingleOrDefault();

                // Assert
                Assert.NotNull(tierheimBack);
            }
        }



        [Fact]
        public void GetById_ReturnsNull_WhenTierheimDoesNotExist()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var tierheimId = -1;
                var repository = new TierheimRepository(context);

                // Act
                var result = repository.GetById(tierheimId).SingleOrDefault();

                // Assert
                Assert.Null(result);
            }
        }



        [Fact]
        public void Create_AddsNewTierheimToDatabase_WhenValidEntityProvided()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var repository = new TierheimRepository(context);

                Tierheimhaus newTierheim = new Tierheimhaus(Guid.NewGuid(),
                    "Test Tierheim", new Addresse("Musterstraße", "123", "Musterstadt", "Musterland"),
                    TimeSpan.FromHours(8), TimeSpan.FromHours(17));

                // Act
                repository.Create(newTierheim);

                // Assert
                Assert.NotEqual(0, newTierheim.Id);
            }
        }



        [Fact]
        public void Create_ThrowsRepositoryException_WhenDbUpdateExceptionOccurs()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var repository = new TierheimRepository(context);

                Tierheimhaus newTierheim = new Tierheimhaus(Guid.NewGuid(),
                    "Test Tierheim", new Addresse("Musterstraße", "123", "Musterstadt", "Musterland"),
                    TimeSpan.FromHours(8), TimeSpan.FromHours(17));

                // Act
                repository.Create(newTierheim);

                // Act & Assert
                Assert.Throws<TierheimRepositoryException>(() => repository.Create(newTierheim));
            }
        }



        [Fact]
        public void Delete_RemovesTierheimFromDatabase_WhenTierheimExists()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var tierheimId = 1;
                var repository = new TierheimRepository(context);

                // Act
                repository.Delete(tierheimId);

                // Assert
                Assert.Null(repository.GetById(tierheimId).SingleOrDefault());
            }
        }



        [Fact]
        public void Delete_ThrowsTierheimRepositoryException_WhenTierheimDoesNotExist()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var tierheimId = -1;
                var repository = new TierheimRepository(context);

                // Act & Assert
                Assert.Throws<TierheimRepositoryException>(() => repository.Delete(tierheimId));
            }
        }
    }
}



