using System.ComponentModel;

namespace web.Models
{
    public class Item
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }
        
        public int Quantity { get; set; } = 1;

        public bool Active { get; set; } = true;

        public int WarehouseID { get; set; }

        public int CustomerID { get; set; }

        public Customer? Customer { get; set; }

        public Warehouse? Warehouse { get; set; }
    }
}