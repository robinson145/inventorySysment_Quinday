using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem
{
    public class ProductService
    {
        private List<Product> products;
        private List<Category> categories;
        private List<Supplier> suppliers;
        private TransactionService transactionService;
        private int productIdCounter;

        public ProductService(List<Product> products,
                              List<Category> categories,
                              List<Supplier> suppliers,
                              TransactionService transactionService,
                              ref int counter)
        {
            this.products = products;
            this.categories = categories;
            this.suppliers = suppliers;
            this.transactionService = transactionService;
            this.productIdCounter = counter;
        }

        public void AddProduct()
        {
            Console.Clear();

            if (!categories.Any() || !suppliers.Any())
            {
                Console.WriteLine("Missing category/supplier.");
                Console.ReadKey();
                return;
            }

            int categoryId = InputHelper.GetValidNumber("Category ID: ");
            int supplierId = InputHelper.GetValidNumber("Supplier ID: ");

            string name = InputHelper.GetValidLetters("Name: ");
            string desc = InputHelper.GetValidLetters("Desc: ");
            decimal price = InputHelper.GetValidDecimal("Price: ");
            int qty = InputHelper.GetValidNumber("Qty: ");
            int min = InputHelper.GetValidNumber("Min stock: ");

            var p = new Product(productIdCounter++, name, desc, categoryId, supplierId, price, qty, min);
            products.Add(p);

            transactionService.Record(p.Id, p.Name, "INITIAL", qty, "Initial stock");

            Console.WriteLine("Product added.");
            Console.ReadKey();
        }

        public void ViewAllProducts()
        {
            Console.Clear();

            foreach (var p in products)
            {
                Console.WriteLine($"{p.Id} | {p.Name} | {p.Quantity}");
            }

            Console.ReadKey();
        }

        public void Restock()
        {
            int id = InputHelper.GetValidNumber("ID: ");
            var p = products.FirstOrDefault(x => x.Id == id);

            int qty = InputHelper.GetValidNumber("Add qty: ");
            p.Quantity += qty;

            transactionService.Record(p.Id, p.Name, "RESTOCK", qty, "Restocked");
        }

        public void Deduct()
        {
            int id = InputHelper.GetValidNumber("ID: ");
            var p = products.FirstOrDefault(x => x.Id == id);

            int qty = InputHelper.GetValidNumber("Deduct: ");
            p.Quantity -= qty;

            transactionService.Record(p.Id, p.Name, "DEDUCT", qty, "Deducted");
        }
    }
}