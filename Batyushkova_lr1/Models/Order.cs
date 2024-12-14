namespace Batyushkova_lr1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime TimeOrdered { get; set; }
        public List<Dish> Dishes { get; set; }
        public Table Table { get; set; }
        public bool IsCompleted { get; set; }

        // Метод для подсчета общей суммы заказа
        public decimal CalculateTotal()
        {
            return Dishes.Sum(d => d.Price);
        }
    }
}
