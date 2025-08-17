using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ExpenseTracker.Models.Dto
{
    public class ExpenseUpdateDto
    {
        public required string Title { get; set; }
        public decimal Amount { get; set; }

        [DateWithinReason]
        [Required(ErrorMessage = "The date is required.")]
        public DateTime Date { get; set; }
        public required string Category { get; set; }
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Nouriture", Text = "Nouriture" },
            new SelectListItem { Value = "Transport", Text = "Transport" },
            new SelectListItem { Value = "divertissement", Text = "divertissement" },
            new SelectListItem { Value = "Santé", Text = "Santé" },
            new SelectListItem { Value = "Autre", Text = "Autre" }
        };

    }
}
