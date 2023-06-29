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
    public class KundeRepositoryTest
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
        public void GetById_ReturnsKundeById()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var repository = new KundeRepository(context);

                // Act
                Kunde? kundeBack = repository.GetById(1).SingleOrDefault();

                // Assert
                Assert.NotNull(kundeBack);
            }
        }



        [Fact]
        public void GetById_ReturnsNull_WhenKundeDoesNotExist()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var kundenId = -1;
                var repository = new KundeRepository(context);

                // Act
                var result = repository.GetById(kundenId).SingleOrDefault();

                // Assert
                Assert.Null(result);
            }
        }



        [Fact]
        public void Create_AddsNewKundeToDatabase_WhenValidEntityProvided()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var repository = new KundeRepository(context);

                var newKunde = new Kunde(Guid.NewGuid(), "John", "Doe",
                    new Addresse("Teststraße", "456", "Teststadt", "Testland"), Geschlecht.Mann);

                // Act
                repository.Create(newKunde);

                // Assert
                Assert.NotEqual(0, newKunde.Id);
            }
        }



        [Fact]
        public void Create_ThrowsRepositoryException_WhenDbUpdateExceptionOccurs()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var repository = new KundeRepository(context);

                var newKunde = new Kunde(Guid.NewGuid(), "John", "Doe",
                    new Addresse("Teststraße", "456", "Teststadt", "Testland"), Geschlecht.Mann);

                repository.Create(newKunde);

                // Act & Assert
                Assert.Throws<KundeRepositoryException>(() => repository.Create(newKunde));
            }
        }



        [Fact]
        public void Update_UpdatesExistingKundeInDatabase_WhenValidEntityProvided()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var repository = new KundeRepository(context);
                var existingKunde = repository.GetById(1).Single();

                // Act
                existingKunde.Nachname = "Suwarti";
                repository.Update(existingKunde);

                // Assert
                Assert.Equal(existingKunde.Nachname, repository.GetById(1).Single().Nachname);
            }
        }



        [Fact]
        public void Delete_RemovesKundeFromDatabase_WhenKundeExists()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var kundeId = 1;
                var repository = new KundeRepository(context);

                // Act
                repository.Delete(kundeId);

                // Assert
                Assert.Null(repository.GetById(kundeId).SingleOrDefault());
            }
        }



        [Fact]
        public void Delete_ThrowsKundeRepositoryException_WhenKundeDoesNotExist()
        {
            // Arrange
            using (var context = GenerateDb())
            {
                var kundeId = -1;
                var repository = new KundeRepository(context);

                // Act & Assert
                Assert.Throws<KundeRepositoryException>(() => repository.Delete(kundeId));
            }
        }
        
    }
}

