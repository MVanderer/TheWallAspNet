using Microsoft.EntityFrameworkCore;
using TheWall.Models;

namespace TheWall.Data {
    public class DataContext : DbContext {
        public DataContext (DbContextOptions options) : base (options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages {get;set;}
        public DbSet<Comment> Comments{get;set;}

    }
}