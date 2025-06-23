using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WorkshopManager.Models
{
    public class OrderIndexData
    {
        public ServiceOrder Order { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        public IEnumerable<ServiceTask> ServiceTasks { get; set; }

        public IEnumerable<UsedPart> UsedParts { get; set; }
    }
}
