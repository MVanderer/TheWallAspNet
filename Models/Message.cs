using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheWall.Models {
    public class Message {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public string MessageText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int AuthorId{get;set;}
        public User Author {get;set;}
        public List<Comment> Comments{get;set;}
    }
}