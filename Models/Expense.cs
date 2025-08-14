using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExpenseTracker.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50, ErrorMessage ="The title must not exceed 50 characters.")]
        public required string Title { get; set; }
        public decimal Amount { get; set; }
        [DateWithinReason]
        [Required(ErrorMessage = "The date is required.")]
        public DateTime Date { get; set; }
        public required string Category { get; set; }

        [NotMapped]        
        [Display(Name = "Category")]
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Nouriture", Text = "Nouriture" },
            new SelectListItem { Value = "Transport", Text = "Transport" },
            new SelectListItem { Value = "divertissement", Text = "divertissement" },
            new SelectListItem { Value = "Santé", Text = "Santé" },
            new SelectListItem { Value = "Autre", Text = "Autre" }
        };
        [ValidateNever]
        public string UserId { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; }

    }
}
