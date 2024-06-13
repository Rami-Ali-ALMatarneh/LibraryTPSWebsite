using System.ComponentModel.DataAnnotations;

namespace LibraryTPSWebsite.Models
    {
    public class shelf
        {
        public int shelfID { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+\s+\d+$", ErrorMessage = "The Name field must contain start with letters and end with Number without Space.")]
        [StringLength(50,ErrorMessage = "Shelf name must be between 3 and 50 characters long.",MinimumLength =3)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<book> books { get; set; }
        }
    }
