namespace Road2Door.Models
{
    public class RiderOrder
    {
        public int OrderId { get; set; }
        public Consumer? Consumer { get; set; }
        public int RiderId { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }

    }
}
