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

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Subcategory> Subcategories {  get; set; }
    public virtual DbSet<ProductImage> ProductImages { get; set; }
   
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

        // Product Price Configuration
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Product>()
            .Property(p => p.SellingPricePercent)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Product>()
            .Property(p => p.CalculatedSellingPrice)
            .HasColumnType("decimal(18,2)");

        // Product - Category Relationship 
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Product - Subcategory Relationship
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Subcategory)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SubcategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Category - Subcategory Relationship
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Subcategories)
            .WithOne(s => s.Category)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        // Product - ProductImage Relationship
        modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductImages)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.ToTable("ProductImages");

            entity.Property(e => e.ImageUrl)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.IsPrimaryImage)
                .HasDefaultValue(false);
        });
        modelBuilder.Entity<Category>()
           .HasIndex(c => c.Name)
           .IsUnique()
           .HasDatabaseName("Category_Name_Unique");
        modelBuilder.Entity<Product>()
          .HasIndex(c => c.SKU)
          .IsUnique()
          .HasDatabaseName("Product_SKU_Unique");

        modelBuilder.Entity<Subcategory>()
            .HasIndex(s => s.Name)
            .IsUnique()
            .HasDatabaseName("Subcategory_Name_Unique");

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name)
            .IsUnique()
            .HasDatabaseName("Product_Name_Unique");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
