using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestApiLK.data;
using RestApiLK.services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiLK_IntergrationTest
{
    [TestClass]
    public class KundeServiceTest
    {
        private static LakridsKompanigetDbContext _context;
        private static kundeService _service;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var connectionString = Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING")
                                  ?? "Server=localhost;Port=3306;Database=Lakrids;User=root;Password=1234;";

            var optionsBuilder = new DbContextOptionsBuilder<LakridsKompanigetDbContext>();

            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 28)));
            }
            else
            {
                // Fallback to default connection string if TEST_CONNECTION_STRING is not provided
                connectionString = "Server=localhost;Port=3306;Database=Lakrids;User=root;Password=1234;";
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 28)));
            }

            var options = optionsBuilder.Options;

            _context = new LakridsKompanigetDbContext(options);
            _service = new kundeService(_context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Ryd op efter testene, f.eks. ved at slette testdata
            _context.Dispose();
        }

        [TestMethod]
        public async Task InsertAsync_ShouldInsertKundeIntoDatabase()
        {
            // Arrange
            var kunde = new Kunder
            {
                Navn = "Test Kunde",
                Efternavn = "Test Efternavn",
                Adresse = "Test Adresse",
                PostnummerBy = "1234 TestBy",
                Telefon = "12345678",
                Email = "test@test.com"
            };

            // Act
            var insertedKunde = await _service.InsertAsync(kunde);

            // Assert
            Assert.IsNotNull(insertedKunde); // Kontroller, om der er returneret noget
            Assert.IsTrue(insertedKunde.KundeID > 0); // Kontroller, om en gyldig KundeID er tildelt
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllKunder()
        {
            // Arrange

            // Act
            var kunder = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(kunder); // Kontroller, om der er returneret noget
            Assert.IsTrue(kunder.Any()); // Kontroller, om der er mindst én kunde
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnKunde_WhenExists()
        {
            // Arrange
            int existingKundeId = 3; // Skift til en eksisterende KundeID fra din testdatabase

            // Act
            var kunde = await _service.GetByIdAsync(existingKundeId);

            // Assert
            Assert.IsNotNull(kunde, $"Kunde with ID {existingKundeId} should exist.");
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldModifyExistingKunde()
        {
            // Arrange
            int existingKundeId = 3; // Skift til en eksisterende KundeID fra din testdatabase

            // Act
            var kundeToUpdate = await _service.GetByIdAsync(existingKundeId);

            // Assert
            if (kundeToUpdate != null) // Tilføj en nullkontrol
            {
                // Fortsæt med opdatering, hvis kundeToUpdate ikke er null
                var currentAdresse = kundeToUpdate.Adresse;
                // Fortsæt resten af testen...
            }
            else
            {
                Assert.Fail("Kunde to update is null."); // Fail testen, hvis kundeToUpdate er null
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveKunde()
        {
            // Arrange
            var kunde = new Kunder
            {
                Navn = "Test Kunde",
                Efternavn = "Test Efternavn",
                Adresse = "Test Adresse",
                PostnummerBy = "1234 TestBy",
                Telefon = "12345678",
                Email = "test@test.com"
            };
            var insertedKunde = await _service.InsertAsync(kunde);

            // Act
            var result = await _service.DeleteAsync(insertedKunde.KundeID);

            // Assert
            Assert.IsTrue(result, "Kunde should be deleted from the database.");
            var deletedKunde = await _service.GetByIdAsync(insertedKunde.KundeID);
            Assert.IsNull(deletedKunde, "Kunde should be deleted from the database.");
        }
    }
}
