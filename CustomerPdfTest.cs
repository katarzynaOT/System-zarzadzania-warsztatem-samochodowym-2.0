using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QuestPDF;
using WorkshopManager.Models;
using WorkshopManager.Documents;

namespace WorkshopManagerTests
{
    public class CustomerPdfTest
    {
        [Fact]
        public void GeneratePdf_Should_Return_Bytes()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var customer = new Customer { Name = "Anna", LastName = "Nowak", Email = "a@a", Phone = "123456789",  Address = "Testowa 1" };
            var car = new Car { Id = 1, Name = "Fiat", Brand = "Fiat", Model = "Punto", RegistrationPlate = "ABC123", Description = "ladny", VIN = "12S" };
            var order = new ServiceOrder { Description = "Wymiana oleju", Status = "Done", CompletedDate = "2024-06-01", AssignedMechanic = "Adam", Price = 200 };

            var carWithOrders = new CarWithOrders
            {
                Car = car,
                Orders = new List<ServiceOrder> { order }
            };

            var doc = new CustomerReportDocument(customer, new List<CarWithOrders> { carWithOrders });

            var result = doc.GeneratePdf();

            Assert.NotNull(result);
            Assert.True(result.Length > 100); //min długosc PDF

        }

        [Fact]
        public void GeneratePdf_Should_Not_Return_Bytes()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            Customer nullCustomer = null!;
            var emptyCarOrders = new List<CarWithOrders>();

            Assert.Throws<ArgumentNullException>(() => new CustomerReportDocument(nullCustomer, emptyCarOrders));
        }

        [Fact]
        public void GeneratePdf_Should_Return_EmptyPdf_WhenNoOrders()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var customer = new Customer
            {
                Name = "Anna",
                LastName = "Nowak",
                Email = "anna@wp.pl",
                Phone = "123456789",
                Address = "Testowa 1"
            };

            var carOrders = new List<CarWithOrders>(); //puste

            var doc = new CustomerReportDocument(customer, carOrders);

            var result = doc.GeneratePdf();

            Assert.NotNull(result);
            Assert.True(result.Length > 100);  //min bajty dla pdf
        }

    }
}
