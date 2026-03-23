namespace E_commerce_API.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
