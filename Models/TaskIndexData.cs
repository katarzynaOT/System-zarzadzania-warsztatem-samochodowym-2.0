using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WorkshopManager.Models
{
    public class TaskIndexData
    {
        public ServiceTask Task { get; set; }
        public IEnumerable<UsedPart> TaskParts { get; set; }
    }
}
