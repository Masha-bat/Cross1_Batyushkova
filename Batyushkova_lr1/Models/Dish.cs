namespace Batyushkova_lr1.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        // Методы для работы с блюдом
        public void UpdatePrice(decimal newPrice)
        {
            this.Price = newPrice;
        }
    }
}
