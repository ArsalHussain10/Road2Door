namespace Road2Door.Models
{
    public class MenuConsumer
    {
        public virtual List<MenuDetail> MenuDetails { get; set; } = null!;
        public virtual List<Item> Items { get; set; }

    }

   
}
