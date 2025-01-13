using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Task1LoginRegister.Models;

public partial class WebMobiTask1DbContext : DbContext
{
    public WebMobiTask1DbContext()
    {
    }

    public WebMobiTask1DbContext(DbContextOptions<WebMobiTask1DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Userlogin> Userlogins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=dbcs");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.ToTable("userlogin");

            entity.HasIndex(e => e.Id, "email_unique_userlogin").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Photo).IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
