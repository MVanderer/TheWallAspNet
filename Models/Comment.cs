using System;
using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class Comment
    {
        [Key]
        public int CommentId{get;set;}
        [Required]
        public string CommentText{get;set;}
        public int AuthorId{get;set;}
        public User Author{get;set;}
        public int MessageId{get;set;}
        public Message Message{get;set;}
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}