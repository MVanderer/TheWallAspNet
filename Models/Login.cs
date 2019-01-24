using System.ComponentModel.DataAnnotations;

namespace TheWall.Models {
    public class Login {
        [EmailAddress]
        [Required]
        [Display (Name = "Email")]
        public string LoginEmail { get; set; }

        [DataType (DataType.Password)]
        [Required]
        [Display (Name = "Password")]
        [MinLength (8, ErrorMessage = "Password's too short")]
        public string LoginPassword { get; set; }
    }
}