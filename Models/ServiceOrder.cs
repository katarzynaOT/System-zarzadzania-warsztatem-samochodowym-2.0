namespace WorkshopManager.Models
{
    public class ServiceOrder
    {
        public int Id { get; set; }

        public int CarId { get; set; } // klucz do klienta

        public Car Car { get; set; } = null!; //nie może być samochodu bez klienta

        public String Description { get; set; } = null!;
        public string Status { get; set; }

        public string CompletedDate { get; set; }
        public string AssignedMechanic { get; set; }

        public List<ServiceTask> Tasks { get; set; }
        public List<Comment> Comments { get; set; }
        public int Price { get; set; }
    }
}
