using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ExpenseTracker.Services.Interfaces;


namespace ExpenseTracker.Services
{
    public class ExpenseServiceContext : IExpenseService
    {
        private readonly ApplicationDbContext _context;
        public ExpenseServiceContext(ApplicationDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            var expenses = await _context.Expenses.ToListAsync();

            return expenses;
        }

        public async Task CreateExpenseAsync(Expense expense)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }
            var existingExpense = await _context.Expenses.FindAsync(expense.Id);
            if (existingExpense == null )
            {
                throw new KeyNotFoundException($"Expense with ID {expense.Id} not found.");
            }
            _context.Entry(existingExpense).CurrentValues.SetValues(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExpenseAsync(int id)
        {
            var existingExpense = await _context.Expenses.FindAsync(id);
            if (existingExpense == null)
            {
                throw new KeyNotFoundException($"Expense with ID {id} not found.");
            }
            _context.Expenses.Remove(existingExpense);
            await _context.SaveChangesAsync();
        }

        public async Task<Expense?> GetExpenseByIdAsync(int id)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id);
            return expense;
        }

        public async Task<List<Expense>> GetExpensesByUserIdAsync(string userId)
        {
            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .ToListAsync();
            return expenses;
        }

        public async Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException(nameof(category));
            }
            var expenses = await _context.Expenses
                .Where(e => e.Category == category)
                .ToListAsync();
            return expenses;
        }
    }
}
