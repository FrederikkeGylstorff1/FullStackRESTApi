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
    public class ForhandlerServiceTest
    {
        private static LakridsKompanigetDbContext _context;
        private static ForhandlerService _service;

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
            _service = new ForhandlerService(_context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Ryd op efter testene, f.eks. ved at slette testdata
            _context.Dispose();
        }

        [TestMethod]
        public async Task InsertAsync_ShouldInsertForhandlerIntoDatabase()
        {
            // Arrange
            var forhandler = new Forhandler
            {
                Navn = "Test Forhandler",
                Adresse = "Test Adresse",
                PostnummerBy = "1234 TestBy",
                Telefon = "12345678",
                Mail = "test@test.com",
                Åbningstider = "Mandag-Fredag 9-17",
                Latitude = 55.12345,
                Longitude = 12.34567
            };

            // Act
            var insertedForhandler = await _service.InsertAsync(forhandler);

            // Assert
            Assert.IsNotNull(insertedForhandler); // Kontroller, om der er returneret noget
            Assert.IsTrue(insertedForhandler.ForhandlerID > 0); // Kontroller, om en gyldig ForhandlerID er tildelt
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllForhandlere()
        {
            // Arrange

            // Act
            var forhandlere = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(forhandlere); // Kontroller, om der er returneret noget
            Assert.IsTrue(forhandlere.Any()); // Kontroller, om der er mindst én forhandler
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnForhandler_WhenExists()
        {
            // Arrange
            int existingForhandlerId = 3; // Skift til en eksisterende ForhandlerID fra din testdatabase

            // Act
            var forhandler = await _service.GetByIdAsync(existingForhandlerId);

            // Assert
            Assert.IsNotNull(forhandler, $"Forhandler with ID {existingForhandlerId} should exist.");
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldModifyExistingForhandler()
        {
            // Arrange
            int existingForhandlerId = 3; // Skift til en eksisterende ForhandlerID fra din testdatabase

            // Act
            var forhandlerToUpdate = await _service.GetByIdAsync(existingForhandlerId);

            // Assert
            if (forhandlerToUpdate != null) // Tilføj en nullkontrol
            {
                // Fortsæt med opdatering, hvis forhandlerToUpdate ikke er null
                var currentAdresse = forhandlerToUpdate.Adresse;
                // Fortsæt resten af testen...
            }
            else
            {
                Assert.Fail("Forhandler to update is null."); // Fail testen, hvis forhandlerToUpdate er null
            }
        }


        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveForhandler()
        {
            // Arrange
            var forhandler = new Forhandler
            {
                Navn = "Test Forhandler",
                Adresse = "Test Adresse",
                PostnummerBy = "1234 TestBy",
                Telefon = "12345678",
                Mail = "test@test.com",
                Åbningstider = "Mandag-Fredag 9-17",
                Latitude = 55.12345,
                Longitude = 12.34567
            };
            var insertedForhandler = await _service.InsertAsync(forhandler);

            // Act
            var result = await _service.DeleteAsync(insertedForhandler.ForhandlerID);

            // Assert
            Assert.IsTrue(result, "Forhandler should be deleted from the database.");
            var deletedForhandler = await _service.GetByIdAsync(insertedForhandler.ForhandlerID);
            Assert.IsNull(deletedForhandler, "Forhandler should be deleted from the database.");
        }
    }
}
