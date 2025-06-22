using Microsoft.AspNetCore.Mvc.Rendering;

namespace WorkshopManager.Models
{
    public class ServiceArchive
    {
        public string SearchName { get; set; }
        public string SearchCar { get; set; }


        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public IEnumerable<ServiceRecord> SearchResults { get; set; }
    }

    public class ServiceRecord
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string CustomerName { get; set; }
        public string CarName { get; set; }
        public DateTime CompletedDate { get; set; }
        public int TotalCost { get; set; }

    }
}
