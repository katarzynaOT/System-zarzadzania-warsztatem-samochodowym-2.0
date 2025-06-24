<<<<<<< HEAD
﻿namespace WorkshopManager.Models
=======
﻿using System.ComponentModel.DataAnnotations;

namespace WorkshopManager.Models
>>>>>>> 54dcd2ecc6acc825d8f83c067fbe8d639c7b5495
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName {  get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

<<<<<<< HEAD
        public ICollection<Car> Cars { get; } = new List<Car>();
=======
        public ICollection<Car> Cars { get; } = new List<Car>(); //relacja 1:N z pojazdami

>>>>>>> 54dcd2ecc6acc825d8f83c067fbe8d639c7b5495
    }
}
