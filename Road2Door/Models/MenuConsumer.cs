namespace Road2Door.Models
{
    //this class is used to show rider's menu to consumers with complete detail
    public class MenuConsumer
    {
        public virtual List<MenuDetail> MenuDetails { get; set; } = null!;
        public virtual List<Item> Items { get; set; }

    }

   
}
