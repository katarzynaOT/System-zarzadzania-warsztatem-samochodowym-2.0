using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WorkshopManager.Models;
using WorkshopManager.Controllers;


namespace WorkshopManager.Documents;

using System;
using System.Collections.Generic;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using WorkshopManager.Models;
using WorkshopManager.Services;


public class CarWithOrders
{
    public Car Car { get; set; }
    public List<ServiceOrder> Orders { get; set; }
}

public class CustomerReportDocument
{
    private readonly Customer _customer;
    private readonly List<CarWithOrders> _carOrders;

    public CustomerReportDocument(Customer customer, List<CarWithOrders> carOrders)
    {
        _customer = customer ?? throw new ArgumentNullException(nameof(customer));
        _carOrders = carOrders ?? new List<CarWithOrders>();
    }

    public byte[] GeneratePdf()
    {

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header()
                    .Text($"Customer report: {_customer.Name} {_customer.LastName}")
                    .FontSize(20).Bold();

                page.Content().Column(column =>
                {
                    column.Item().Text($"Email: {_customer.Email}");
                    column.Item().Text($"Phone: {_customer.Phone}");
                    column.Item().Text($"Adress: {_customer.Address}");

                    column.Item().PaddingVertical(10).LineHorizontal(1);

                    column.Item().Text("All orders:").FontSize(16).Bold();

                    if (_carOrders.Count == 0)
                    {
                        column.Item().Text("No orders.");
                    }
                    else
                    {
                        foreach (var carWithOrders in _carOrders)
                        {
                            column.Item().Height(15);
                            column.Item().Text(text =>
                            {
                                text.Span("Car Id: ").FontSize(14).Bold();
                                text.Span($"{carWithOrders.Car.Id}").FontSize(14);
                            });
                            column.Item().Text(text =>
                            {
                                text.Span("Car Name: ").FontSize(14).Bold();
                                text.Span($"{carWithOrders.Car.Name}").FontSize(14);
                            });
                            column.Item().Text(text =>
                            {
                                text.Span("Car Brand: ").FontSize(14).Bold();
                                text.Span($"{carWithOrders.Car.Brand}").FontSize(14);
                            });
                            column.Item().Text(text =>
                            {
                                text.Span("Car Model: ").FontSize(14).Bold();
                                text.Span($"{carWithOrders.Car.Model}").FontSize(14);
                            });
                            column.Item().Text(text =>
                            {
                                text.Span("Car Registration Plate: ").FontSize(14).Bold();
                                text.Span($"{carWithOrders.Car.RegistrationPlate}").FontSize(14);
                            }); 

                            if (carWithOrders.Orders.Count == 0)
                            {
                                column.Item().Text("No orders for this car.").Italic();
                            }
                            else
                            {
                                foreach (var order in carWithOrders.Orders)
                                {
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Service Description: ");
                                        text.Span($"{order.Description}").Italic();
                                    });
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Order Status: ");
                                        text.Span($"{order.Status}").Italic();
                                    });
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Completed Date: ");
                                        text.Span($"{order.CompletedDate}").Italic();
                                    });
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Assigned Mechanic: ");
                                        text.Span($"{order.AssignedMechanic}").Italic();
                                    });
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Final Price: ");
                                        text.Span($"{order.Price}").Italic();
                                    });
                                }
                            }
                        }
                    }
                });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Generated: ");
                        x.Span(DateTime.Now.ToString("g")).SemiBold();
                    });
            });
        });

        using var ms = new MemoryStream();
        document.GeneratePdf(ms);
        return ms.ToArray();
    }
}

