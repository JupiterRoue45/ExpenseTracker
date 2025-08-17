using ExpenseTracker.Models;
using ExpenseTracker.Models.Dto;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IExpenseService
    {
        //public Task<IEnumerable<Expense>> GetAllExpensesAsync();
        public Task<Expense?> GetExpenseByIdAsync(int id);
        public Task CreateExpenseAsync(Expense expense);
        public Task<bool> UpdateExpenseAsync(string userId, int id, ExpenseUpdateDto dto);
        public Task<bool> DeleteExpenseAsync(string UserId, int id);
        public Task<List<Expense>> GetExpensesByUserIdAsync(string userId);
        public Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(string category);
    }
}
