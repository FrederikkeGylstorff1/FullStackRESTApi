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
    public class ProductServiceTest
    {
        private static LakridsKompanigetDbContext _context;
        private static ProduktService _service;

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
            _service = new ProduktService(_context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Clean up after the tests, for example, by deleting test data
            _context.Dispose();
        }

        [TestMethod]
        public async Task InsertAsync_ShouldInsertProductIntoDatabase()
        {
            // Arrange
            var product = new produkter
            {
                Titel = "Test Product",
                Beskrivelse = "Test Description",
                Pris = 19.99m,
                AntalPåLager = 10,
                Indhold = "Test Content"
            };

            // Act
            var insertedProduct = await _service.InsertAsync(product);

            // Assert
            Assert.IsNotNull(insertedProduct); // Check if something is returned
            Assert.IsTrue(insertedProduct.ProduktID > 0); // Check if a valid ProduktID is assigned
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Act
            var products = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(products); // Check if something is returned
            Assert.IsTrue(products.Any()); // Check if there is at least one product
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
        {
            // Arrange
            int existingProductId = 1; // Change to an existing ProduktID from your test database

            // Act
            var product = await _service.GetByIdAsync(existingProductId);

            // Assert
            Assert.IsNotNull(product, $"Product with ID {existingProductId} should exist.");
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldModifyExistingProduct()
        {
            // Arrange
            int existingProductId = 1; // Change to an existing ProduktID from your test database

            // Act
            var productToUpdate = await _service.GetByIdAsync(existingProductId);

            // Assert
            if (productToUpdate != null) // Add a null check
            {
                // Continue with the update if productToUpdate is not null
                var currentDescription = productToUpdate.Beskrivelse;
                productToUpdate.Beskrivelse = "Updated Description";
                await _service.UpdateAsync(productToUpdate);

                // Assert after the update
                var updatedProduct = await _service.GetByIdAsync(existingProductId);
                Assert.IsNotNull(updatedProduct);
                Assert.AreEqual("Updated Description", updatedProduct.Beskrivelse, "Product description should be updated.");

                // Roll back the changes so the database is not affected by the test
                // Restore the description to its original value
                updatedProduct.Beskrivelse = currentDescription;
                await _context.SaveChangesAsync();
            }
            else
            {
                Assert.Fail("Product to update is null."); // Fail the test if productToUpdate is null
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveProduct()
        {
            // Arrange
            var product = new produkter
            {
                Titel = "Test Product",
                Beskrivelse = "Test Description",
                Pris = 19.99m,
                AntalPåLager = 10,
                Indhold = "Test Content"
            };
            var insertedProduct = await _service.InsertAsync(product);

            // Act
            var result = await _service.DeleteAsync(insertedProduct.ProduktID);

            // Assert
            Assert.IsTrue(result, "Product should be deleted from the database.");
            var deletedProduct = await _service.GetByIdAsync(insertedProduct.ProduktID);
            Assert.IsNull(deletedProduct, "Product should be deleted from the database.");
        }
    }
}
