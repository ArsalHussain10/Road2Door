namespace Road2Door.Models
{
    public class MenuConsumer
    {
        public virtual MenuDetail MenuDetail { get; set; } = null!;
        public virtual List<Item> Items { get; set; }

    }

   
}
