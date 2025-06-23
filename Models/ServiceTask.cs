namespace WorkshopManager.Models
{
    public class ServiceTask
    {
        public int Id { get; set; }

        public int OrderId { get; set; } // klucz do klienta

        public ServiceOrder Order { get; set; } = null!; //nie może być zadania bez zlecenia

        public string Description { get; set; }
        public string Title { get; set; }

        public int LaborCost { get; set; }
        public List<UsedPart> UsedParts { get; set; }
    }
}
