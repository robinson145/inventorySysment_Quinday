using System;
using System.Collections.Generic;

namespace InventoryManagementSystem
{
    public class SupplierService
    {
        private List<Supplier> suppliers;
        private int supplierIdCounter;

        public SupplierService(List<Supplier> suppliers, ref int counter)
        {
            this.suppliers = suppliers;
            this.supplierIdCounter = counter;
        }

        public void AddSupplier()
        {
            Console.Clear();
            Console.WriteLine("===== ADD SUPPLIER =====");

            string name = InputHelper.GetValidLetters("Enter name: ");
            string contact = InputHelper.GetValidNumber("Enter contact: ").ToString();
            string email = InputHelper.GetValidLettersOrDigits("Enter email: ");

            Supplier supplier = new Supplier(supplierIdCounter++, name, contact, email);
            suppliers.Add(supplier);

            Console.WriteLine("Supplier added.");
            Console.ReadKey();
        }

        public void ViewAllSuppliers()
        {
            Console.WriteLine("\n--- Suppliers ---");
            foreach (var s in suppliers)
            {
                Console.WriteLine($"ID: {s.Id} - {s.Name}");
            }
        }
    }
}