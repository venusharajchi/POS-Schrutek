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
    public class KundeRepositoryTest
    {
        private readonly Mock<TierheimContext> _db = new Mock<TierheimContext>();
        private readonly IReadOnlyRepositoryBase<Kunde> _unitToTest;

        private readonly ITestOutputHelper output;


        public KundeRepositoryTest(ITestOutputHelper output)
        {
            IReadOnlyRepositoryBase<Kunde> _unitToTest = new RepositoryBase<Kunde>(_db.Object);

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

                Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
                Tierheimhaus haus = new Tierheimhaus(Guid.NewGuid(), "Heim666", adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));

                var tierheimRepository = new TierheimRepository(db);
                var actual = tierheimRepository.GetAll().Count();

                // Act
                tierheimRepository.Create(haus);

                // Assert
                var expected = tierheimRepository.GetAll().Count();
                Assert.Equal(actual+1, expected);
            }
        }


        // FAILED TEST
        [Fact]
        public void Create_TierheimRepositoryCreateException_Test()
        {
            // Arrange (Entity, DB)
            using (TierheimContext db = new TierheimContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);
                
                Tierheimhaus haus1 = new Tierheimhaus(Guid.NewGuid(), "Heim666", null, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));

                // Act & Assert
                Assert.Throws<RepositoryException>(() => new TierheimRepository(db).Create(haus1));
            }
        }



        // SUCESS TEST
        [Fact]
        public void Tierheimhaus_GetByPK_Success_Test()
        {
            // Arrange
            using (TierheimContext db = new TierheimContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);

                Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
                Tierheimhaus expected = new Tierheimhaus(Guid.NewGuid(), "Heim666", adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));
                new TierheimRepository(db).Create(expected);

                // Act
                Tierheimhaus actual = new RepositoryBase<Tierheimhaus>(db).GetByPK<string, Personal>("Heim666")!;

                // Assert
                Assert.Equal(expected.Name, actual.Name);
            }
        }



        // SUCESS TEST
        [Fact]
        public void Personal_GetByGuid_Success_Test()
        {
            // Arrange
            using (TierheimContext db = new TierheimContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);


                Guid personalGuid = Guid.NewGuid();

                Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
                Tierheimhaus haus = new Tierheimhaus(Guid.NewGuid(), "Heim666", adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));
                Personal expected = new Personal(personalGuid, "susi", "polter", Geschlecht.Frau, "1234567890", haus);

                haus.PersonalAnstellen(expected);
                new TierheimRepository(db).Create(haus);

                // Act
                Personal actual = new RepositoryBase<Personal>(db).GetByGuid<Personal>(personalGuid)!;

                // Assert
                Assert.Equal(personalGuid, actual.Guid);
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


                Guid haustierGuid = Guid.NewGuid();

                Addresse adresse1 = new Addresse("eine strasse", "33", "wien", "oesterreich");
                Tierheimhaus haus = new Tierheimhaus(Guid.NewGuid(), "Heim666", adresse1, DateTime.Now, DateTime.Now.Add(TimeSpan.FromHours(10)));
                Katze expected = new Katze(haustierGuid, isAnschmiegsam: true, "Dora", Geschlecht.Frau, alter: 1);

                haus.TierInsHeimBringen(expected);
                new TierheimRepository(db).Create(haus);

                // Act
                Haustier actual = new RepositoryBase<Haustier>(db).GetByGuid<Haustier>(haustierGuid)!;

                // Assert
                Assert.Equal(haustierGuid, actual.Guid);
            }
        }

    }
}
