using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InventoryManagementSystem
{
    // ==================== MODELS ====================

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Category(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }

    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }

        public Supplier(int id, string name, string contact, string email)
        {
            Id = id;
            Name = name;
            Contact = contact;
            Email = email;
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int MinStockLevel { get; set; }

        public Product(int id, string name, string description, int categoryId, int supplierId, decimal price, int quantity, int minStockLevel)
        {
            Id = id;
            Name = name;
            Description = description;
            CategoryId = categoryId;
            SupplierId = supplierId;
            Price = price;
            Quantity = quantity;
            MinStockLevel = minStockLevel;
        }

        public bool IsLowStock()
        {
            return Quantity <= MinStockLevel;
        }

        public decimal GetInventoryValue()
        {
            return Price * Quantity;
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }

        public User(int id, string username, string role)
        {
            Id = id;
            Username = username;
            Role = role;
        }
    }

    public class TransactionRecord
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }

        public TransactionRecord(int id, int productId, string productName, string type, int quantity, string notes)
        {
            Id = id;
            ProductId = productId;
            ProductName = productName;
            Type = type;
            Quantity = quantity;
            Date = DateTime.Now;
            Notes = notes;
        }
    }

    // ==================== MAIN PROGRAM ====================

    class Program
    {
        static List<Category> categories = new List<Category>();
        static List<Supplier> suppliers = new List<Supplier>();
        static List<Product> products = new List<Product>();
        static List<User> users = new List<User>();
        static List<TransactionRecord> transactions = new List<TransactionRecord>();

        static int categoryIdCounter = 1;
        static int supplierIdCounter = 1;
        static int productIdCounter = 1;
        static int userIdCounter = 1;
        static int transactionIdCounter = 1;

        static void Main(string[] args)
        {
            SeedData();

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("===== INVENTORY MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. Add Category");
                Console.WriteLine("2. Add Supplier");
                Console.WriteLine("3. Add Product");
                Console.WriteLine("4. View All Products");
                Console.WriteLine("5. Search Product");
                Console.WriteLine("6. Update Product");
                Console.WriteLine("7. Delete Product");
                Console.WriteLine("8. Restock Product");
                Console.WriteLine("9. Deduct Stock");
                Console.WriteLine("10. View Transaction History");
                Console.WriteLine("11. Show Low Stock Items");
                Console.WriteLine("12. Compute Total Inventory Value");
                Console.WriteLine("0. Exit");
                Console.WriteLine("======================================");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddCategory(); break;
                        case "2": AddSupplier(); break;
                        case "3": AddProduct(); break;
                        case "4": ViewAllProducts(); break;
                        case "5": SearchProduct(); break;
                        case "6": UpdateProduct(); break;
                        case "7": DeleteProduct(); break;
                        case "8": RestockProduct(); break;
                        case "9": DeductStock(); break;
                        case "10": ViewTransactionHistory(); break;
                        case "11": ShowLowStockItems(); break;
                        case "12": ComputeTotalInventoryValue(); break;
                        case "0": running = false; break;
                        default: Console.WriteLine("Invalid choice!"); Pause(); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Pause();
                }
            }
        }

        // ==================== INPUT VALIDATION METHODS ====================

        static int GetValidNumber(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty. Please enter a number.");
                    continue;
                }

                if (!input.All(char.IsDigit))
                {
                    Console.WriteLine("Invalid input. Numbers only allowed.");
                    continue;
                }

                if (int.TryParse(input, out int result) && result >= 0)
                {
                    return result;
                }

                Console.WriteLine("Invalid number. Please try again.");
            }
        }

        static decimal GetValidDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty. Please enter a decimal number.");
                    continue;
                }

                bool hasOnlyDigitsAndDot = true;
                int dotCount = 0;

                foreach (char c in input)
                {
                    if (c == '.')
                    {
                        dotCount++;
                        if (dotCount > 1)
                        {
                            hasOnlyDigitsAndDot = false;
                            break;
                        }
                    }
                    else if (!char.IsDigit(c))
                    {
                        hasOnlyDigitsAndDot = false;
                        break;
                    }
                }

                if (!hasOnlyDigitsAndDot)
                {
                    Console.WriteLine("Invalid input. Numbers and decimal point only allowed.");
                    continue;
                }

                if (decimal.TryParse(input, out decimal result) && result >= 0)
                {
                    return result;
                }

                Console.WriteLine("Invalid decimal. Please try again.");
            }
        }

        static string GetValidLetters(string prompt, bool allowSpaces = true)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty.");
                    continue;
                }

                bool valid = true;
                foreach (char c in input)
                {
                    if (!char.IsLetter(c))
                    {
                        if (allowSpaces && c == ' ')
                        {
                            continue;
                        }
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                {
                    Console.WriteLine("Invalid input. Letters only allowed.");
                    continue;
                }

                return input.Trim();
            }
        }

        static string GetValidLettersOrDigits(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty.");
                    continue;
                }

                bool valid = true;
                foreach (char c in input)
                {
                    if (!char.IsLetterOrDigit(c) && c != ' ' && c != '-' && c != '_' && c != '@' && c != '.')
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                {
                    Console.WriteLine("Invalid input. Letters, numbers, spaces, hyphens, underscores, @ and . only allowed.");
                    continue;
                }

                return input.Trim();
            }
        }

        // ==================== FEATURE METHODS ====================

        static void AddCategory()
        {
            Console.Clear();
            Console.WriteLine("===== ADD CATEGORY =====");

            string name = GetValidLetters("Enter category name (letters only): ");
            string description = GetValidLetters("Enter description (letters only): ");

            Category category = new Category(categoryIdCounter++, name, description);
            categories.Add(category);

            Console.WriteLine($"Category '{name}' added successfully with ID: {category.Id}");
            Pause();
        }

        static void AddSupplier()
        {
            Console.Clear();
            Console.WriteLine("===== ADD SUPPLIER =====");

            string name = GetValidLetters("Enter supplier name (letters only): ");
            string contact = GetValidNumber("Enter contact number (numbers only): ").ToString();
            string email = GetValidLettersOrDigits("Enter email: ");

            Supplier supplier = new Supplier(supplierIdCounter++, name, contact, email);
            suppliers.Add(supplier);

            Console.WriteLine($"Supplier '{name}' added successfully with ID: {supplier.Id}");
            Pause();
        }

        static void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("===== ADD PRODUCT =====");

            if (categories.Count == 0)
            {
                Console.WriteLine("No categories available. Please add a category first.");
                Pause();
                return;
            }

            if (suppliers.Count == 0)
            {
                Console.WriteLine("No suppliers available. Please add a supplier first.");
                Pause();
                return;
            }

            ViewAllCategories();
            int categoryId = GetValidNumber("Enter Category ID (numbers only): ");

            if (!categories.Any(c => c.Id == categoryId))
            {
                Console.WriteLine("Invalid Category ID!");
                Pause();
                return;
            }

            ViewAllSuppliers();
            int supplierId = GetValidNumber("Enter Supplier ID (numbers only): ");

            if (!suppliers.Any(s => s.Id == supplierId))
            {
                Console.WriteLine("Invalid Supplier ID!");
                Pause();
                return;
            }

            string name = GetValidLetters("Enter product name (letters only): ");
            string description = GetValidLetters("Enter description (letters only): ");
            decimal price = GetValidDecimal("Enter price (numbers only): ");
            int quantity = GetValidNumber("Enter quantity (numbers only): ");
            int minStock = GetValidNumber("Enter minimum stock level (numbers only): ");

            Product product = new Product(productIdCounter++, name, description, categoryId, supplierId, price, quantity, minStock);
            products.Add(product);

            RecordTransaction(product.Id, product.Name, "INITIAL_STOCK", quantity, "Initial stock added");

            Console.WriteLine($"Product '{name}' added successfully with ID: {product.Id}");
            Pause();
        }

        static void ViewAllProducts()
        {
            Console.Clear();
            Console.WriteLine("===== ALL PRODUCTS =====");

            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                Pause();
                return;
            }

            Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10} {4,-10} {5,-10}", "ID", "Name", "Category", "Price", "Qty", "Status");
            Console.WriteLine(new string('-', 75));

            foreach (var p in products)
            {
                string categoryName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Name ?? "Unknown";
                string status = p.IsLowStock() ? "LOW STOCK" : "OK";
                Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10:C2} {4,-10} {5,-10}", p.Id, p.Name, categoryName, p.Price, p.Quantity, status);
            }

            Pause();
        }

        static void SearchProduct()
        {
            Console.Clear();
            Console.WriteLine("===== SEARCH PRODUCT =====");
            Console.WriteLine("Search by: 1. ID  2. Name");
            Console.Write("Choice: ");
            string searchChoice = Console.ReadLine();

            if (searchChoice == "1")
            {
                int id = GetValidNumber("Enter Product ID (numbers only): ");
                var product = products.FirstOrDefault(p => p.Id == id);

                if (product != null)
                {
                    DisplayProductDetails(product);
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }
            }
            else if (searchChoice == "2")
            {
                string name = GetValidLetters("Enter Product Name (letters only): ");
                var results = products.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();

                if (results.Count > 0)
                {
                    foreach (var p in results)
                    {
                        DisplayProductDetails(p);
                        Console.WriteLine("-------------------");
                    }
                }
                else
                {
                    Console.WriteLine("No products found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }

            Pause();
        }

        static void UpdateProduct()
        {
            Console.Clear();
            Console.WriteLine("===== UPDATE PRODUCT =====");
            ViewAllProducts();

            int id = GetValidNumber("Enter Product ID to update (numbers only): ");
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                Pause();
                return;
            }

            Console.WriteLine("Leave blank to keep current value.");

            Console.Write($"Current Name: {product.Name}. New Name (letters only): ");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (name.All(c => char.IsLetter(c) || c == ' '))
                    product.Name = name;
                else
                    Console.WriteLine("Invalid name format. Keeping old value.");
            }

            Console.Write($"Current Price: {product.Price}. New Price (numbers only): ");
            string priceStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(priceStr))
            {
                if (priceStr.All(c => char.IsDigit(c) || c == '.'))
                {
                    if (decimal.TryParse(priceStr, out decimal newPrice))
                        product.Price = newPrice;
                }
                else
                {
                    Console.WriteLine("Invalid price format. Keeping old value.");
                }
            }

            Console.WriteLine("Product updated successfully.");
            Pause();
        }

        static void DeleteProduct()
        {
            Console.Clear();
            Console.WriteLine("===== DELETE PRODUCT =====");
            ViewAllProducts();

            int id = GetValidNumber("Enter Product ID to delete (numbers only): ");
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                Pause();
                return;
            }

            Console.Write($"Are you sure you want to delete '{product.Name}'? (yes/no): ");
            string confirm = Console.ReadLine()?.ToLower();

            if (confirm == "yes")
            {
                products.Remove(product);
                RecordTransaction(product.Id, product.Name, "DELETE", product.Quantity, "Product deleted");
                Console.WriteLine("Product deleted successfully.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }

            Pause();
        }

        static void RestockProduct()
        {
            Console.Clear();
            Console.WriteLine("===== RESTOCK PRODUCT =====");
            ViewAllProducts();

            int id = GetValidNumber("Enter Product ID to restock (numbers only): ");
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                Pause();
                return;
            }

            int amount = GetValidNumber("Enter quantity to add (numbers only): ");

            if (amount <= 0)
            {
                Console.WriteLine("Amount must be greater than 0.");
                Pause();
                return;
            }

            product.Quantity += amount;
            RecordTransaction(product.Id, product.Name, "RESTOCK", amount, "Stock replenished");

            Console.WriteLine($"Restocked {amount} units. New quantity: {product.Quantity}");
            Pause();
        }

        static void DeductStock()
        {
            Console.Clear();
            Console.WriteLine("===== DEDUCT STOCK =====");
            ViewAllProducts();

            int id = GetValidNumber("Enter Product ID to deduct stock (numbers only): ");
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                Pause();
                return;
            }

            int amount = GetValidNumber("Enter quantity to deduct (numbers only): ");

            if (amount <= 0)
            {
                Console.WriteLine("Amount must be greater than 0.");
                Pause();
                return;
            }

            if (amount > product.Quantity)
            {
                Console.WriteLine("Insufficient stock!");
                Pause();
                return;
            }

            product.Quantity -= amount;
            RecordTransaction(product.Id, product.Name, "DEDUCT", amount, "Stock deducted");

            Console.WriteLine($"Deducted {amount} units. New quantity: {product.Quantity}");
            Pause();
        }

        static void ViewTransactionHistory()
        {
            Console.Clear();
            Console.WriteLine("===== TRANSACTION HISTORY =====");

            if (transactions.Count == 0)
            {
                Console.WriteLine("No transactions found.");
                Pause();
                return;
            }

            Console.WriteLine("{0,-5} {1,-5} {2,-20} {3,-12} {4,-8} {5,-20} {6,-20}", "ID", "PID", "Product", "Type", "Qty", "Date", "Notes");
            Console.WriteLine(new string('-', 95));

            foreach (var t in transactions.OrderByDescending(t => t.Date))
            {
                Console.WriteLine("{0,-5} {1,-5} {2,-20} {3,-12} {4,-8} {5,-20:MM/dd/yyyy HH:mm} {6,-20}",
                    t.Id, t.ProductId, t.ProductName, t.Type, t.Quantity, t.Date, t.Notes);
            }

            Pause();
        }

        static void ShowLowStockItems()
        {
            Console.Clear();
            Console.WriteLine("===== LOW STOCK ITEMS =====");

            var lowStock = products.Where(p => p.IsLowStock()).ToList();

            if (lowStock.Count == 0)
            {
                Console.WriteLine("No low stock items found.");
                Pause();
                return;
            }

            Console.WriteLine("{0,-5} {1,-20} {2,-10} {3,-15} {4,-15}", "ID", "Name", "Current", "Min Level", "Status");
            Console.WriteLine(new string('-', 70));

            foreach (var p in lowStock)
            {
                string status = p.Quantity == 0 ? "OUT OF STOCK" : "LOW STOCK";
                Console.WriteLine("{0,-5} {1,-20} {2,-10} {3,-15} {4,-15}", p.Id, p.Name, p.Quantity, p.MinStockLevel, status);
            }

            Pause();
        }

        static void ComputeTotalInventoryValue()
        {
            Console.Clear();
            Console.WriteLine("===== TOTAL INVENTORY VALUE =====");

            if (products.Count == 0)
            {
                Console.WriteLine("No products in inventory.");
                Pause();
                return;
            }

            decimal totalValue = 0;

            Console.WriteLine("{0,-5} {1,-20} {2,-10} {3,-10} {4,-15}", "ID", "Name", "Price", "Qty", "Value");
            Console.WriteLine(new string('-', 65));

            foreach (var p in products)
            {
                decimal value = p.GetInventoryValue();
                totalValue += value;
                Console.WriteLine("{0,-5} {1,-20} {2,-10:C2} {3,-10} {4,-15:C2}", p.Id, p.Name, p.Price, p.Quantity, value);
            }

            Console.WriteLine(new string('-', 65));
            Console.WriteLine("{0,-50} {1,-15:C2}", "TOTAL INVENTORY VALUE:", totalValue);

            Pause();
        }

        // ==================== HELPER METHODS ====================

        static void ViewAllCategories()
        {
            Console.WriteLine("\n--- Available Categories ---");
            foreach (var c in categories)
            {
                Console.WriteLine($"ID: {c.Id} - {c.Name}");
            }
            Console.WriteLine();
        }

        static void ViewAllSuppliers()
        {
            Console.WriteLine("\n--- Available Suppliers ---");
            foreach (var s in suppliers)
            {
                Console.WriteLine($"ID: {s.Id} - {s.Name}");
            }
            Console.WriteLine();
        }

        static void DisplayProductDetails(Product p)
        {
            string categoryName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Name ?? "Unknown";
            string supplierName = suppliers.FirstOrDefault(s => s.Id == p.SupplierId)?.Name ?? "Unknown";

            Console.WriteLine($"ID: {p.Id}");
            Console.WriteLine($"Name: {p.Name}");
            Console.WriteLine($"Description: {p.Description}");
            Console.WriteLine($"Category: {categoryName}");
            Console.WriteLine($"Supplier: {supplierName}");
            Console.WriteLine($"Price: {p.Price:C}");
            Console.WriteLine($"Quantity: {p.Quantity}");
            Console.WriteLine($"Min Stock Level: {p.MinStockLevel}");
            Console.WriteLine($"Status: {(p.IsLowStock() ? "LOW STOCK" : "OK")}");
            Console.WriteLine($"Inventory Value: {p.GetInventoryValue():C}");
        }

        static void RecordTransaction(int productId, string productName, string type, int quantity, string notes)
        {
            TransactionRecord trans = new TransactionRecord(transactionIdCounter++, productId, productName, type, quantity, notes);
            transactions.Add(trans);
        }

        static void SeedData()
        {
            // Add default categories
            categories.Add(new Category(categoryIdCounter++, "Electronics", "Electronic devices and accessories"));
            categories.Add(new Category(categoryIdCounter++, "Food", "Food and beverages"));

            // Add default suppliers
            suppliers.Add(new Supplier(supplierIdCounter++, "TechCorp", "1234567890", "tech@corp.com"));
            suppliers.Add(new Supplier(supplierIdCounter++, "FoodMart", "0987654321", "food@mart.com"));

            // Add deFault user
            users.Add(new User(userIdCounter++, "admin", "Administrator"));
        }

        static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}