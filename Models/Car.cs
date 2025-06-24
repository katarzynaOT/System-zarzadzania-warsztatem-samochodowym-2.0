<<<<<<< HEAD
﻿namespace WorkshopManager.Models
=======
﻿using WorkshopManager.Services;


namespace WorkshopManager.Models
>>>>>>> 54dcd2ecc6acc825d8f83c067fbe8d639c7b5495
{
    public class Car
    {
        public int Id { get; set; }

        public int CustomerId { get; set; } // klucz do klienta

        public Customer Customer { get; set; } = null!; //nie może być samochodu bez klienta
        public required string Name { get; set; }
        public required string Description { get; set; }
<<<<<<< HEAD
        public required string Brand {  get; set; }
        public required string Model {  get; set; }
        public required string VIN {  get; set; }
=======
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public required string VIN { get; set; }
>>>>>>> 54dcd2ecc6acc825d8f83c067fbe8d639c7b5495
        public required string RegistrationPlate { get; set; }
        public int ManufacturedYear { get; set; }

        public string? imageUrl { get; set; }

        public ICollection<ServiceOrder> Orders { get; } = new List<ServiceOrder>();
    }
}
