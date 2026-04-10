using System;
using System.Collections.Generic;

namespace InventoryManagementSystem
{
    public class CategoryService
    {
        private List<Category> categories;
        private int categoryIdCounter;

        public CategoryService(List<Category> categories, ref int counter)
        {
            this.categories = categories;
            this.categoryIdCounter = counter;
        }

        public void AddCategory()
        {
            Console.Clear();
            Console.WriteLine("===== ADD CATEGORY =====");

            string name = InputHelper.GetValidLetters("Enter category name: ");
            string description = InputHelper.GetValidLetters("Enter description: ");

            Category category = new Category(categoryIdCounter++, name, description);
            categories.Add(category);

            Console.WriteLine($"Added: {name}");
            Console.ReadKey();
        }

        public void ViewAllCategories()
        {
            Console.WriteLine("\n--- Categories ---");
            foreach (var c in categories)
            {
                Console.WriteLine($"ID: {c.Id} - {c.Name}");
            }
        }
    }
}