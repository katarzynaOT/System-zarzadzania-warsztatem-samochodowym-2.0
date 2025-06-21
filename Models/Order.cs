namespace WorkshopManager.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string AssignedMechanic { get; set; }

        public List<ServiceTask> Tasks { get; set; }
        public List<Comment> Comments { get; set; }
        public int Price { get; internal set; }
    }
}
