using System;
using System.Collections.Generic;
using Library_Management_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_Application.Data;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PK__Book__447D36EB6341042A");

            entity.ToTable("Book");

            entity.Property(e => e.Isbn)
                .HasMaxLength(12)
                .HasColumnName("ISBN");
            entity.Property(e => e.Author).HasMaxLength(80);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Genre).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BookCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Book__CreatedBy__6EF57B66");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BookUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Book__UpdatedBy__6EF57B66");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Member__0CF04B189543A308");

            entity.ToTable("Member");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .HasColumnName("EMail");
            entity.Property(e => e.Name).HasMaxLength(80);
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
            entity.Property(e => e.RoleType).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6B0B2ED767");

            entity.ToTable("Transaction");

            entity.Property(e => e.BookIsbn)
                .HasMaxLength(12)
                .HasColumnName("BookISBN");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.BookIsbnNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.BookIsbn)
                .HasConstraintName("FK__Transacti__BookI__52593CB8");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TransactionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("Fo_KEY");

            entity.HasOne(d => d.Member).WithMany(p => p.TransactionMembers)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK__Transacti__Membe__534D60F1");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TransactionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Transacti__Updat__70DDC3D8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
