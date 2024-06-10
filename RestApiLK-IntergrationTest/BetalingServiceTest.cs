using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestApiLK.data;
using RestApiLK.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiLK_IntergrationTest;
[TestClass]
public class BetalingServiceTest
{
    private static LakridsKompanigetDbContext _context;
    private static BetalingService _service;

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
        _service = new BetalingService(_context);
    }



    [ClassCleanup]
    public static void ClassCleanup()
    {
        // Ryd op efter testene, f.eks. ved at slette testdata
        _context.Dispose();
    }

    [TestMethod]
    public async Task InsertAsync_ShouldInsertPaymentIntoDatabase()
    {
        // Arrange
        var betaling = new Betaling
        {
            OrdreID = 1, // skal være eksisterende OrdreID fra testdatabasen 
            Betalingsmetode = "kort",
            Betalingsstatus = "betalt"
        };

        // Act
        var insertedBetaling = await _service.InsertAsync(betaling);

        // Assert
        Assert.IsNotNull(insertedBetaling); // Kontroller, om der er returneret noget
        Assert.IsTrue(insertedBetaling.BetalingsID > 0); // Kontroller, om en gyldig BetalingsID er tildelt
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnAllPayments()
    {
        // Arrange

        // Act
        var payments = await _service.GetAllAsync();

        // Assert
        Assert.IsNotNull(payments); // Kontroller, om der er returneret noget
        Assert.IsTrue(payments.Any()); // Kontroller, om der er mindst én betaling
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnPayment_WhenExists()
    {
        // Arrange
        int existingPaymentId = 1; // Skift til en eksisterende BetalingsID fra din testdatabase

        // Act
        var payment = await _service.GetByIdAsync(existingPaymentId);

        // Assert
        Assert.IsNotNull(payment, $"Payment with ID {existingPaymentId} should exist.");
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldModifyExistingPayment()
    {
        // Arrange
        int existingPaymentId = 1; // Skift til en eksisterende BetalingsID fra din testdatabase

        // Hent betalingen fra databasen
        var paymentToUpdate = await _service.GetByIdAsync(existingPaymentId);

        // Gem den nuværende betalingsstatus for sammenligning senere
        var currentStatus = paymentToUpdate.Betalingsstatus;

        // Act
        if (paymentToUpdate != null)
        {
            // Opdater betalingsstatus til en værdi, der er inden for den maksimale længde
            paymentToUpdate.Betalingsstatus = "betalt"; // Eksempel: "C" i stedet for "Completed"

            // Opdater betalingen i databasen
            await _service.UpdateAsync(paymentToUpdate);
        }

        // Assert
        // Hent den opdaterede betaling fra databasen
        var updatedPayment = await _service.GetByIdAsync(existingPaymentId);

        // Assertér at den opdaterede betaling ikke er null
        Assert.IsNotNull(updatedPayment);

        // Assertér at den opdaterede betalingsstatus er ændret som forventet
        Assert.AreEqual("betalt", updatedPayment.Betalingsstatus, "Payment status should be updated.");

        // Rul ændringerne tilbage, så databasen ikke påvirkes af testen
        // Gendan betalingsstatus til den oprindelige værdi
        updatedPayment.Betalingsstatus = currentStatus;
        await _context.SaveChangesAsync();
    }


    [TestMethod]
    public async Task DeleteAsync_ShouldRemovePayment()
    {
        // Arrange
        var betaling = new Betaling
        {
            OrdreID = 1, // skal være eksisterende OrdreID fra testdatabasen 
            Betalingsmetode = "kort",
            Betalingsstatus = "betalt"
        };
        var insertedBetaling = await _service.InsertAsync(betaling);

        // Act
        var result = await _service.DeleteAsync(insertedBetaling.BetalingsID);

        // Assert
        Assert.IsTrue(result, "Payment should be deleted from the database.");
        var deletedPayment = await _service.GetByIdAsync(insertedBetaling.BetalingsID);
        Assert.IsNull(deletedPayment, "Payment should be deleted from the database.");
    }
}
