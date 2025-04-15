using Microsoft.EntityFrameworkCore;
using CheckOut.Models;

namespace CheckOut.Data 
{
    public class CheckOutContext : DbContext
    {
        public CheckOutContext(DbContextOptions<CheckOutContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Checklist> Checklists {get; set;}
        public DbSet<ToDo> ToDos {get; set;}
    }
}
