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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .Property(b => b.PublishedDate)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Loan>()
                .HasOne<Book>()
                .WithMany()
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Loan>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Loan>()
                .Property(b => b.LoanDate)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Loan>()
                .Property(b => b.DueDate)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<User>().HasData(
               new User
               {
                   Id = 1,
                   FirstName = "John",
                   LastName = "Doe",
                   EmailAddress = "john.doe@example.com"
               }
           );

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "C# Programming",
                    Author = "John Smith",
                    IsBorrowed = true,
                    PublishedDate = new DateTime(2020, 1, 1)
                }
            );

            modelBuilder.Entity<Loan>().HasData(
                new Loan
                {
                    Id = 1,
                    BookId = 1,
                    UserId = 1,
                    LoanDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14)
                }
            );
        }
    }
}
