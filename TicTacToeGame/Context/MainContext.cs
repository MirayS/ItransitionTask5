using Microsoft.EntityFrameworkCore;
using TicTacToeGame.Context.Models;

namespace TicTacToeGame.Context
{
    public class MainContext : DbContext
    {
        public DbSet<Room> Rooms;
        public DbSet<Tag> Tags;

        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomTag>()
                .HasKey(t => new { t.RoomId, t.TagId });
 
            modelBuilder.Entity<RoomTag>()
                .HasOne(sc => sc.Room)
                .WithMany(s => s.RoomTags)
                .HasForeignKey(sc => sc.RoomId);
 
            modelBuilder.Entity<RoomTag>()
                .HasOne(sc => sc.Tag)
                .WithMany(c => c.RoomTags)
                .HasForeignKey(sc => sc.TagId);
        }
    }
}