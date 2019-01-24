using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheWall.Models
{
    public class User
    {
        //Universal user fields
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [DataType (DataType.Password)]
        [Required]
        [MinLength (8, ErrorMessage = "Password's too short")]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        [Compare ("Password", ErrorMessage = "Passwords do not match")]
        [DataType (DataType.Password)]
        [Display (Name = "Confirm Password")]
        public string Confirm { get; set; }

        //Custom fields
        public List<Message> Messages{get;set;}
        public List<Comment> Comments{get;set;}
    }
}