using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WorkshopManager.Models
{
    public class CustomerIndexData
    {
        public IEnumerable<Customer> Customers { get; set; }

        public Customer selectedCustomer { get; set; }
        public IEnumerable<Car> Cars { get; set; }

        public Car selectedCar { get; set; }

        public IEnumerable<ServiceOrder> SelectedCarOrders { get; set; }


    }
}
