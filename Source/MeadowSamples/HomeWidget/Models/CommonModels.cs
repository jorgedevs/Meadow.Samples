using System.Collections.Generic;

namespace HomeWidget.Models
{
    public class WeeklyMeal
    {
        public Recipe MealA { get; set; }
        public Recipe MealB { get; set; }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public List<string> Ingridients { get; set; }
    }
}