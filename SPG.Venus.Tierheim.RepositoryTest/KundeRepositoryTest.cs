using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Interfaces;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Infrastructure;
using SPG.Venus.Tierheim.Repository;
using SPG.Venus.Tierheim.Repository.Tierheim;
using SPG.Venus.Tierheim.RepositoryTest.Helpers;
using System;
using System.Security.Cryptography;
using Xunit.Abstractions;

namespace SPG.Venus.Tierheim.RepositoryTest
{
    public class TierheimRepositoryTest
    {
        private readonly Mock<TierheimContext> _db = new Mock<TierheimContext>();
        private readonly IReadOnlyRepositoryBase<Tierheimhaus> _unitToTest;

        private readonly ITestOutputHelper output;


        public TierheimRepositoryTest(ITestOutputHelper output)
        {
            IReadOnlyRepositoryBase<Tierheimhaus> _unitToTest = new RepositoryBase<Tierheimhaus>(_db.Object);

            this.output = output;
        }


        // SUCESS TEST
        [Fact]
        public void Create_Success_Test()
        {
            // Arrange (Enty, DB)
            using (TierheimContext db = new TierheimContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);

                Addresse adresse = new Addresse("andere strasse", "10", "linz", "oesterreich");
                Kunde kunde = new Kunde(Guid.NewGuid(), "Venus", "Harajchi", adresse, Geschlecht.Frau);

                var kundeRepository = new KundeRepository(db);
                var actual = kundeRepository.GetAll().Count();

                // Act
                new KundeRepository(db).Create(kunde);

                // Assert
                var expected = kundeRepository.GetAll().Count();
                Assert.Equal(actual+1, expected);
            }
        }


        // FAILED TEST
        [Fact]
        public void Create_KundeRepositoryCreateException_Test()
        {
            // Arrange (Enty, DB)
            using (TierheimContext db = new TierheimContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);

                Kunde kunde = new Kunde(Guid.NewGuid(), "Venus", "Harajchi", null, Geschlecht.Frau);

                // Act & Assert
                Assert.Throws<RepositoryException>(() => new KundeRepository(db).Create(kunde));
            }
        }




        // SUCESS TEST
        [Fact]
        public void Haustier_GetByGuid_Success_Test()
        {
            // Arrange
            using (TierheimContext db = new TierheimContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);

                    // Haustier
                Guid haustierGuid = Guid.NewGuid();
                Katze katze = new Katze(haustierGuid, isAnschmiegsam: true, "Dora", Geschlecht.Frau, alter: 1);

                    // Heim
                Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
                Tierheimhaus haus = new Tierheimhaus(Guid.NewGuid(), "Heim666", adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));

                    // Kunde
                Addresse adresse2 = new Addresse("andere strasse", "10", "linz", "oesterreich");
                Kunde kunde = new Kunde(Guid.NewGuid(), "Venus", "Harajchi", adresse2, Geschlecht.Frau);

                haus.TierInsHeimBringen(katze);
                kunde.HoleHundAusHeim(haus, 10);
                new KundeRepository(db).Create(kunde);

                // Act
                Haustier actual = new RepositoryBase<Haustier>(db).GetByGuid<Haustier>(haustierGuid)!;

                // Assert
                Assert.Equal(haustierGuid, actual.Guid);
            }
        }

    }
}
