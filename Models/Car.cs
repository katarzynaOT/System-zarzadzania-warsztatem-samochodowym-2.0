using WorkshopManager.Services;


namespace WorkshopManager.Models
{
    public class Car
    {
        public int Id { get; set; }

        public int CustomerId { get; set; } // klucz do klienta

        public Customer Customer { get; set; } = null!; //nie może być samochodu bez klienta
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public required string VIN { get; set; }
        public required string RegistrationPlate { get; set; }
        public int ManufacturedYear { get; set; }

        public string? imageUrl { get; set; }

        public ICollection<ServiceOrder> Orders { get; } = new List<ServiceOrder>();
    }
}
