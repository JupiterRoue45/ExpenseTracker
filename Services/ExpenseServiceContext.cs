using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Models.Dto;


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

        public async Task<bool> UpdateExpenseAsync(string userId, int id, ExpenseUpdateDto dto)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (expense == null)
            {
                return false; // Expense not found or does not belong to the user
            }
            expense.Title = dto.Title;
            expense.Amount = dto.Amount;
            expense.Date = dto.Date;
            expense.Category = dto.Category;
            await _context.SaveChangesAsync();
            return true; // Update successful
        }

        public async Task<bool> DeleteExpenseAsync(string UserId, int id)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == UserId);
            if (expense == null)
            {
               return false; // Expense not found or does not belong to the user
            }
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true; // Deletion successful
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
                .AsNoTracking()
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
                .AsNoTracking()
                .ToListAsync();
            return expenses;
        }
    }
}
