using System.Collections.Generic;

namespace InventoryManagementSystem
{
    public class TransactionService
    {
        private List<TransactionRecord> transactions;
        private int transactionIdCounter;

        public TransactionService(List<TransactionRecord> transactions, ref int counter)
        {
            this.transactions = transactions;
            this.transactionIdCounter = counter;
        }

        public void Record(int productId, string name, string type, int qty, string notes)
        {
            transactions.Add(new TransactionRecord(transactionIdCounter++, productId, name, type, qty, notes));
        }

        public List<TransactionRecord> GetAll()
        {
            return transactions;
        }
    }
}