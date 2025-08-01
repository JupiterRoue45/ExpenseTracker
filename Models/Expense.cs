using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ExpenseTracker.Validation;
{
    
}

namespace ExpenseTracker.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50, ErrorMessage ="The title must not exceed 100 characters.")]
        public required string Title { get; set; }
        public decimal Amount { get; set; }
        [DateWithinReason]
        [Required(ErrorMessage = "The date is required.")]
        public DateTime Date { get; set; }
        public required string Category { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
