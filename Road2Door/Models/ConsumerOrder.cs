namespace Road2Door.Models
{
    public class ConsumerOrder
    {
        public int OrderId { get; set; }
        public Rider? Rider { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
