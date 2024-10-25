using LibraryManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Ensure auto-generated IDs
        //    modelBuilder.Entity<Book>()
        //        .Property(b => b.Id)
        //        .ValueGeneratedOnAdd();

        //    modelBuilder.Entity<Book>()
        //        .Property(b => b.PublishedDate)
        //        .HasColumnType("timestamp without time zone");

        //    modelBuilder.Entity<Loan>()
        //        .HasOne<Book>()
        //        .WithMany()
        //        .HasForeignKey(l => l.BookId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<Loan>()
        //        .HasOne<User>()
        //        .WithMany()
        //        .HasForeignKey(l => l.UserId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<Loan>()
        //        .Property(l => l.Id)
        //        .ValueGeneratedOnAdd();

        //    modelBuilder.Entity<Loan>()
        //        .Property(b => b.LoanDate)
        //        .HasColumnType("timestamp without time zone");

        //    modelBuilder.Entity<Loan>()
        //        .Property(b => b.DueDate)
        //        .HasColumnType("timestamp without time zone");

        //    modelBuilder.Entity<User>()
        //        .Property(u => u.Id)
        //        .ValueGeneratedOnAdd();

        //    // Seed data without specifying Ids
        //    modelBuilder.Entity<User>().HasData(
        //        new User
        //        {
        //            FirstName = "Bob",
        //            LastName = "Stone",
        //            EmailAddress = "bob.stone@example.com"
        //        }
        //    );

        //    modelBuilder.Entity<Book>().HasData(
        //        new Book
        //        {
        //            Title = "How to beat casino 101",
        //            Author = "Johny Ace",
        //            IsBorrowed = false,
        //            PublishedDate = new DateTime(2016, 1, 1)
        //        }
        //    );

        //    modelBuilder.Entity<Loan>().HasData(
        //        new Loan
        //        {
        //            BookId = 1,
        //            UserId = 1,
        //            LoanDate = DateTime.Now,
        //            DueDate = DateTime.Now.AddDays(14)
        //        }
        //    );
        //}
    }
}
