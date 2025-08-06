using ExpenseTracker.Models;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IExpenseService
    {
        //public Task<IEnumerable<Expense>> GetAllExpensesAsync();
        public Task<Expense?> GetExpenseByIdAsync(int id);
        public Task CreateExpenseAsync(Expense expense);
        public Task UpdateExpenseAsync(Expense expense);
        public Task DeleteExpenseAsync(int id);
        public Task<List<Expense>> GetExpensesByUserIdAsync(string userId);
        public Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(string category);
    }
}
