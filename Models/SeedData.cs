using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using WorkshopManager.Data;
using System.Linq;



namespace WorkshopManager.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>()))
        {
            // Look for any movies.
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            //seeding customers
            context.Customers.AddRange(
                new Customer
                {
                    Name = "Jan",
                    LastName = "Kowalski",
                    Email = "customer1@gmail.com",
                    Phone = "+48 999 888 777",
                    Address = "ul.Pilotow 19, Krakow"
                },
                new Customer
                {
                    Name = "Andrzej",
                    LastName = "Lewicki",
                    Email = "customer2@hotmail.com",
                    Phone = "+48 222 000 777",
                    Address = "ul.Krakowska, Zabierzow"
                },
                new Customer
                {
                    Name = "Alicja",
                    LastName = "Kraszewska",
                    Email = "alicja@gmail.com",
                    Phone = "+48 945 111 222",
                    Address = "ul.Krakowska, Zielonki"
                }
            );

            context.SaveChanges();
            //seeding cars
            //public int CustomerId { get; set; } // klucz do klienta
            //public required string Name { get; set; }
            //public required string Description { get; set; }
            //public required string Brand { get; set; }
            //public required string Model { get; set; }
            //public required string VIN { get; set; }
            //public required string RegistrationPlate { get; set; }
            //public int ManufacturedYear { get; set; }
            context.Cars.AddRange(
                new Car
                {
                    CustomerId = 1,
                    Name = "Toyota",
                    Description = "po wypadku",
                    Brand = "Toyota",
                    Model = "Yaris 1.4",
                    VIN = "ABC123456789",
                    RegistrationPlate = "KR061NN",
                    ManufacturedYear = 2010
                },
                new Car
                {
                    CustomerId = 2,
                    Name = "Peugeot",
                    Description = "dlugo u nas",
                    Brand = "Peugeot",
                    Model = "206",
                    VIN = "XYXLMN456789",
                    RegistrationPlate = "WE0617L",
                    ManufacturedYear = 2016
                },
                new Car
                {
                    CustomerId = 2,
                    Name = "Subaru",
                    Description = "terenowka, biala",
                    Brand = "Subaru",
                    Model = "XV EcoBoost",
                    VIN = "GHYXLMN4563434",
                    RegistrationPlate = "SO034S",
                    ManufacturedYear = 2017
                },
                new Car
                {
                    CustomerId = 3,
                    Name = "Tesla",
                    Description = "elektryk",
                    Brand = "Tesla",
                    Model = "Roadster",
                    VIN = "US-MN4563434",
                    RegistrationPlate = "X-ALICE",
                    ManufacturedYear = 2023
                },
                new Car
                {
                    CustomerId = 3,
                    Name = "Porszak",
                    Description = "uwaga na zaplon!",
                    Brand = "Porsche",
                    Model = "Carrera",
                    VIN = "DE0-4563434",
                    RegistrationPlate = "KRKPO1",
                    ManufacturedYear = 2024
                },
                new Car
                {
                    CustomerId = 3,
                    Name = "Tico",
                    Description = "stare ale jare, pierwsze auto",
                    Brand = "Daewoo",
                    Model = "Tico",
                    VIN = "KR2143VXVX2",
                    RegistrationPlate = "KRK2356",
                    ManufacturedYear = 1995
                }
            );
            context.SaveChanges();
            context.Parts.AddRange(
                new Part
                {
                    Name = "Tlumik Model 1",
                    Type = "Wydech i inne",
                    UnitPrice = 100
                },
                new Part
                {
                    Name = "Tlumik Model 2",
                    Type = "Wydech i inne",
                    UnitPrice = 90
                },
                new Part
                {
                    Name = "Tlumik Model 3",
                    Type = "Wydech i inne",
                    UnitPrice = 112
                },
                new Part
                {
                    Name = "Filtr Paliwa Typ 1",
                    Type = "Filtry",
                    UnitPrice = 99
                },
                new Part
                {
                    Name = "Filtr Paliwa Typ 2",
                    Type = "Filtry",
                    UnitPrice = 66
                },
                new Part
                {
                    Name = "Filtr Paliwa Typ 3 - zamiennik",
                    Type = "Filtry",
                    UnitPrice = 56
                },
                new Part
                {
                    Name = "Reflektory Lewy Przedni",
                    Type = "Oswietlenie",
                    UnitPrice = 45
                },
                new Part
                {
                    Name = "Reflektory Lewy Tylni",
                    Type = "Oswietlenie",
                    UnitPrice = 67
                }
            );
            context.SaveChanges();
            //seed service orders
            //public int Id { get; set; }
            //public int CarId { get; set; } // klucz do klienta
            //public Car Car { get; set; } = null!; //nie może być samochodu bez klienta
            //public String Description { get; set; } = null!;
            //public string Status { get; set; }
            //public string CompletedDate { get; set; }
            //public string AssignedMechanic { get; set; }
            //public List<ServiceTask> Tasks { get; set; }
            //public List<Comment> Comments { get; set; }
            //public int Price { get; set; }
            context.ServiceOrders.AddRange(
                new ServiceOrder
                {
                    CarId = 1,
                    Description = "cos stuka z tylu",
                    Status = "Completed",
                    CompletedDate = "6/22/2025 12:01:22 PM",
                    AssignedMechanic = "Jan Mechanik",
                    Price = 1200
                },
                new ServiceOrder
                {
                    CarId = 1,
                    Description = "cos stuka z tylu znowu",
                    Status = "New",
                    CompletedDate = "Not complete yet",
                    AssignedMechanic = "Andrzej Elektryk",
                    Price = 200
                },
                new ServiceOrder
                {
                    CarId = 2,
                    Description = "zciaga w lewo",
                    Status = "New",
                    CompletedDate = "Not complete yet",
                    AssignedMechanic = "Andrzej Elektryk",
                    Price = 400
                },
                new ServiceOrder
                {
                    CarId = 3,
                    Description = "Problem z napedem 4x4",
                    Status = "Completed",
                    CompletedDate = "6/19/2025 12:01:22 PM",
                    AssignedMechanic = "Jan Mechanik",
                    Price = 600
                },
                new ServiceOrder
                {
                    CarId = 4,
                    Description = "nie laduje, i piszczy jak stoi",
                    Status = "In progress",
                    CompletedDate = "Not complete yet",
                    AssignedMechanic = "Andrzej Elektryk",
                    Price = 4000
                },
                new ServiceOrder
                {
                    CarId = 5,
                    Description = "przeglad, bo i tak czesci nie ma",
                    Status = "In progress",
                    CompletedDate = "Not complete yet",
                    AssignedMechanic = "Jan Mechanik",
                    Price = 399
                }
            );
            context.SaveChanges();
            //seed service comments
            //public int OrderId { get; set; }
            //public DateTime CreatedDate { get; set; }
            //public string? Content { get; set; }
            //public string? Author { get; set; }
            //public string? AuthorId { get; set; }

            context.Comments.AddRange(
                new Comment
                {
                    OrderId=1,
                    CreatedDate = DateTime.Now.AddDays(-2),
                    Content = "No nie moge tego znalezc. Sprobuje jeszcze w jednym miejscu",
                    Author ="mech1@workshop.com",
                    AuthorId ="13414324fdsfs242424"
                },
                new Comment
                {
                    OrderId = 1,
                    CreatedDate = DateTime.Now.AddDays(-1),
                    Content = "Dalej nic nie znalazlem",
                    Author = "mech1@workshop.com",
                    AuthorId = "13414324fdsfs242424"
                },
                new Comment
                {
                    OrderId = 1,
                    CreatedDate = DateTime.Now,
                    Content = "Klient sie upomina o info",
                    Author = "user@workshop.com",
                    AuthorId = "341443reytet3424wrwrew"
                },
                new Comment
                {
                    OrderId = 5,
                    CreatedDate = DateTime.Now.AddDays(-12),
                    Content = "Czekamy na czesci z Niemiec",
                    Author = "user@workshop.com",
                    AuthorId = "341443reytet3424wrwrew"
                },
                new Comment
                {
                    OrderId = 5,
                    CreatedDate = DateTime.Now.AddDays(-7),
                    Content = "Nadal czekamy",
                    Author = "user@workshop.com",
                    AuthorId = "341443reytet3424wrwrew"
                },
                new Comment
                {
                    OrderId = 5,
                    CreatedDate = DateTime.Now.AddDays(-2),
                    Content = "Czesci przyszly, ale nie pasuja!",
                    Author = "mech1@workshop.com",
                    AuthorId = "13414324fdsfs242424"
                },
                new Comment
                {
                    OrderId = 6,
                    CreatedDate = DateTime.Now.AddDays(-1),
                    Content = "Generalne OK, ale jeszcze zostaly spaliny",
                    Author = "mech2@workshop.com",
                    AuthorId = "adfsfs5664fdss324898"
                }
            );
            context.SaveChanges();
            //seed service tasks
            //public int OrderId { get; set; } // klucz do klienta
            //public string Description { get; set; }
            //public string Title { get; set; }
            //public int LaborCost { get; set; }
            context.ServiceTasks.AddRange(
                new ServiceTask
                {
                    OrderId=1,
                    Description="rozkrecenie z tylu, demontaz, itd",
                    Title="diagnostyka",
                    LaborCost=250
                },
                new ServiceTask
                {
                    OrderId = 1,
                    Description = "wymiana tlumika",
                    Title = "naprawa",
                    LaborCost = 350
                },
                new ServiceTask
                {
                    OrderId = 1,
                    Description = "przepalenie, wyziewy, jakosc spalin, itd",
                    Title = "testy ponaprawcze",
                    LaborCost = 150
                },
                new ServiceTask
                {
                    OrderId = 3,
                    Description = "sprawdzenie zbieznosci - OK",
                    Title = "diagnostyka",
                    LaborCost = 250
                },
                new ServiceTask
                {
                    OrderId = 3,
                    Description = "sprawdzenie lozysk i osi",
                    Title = "diagnostyka",
                    LaborCost = 150
                },
                new ServiceTask
                {
                    OrderId = 3,
                    Description = "wymiana lozysk",
                    Title = "naprawa",
                    LaborCost = 450
                }
            );
            context.SaveChanges();
            //seed used parts
            context.UsedParts.AddRange(
                new UsedPart
                {
                    PartId=1,
                    ServiceTaskId=1,
                    Quantity=5,
                    TotalCost=500,
                },
                new UsedPart
                {
                    PartId = 2,
                    ServiceTaskId = 2,
                    Quantity = 3,
                    TotalCost = 270,
                },
                new UsedPart
                {
                    PartId = 3,
                    ServiceTaskId = 2,
                    Quantity = 3,
                    TotalCost = 336,
                },
                new UsedPart
                {
                    PartId = 1,
                    ServiceTaskId = 3,
                    Quantity = 5,
                    TotalCost = 500,
                },
                new UsedPart
                {
                    PartId = 2,
                    ServiceTaskId = 3,
                    Quantity = 3,
                    TotalCost = 270,
                },
                new UsedPart
                {
                    PartId = 3,
                    ServiceTaskId = 4,
                    Quantity = 3,
                    TotalCost = 336,
                },
                new UsedPart
                {
                    PartId = 2,
                    ServiceTaskId = 5,
                    Quantity = 3,
                    TotalCost = 270,
                },
                new UsedPart
                {
                    PartId = 3,
                    ServiceTaskId = 6,
                    Quantity = 3,
                    TotalCost = 336,
                }
            );
            context.SaveChanges();
        }
    }
}
