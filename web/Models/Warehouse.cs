namespace web.Models
{
    public class Warehouse
    {
        public int ID { get; set; }

        public string Address { get; set; }

        public int ZIP { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public List<Item>? Items { get; set; }
    }
}