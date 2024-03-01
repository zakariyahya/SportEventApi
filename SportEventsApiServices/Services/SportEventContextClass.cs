using Microsoft.EntityFrameworkCore;
using SportEventsApiServices.Models;
using SportEventsApiServices.Models.Organizer;
using SportEventsApiServices.Models.User;
namespace SportEventsApiServices.Services
{
    public class SportEventContextClass : DbContext
    {
        public SportEventContextClass(DbContextOptions<SportEventContextClass> opt) : base(opt)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<OrganizerModel> Organizers { get; set; }
        public DbSet<SportEventModel> SportEvents { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }   
}
