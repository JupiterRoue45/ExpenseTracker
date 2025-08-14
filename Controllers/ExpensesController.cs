using System.Threading.Tasks;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        /*
        [Authorize]
        public IActionResult Index()
        {
            return Content("Bienvenue sur ExpenseTracker !");
        }
        */

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
                var errors = ModelState
                .Where(kv => kv.Value.Errors.Count > 0)
                .Select(kv => new { Field = kv.Key, Errors = kv.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                .ToList();
                foreach(var error in errors)
                {
                    Console.WriteLine(error);
                }
                // Rester sur la vue pour afficher les erreurs
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Amount,Date,Category")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _expenseService.UpdateExpenseAsync(expense);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _expenseService.GetExpenseByIdAsync(expense.Id) == null)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }

    }
}
