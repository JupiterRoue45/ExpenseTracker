using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Models
{
    public class ApplicationUser : IdentityUser
    {

        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        [Range(18, 100, ErrorMessage = "Minors and older than 100 years old are not allowed!")]
        public required int Age { get; set; }
        public required string Occupation { get; set; } 
    }
}
