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
<<<<<<< HEAD
        public List<UsedPart> UsedParts { get; set;}
=======
        public List<UsedPart> UsedParts { get; set; }
>>>>>>> 54dcd2ecc6acc825d8f83c067fbe8d639c7b5495
    }
}
