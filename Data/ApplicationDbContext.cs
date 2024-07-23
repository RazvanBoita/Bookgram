using LearnByDoing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnByDoing.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}

    public DbSet<Book> Books { get; set; }
    public DbSet<UserBook> UserBooks { get; set; }
    public DbSet<WishlistUserBook> WishlistUserBooks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        
        modelBuilder.Entity<AspNetUserLogin>()
            .HasKey(l => new {l.LoginProvider, l.ProviderKey});

        modelBuilder.Entity<Book>()
            .HasKey(b => b.BookId);
        
        modelBuilder.Entity<UserBook>()
            .HasKey(ub => new { ub.UserId, ub.BookId });

        modelBuilder.Entity<UserBook>()
            .HasOne(ub => ub.User)
            .WithMany(u => u.UserBooks)
            .HasForeignKey(ub => ub.UserId);

        modelBuilder.Entity<UserBook>()
            .HasOne(ub => ub.Book)
            .WithMany(u => u.UserBooks)
            .HasForeignKey(ub => ub.BookId);
        
        
        modelBuilder.Entity<WishlistUserBook>()
            .HasKey(ub => new { ub.UserId, ub.BookId });

        modelBuilder.Entity<WishlistUserBook>()
            .HasOne(ub => ub.User)
            .WithMany(u => u.WishlistUserBooks)
            .HasForeignKey(ub => ub.UserId);

        modelBuilder.Entity<WishlistUserBook>()
            .HasOne(ub => ub.Book)
            .WithMany(u => u.WishlistUserBooks)
            .HasForeignKey(ub => ub.BookId);

    }
}