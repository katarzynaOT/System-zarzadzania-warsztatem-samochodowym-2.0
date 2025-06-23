using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopManager.Models;
using WorkshopManager.Models;

namespace WorkshopManagerTests
{
    public class ValidationTest
    {
        [Fact]
        public void ServiceOrder_Price_Should_Be_Positive()
        {
            var order = new ServiceOrder { Price = -100 };

            bool isValid = order.Price >= 0;

            Assert.False(isValid);
        }

    }
}
