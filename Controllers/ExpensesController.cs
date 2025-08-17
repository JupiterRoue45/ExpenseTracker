using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Models.Dto;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Utilities;

namespace ExpenseTracker.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly IExpenseService _expenseService;

        public ExpensesController(ApplicationDbContext context, IUserContextService userContextService, IExpenseService expenseService)
        {
            // Constructor logic can be added here if needed
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
            _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            string? userId = await _userContextService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return Unauthorized();
            }
            var expenses = await _expenseService.GetExpensesByUserIdAsync(userId);
            return View(expenses);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Amount,Date,Category")] Expense expense)
        {
            var userId = await _userContextService.GetCurrentUserIdAsync();
            if (userId == null) return Unauthorized();

            // Associer l'utilisateur
            expense.UserId = userId;

            if (!ModelState.IsValid)
            {
                return View(expense);
            }

            await _expenseService.CreateExpenseAsync(expense);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new Expense
            {
                Title = "",
                Amount = 0,
                Category = "",
                Date = DateTime.Today
                // Pas besoin d'initialiser Categories ici : ton initialiseur [NotMapped] le fait déjà
            };
            return View(model);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Amount,Date,Category")] ExpenseUpdateDto expense)
        {
            var userId = await _userContextService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                // Rester sur la vue pour afficher les erreurs
                return View(expense);
            }
            bool updated = await _expenseService.UpdateExpenseAsync(userId, id, expense);
            if (!updated)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));


        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            var userId = await _userContextService.GetCurrentUserIdAsync();
            if (expense == null || expense.UserId != userId)
            {
                return NotFound();
            }
            return View(ExpenseMapper.ToExpenseUpdateDto(expense));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = await _userContextService.GetCurrentUserIdAsync();
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (userId == null || expense?.UserId != userId)
            {
                return NotFound();
            }
            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = await _userContextService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return Unauthorized();
            }
            bool deleted = await _expenseService.DeleteExpenseAsync(userId, id);
            if (!deleted)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));

        }
    }
}
