using ExpenseTracker.Models;
using ExpenseTracker.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Utilities
{
    public static class ExpenseMapper
    {
        public static ExpenseUpdateDto ToExpenseUpdateDto(this Expense expense)
        {
            if (expense == null) throw new ArgumentNullException(nameof(expense));
            return new ExpenseUpdateDto
            {
                Title = expense.Title,
                Amount = expense.Amount,
                Date = expense.Date,
                Category = expense.Category,
                Categories = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Nouriture", Text = "Nouriture" },
                    new SelectListItem { Value = "Transport", Text = "Transport" },
                    new SelectListItem { Value = "divertissement", Text = "divertissement" },
                    new SelectListItem { Value = "Santé", Text = "Santé" },
                    new SelectListItem { Value = "Autre", Text = "Autre" }
                }
            };
        }
    }
}
