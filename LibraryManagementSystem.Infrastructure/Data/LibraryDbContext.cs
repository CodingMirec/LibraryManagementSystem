using System;
using System.Collections.Generic;
using LibraryManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Data;

public partial class LibraryDbContext : DbContext
{
    public LibraryDbContext()
    {
    }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=dpg-csbs7n3tq21c73a7crng-a.frankfurt-postgres.render.com;Port=5432;Database=lib_man_db;Username=lib_man_user;Password=KJNHNKKChA6dQ6OZpC7hteFpUdi6OzJi;Ssl Mode=Require");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.PublishedDate).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasIndex(e => e.BookId, "IX_Loans_BookId");

            entity.HasIndex(e => e.UserId, "IX_Loans_UserId");

            entity.Property(e => e.DueDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.IsReturned).HasDefaultValue(false);
            entity.Property(e => e.LoanDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ReturnDate).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Book).WithMany(p => p.Loans)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.User).WithMany(p => p.Loans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
