namespace web.Models
{
    public class Evidence
    {
        public int ID { get; set; }

        public int? ItemID { get; set; }

        public int WarehouseID { get; set; }

        public int CustomerID { get; set; }

        public DateTime Out { get; set; }

        public Warehouse Warehouse { get; set; }

        public Item? Item { get; set; }

        public Customer Customer { get; set; }
    }
}