namespace DiaHelp.Model
{
    public class FoodModel
    {
        public int Id { get; set; }
        public string FoodName { get; set; }
        public string Category { get; set; }
        public string FoodType { get; set; }
        public double Weight { get; set; }
        public double InsulinFromFood { get; set; }
        public DateTime Date { get; set; }
    }
}
