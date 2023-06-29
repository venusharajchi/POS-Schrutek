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

namespace SPG.Venus.Tierheim.Test.Application
{
    public class KundenServiceTest
    {
        private readonly KundenService _kundenService;

        public KundenServiceTest()
        {
            using (var context = GenerateDb())
            {
                var repository = new KundeRepository(context);
                var validationService = new KundeValidationService(repository);
                _kundenService = new KundenService(repository, validationService, null, null);
            }
        }


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
        public void NewKunde_AddsNewKundeToDatabase_WhenValidEntityProvided()
        {
            // Arrange
            var newKunde = new NewKundeDto
            {
                Vorname = "John",
                Nachname = "Doe",
                Street = "Teststraße",
                Number = "123",
                City = "Teststadt",
                Country = "Testland",
                Geschlecht = Geschlecht.Mann
            };

            // Act
            _kundenService.NewKunde(newKunde);

            // Assert
            using (var context = GenerateDb())
            {
                var repository = new KundeRepository(context);
                var kunden = repository.GetAll();
                Assert.NotEmpty(kunden);
            }
        }
    }
}
