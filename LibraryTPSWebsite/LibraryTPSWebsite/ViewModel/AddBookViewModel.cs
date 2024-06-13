using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace LibraryTPSWebsite.ViewModel
    {
    public class AddBookViewModel
        {
        [Required]
        [RegularExpression(@"[A-Za-z\s*\.*\#*\+*\d*(*)*]+", ErrorMessage = "The Name field must start with letters, may optionally include '.', '#', '+',digits and may end with a optional space .")]
        [StringLength(50, ErrorMessage = "Shelf name must be between 3 and 50 characters long.", MinimumLength = 3)]
        public string BookName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\d*\s*\.*,*!*\#*\+*]+", ErrorMessage = "The Title field must contain only letters, numbers, and spaces.")]
        [StringLength(50, ErrorMessage = "Shelf name must be between 3 and 50 characters long.", MinimumLength = 3)]
        public string Title { get; set; }
        [Required]
        public IFormFile image { get; set; }
        }
    }
