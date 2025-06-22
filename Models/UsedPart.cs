namespace WorkshopManager.Models
{
    public class UsedPart
    {
        public int Id { get; set; }
        public int PartId { get; set; }

        public Part Part { get; set; } = null!; // nie ma uzytej czesci bez czesci z bazy
                                                // 
        public int ServiceTaskId { get; set; }

        public ServiceTask ServiceTask { get; set; } = null!; // nie ma uzytej czesci bez zadania serwisowego
        public int Quantity { get; set; }

        public int TotalCost { get; set; }
    }
}
