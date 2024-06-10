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
    public class OrdreServiceTest
    {
        private static LakridsKompanigetDbContext _context;
        private static OrdreService _service;

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
            _service = new OrdreService(_context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Ryd op efter testene, f.eks. ved at slette testdata
            _context.Dispose();
        }

        [TestMethod]
        public async Task InsertAsync_ShouldInsertOrdreIntoDatabase()
        {
            // Arrange
            var ordre = new Ordre
            {
                KundeID = 1, // Skal være eksisterende KundeID fra testdatabasen 
                ProduktID = 1, // Skal være eksisterende ProduktID fra testdatabasen 
                Antal = 5,
                Ordrestatus = "afsendt"
            };

            // Act
            var insertedOrdre = await _service.InsertAsync(ordre);

            // Assert
            Assert.IsNotNull(insertedOrdre); // Kontroller, om der er returneret noget
            Assert.IsTrue(insertedOrdre.OrdreID > 0); // Kontroller, om en gyldig OrdreID er tildelt
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllOrdrer()
        {
            // Arrange

            // Act
            var ordrer = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(ordrer); // Kontroller, om der er returneret noget
            Assert.IsTrue(ordrer.Any()); // Kontroller, om der er mindst én ordre
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnOrdre_WhenExists()
        {
            // Arrange
            int existingOrdreId = 1; // Skift til en eksisterende OrdreID fra din testdatabase

            // Act
            var ordre = await _service.GetByIdAsync(existingOrdreId);

            // Assert
            Assert.IsNotNull(ordre, $"Ordre with ID {existingOrdreId} should exist.");
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldModifyExistingOrdre()
        {
            // Arrange
            int existingOrdreId = 1; // Skift til en eksisterende OrdreID fra din testdatabase

            // Act
            var ordreToUpdate = await _service.GetByIdAsync(existingOrdreId);

            // Assert
            if (ordreToUpdate != null) // Tilføj en nullkontrol
            {
                // Fortsæt med opdatering, hvis ordreToUpdate ikke er null
                var currentStatus = ordreToUpdate.Ordrestatus;
                ordreToUpdate.Ordrestatus = "behandling"; // Eksempel: "Behandlet" i stedet for "Ny"
                await _service.UpdateAsync(ordreToUpdate);

                // Assert efter opdatering
                var updatedOrdre = await _service.GetByIdAsync(existingOrdreId);
                Assert.IsNotNull(updatedOrdre);
                Assert.AreEqual("behandling", updatedOrdre.Ordrestatus, "Ordre status should be updated.");

                // Rul ændringerne tilbage, så databasen ikke påvirkes af testen
                // Gendan ordrestatus til den oprindelige værdi
                updatedOrdre.Ordrestatus = currentStatus;
                await _context.SaveChangesAsync();
            }
            else
            {
                Assert.Fail("Ordre to update is null."); // Fail testen, hvis ordreToUpdate er null
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveOrdre()
        {
            // Arrange
            var ordre = new Ordre
            {
                KundeID = 1, // Skal være eksisterende KundeID fra testdatabasen 
                ProduktID = 1, // Skal være eksisterende ProduktID fra testdatabasen 
                Antal = 5,
                Ordrestatus = "afsendt"
            };
            var insertedOrdre = await _service.InsertAsync(ordre);

            // Act
            var result = await _service.DeleteAsync(insertedOrdre.OrdreID);

            // Assert
            Assert.IsTrue(result, "Ordre should be deleted from the database.");
            var deletedOrdre = await _service.GetByIdAsync(insertedOrdre.OrdreID);
            Assert.IsNull(deletedOrdre, "Ordre should be deleted from the database.");
        }
    }
}
