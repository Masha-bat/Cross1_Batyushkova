namespace Batyushkova_lr1.Models
{
    public class Table
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool IsOccupied { get; set; }

        // Метод для освобождения стола
        public void FreeTable()
        {
            this.IsOccupied = false;
        }
    }
}
