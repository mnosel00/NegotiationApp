namespace NegotiationApp.Domain.Entities
{
    public class Product
    {
        public int Id { get; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public Product(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            Name = name;
            Price = price;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdatePrice(decimal price)
        {
            Price = price;
        }
    }
}
