using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.Models;

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
    public virtual DbSet<Subcategory> Subcategories { get; set; }
    public virtual DbSet<ProductImage> ProductImages { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<DeliveryAddress> DeliveryAddresses { get; set; }

    public virtual DbSet<RazorpayOrderModel> RazorpayOrders { get; set; }

    public virtual DbSet<RefundDetailsModel> RefundDetails { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }
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

        modelBuilder.Entity<OrderItem>()
        .Property(o => o.TotalPrice)
        .HasComputedColumnSql("[Price] * [Quantity]");

        modelBuilder.Entity<Cart>()
            .HasIndex(c => c.UserId )
             .IsUnique();
        //Order configuration 
        modelBuilder.Entity<Order>()
            .HasIndex(c =>  c.OrderNumber)
             .IsUnique();

        modelBuilder.Entity<Order>()
         .Property(p => p.TotalAmount)
         .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<OrderItem>()
          .Property(p => p.Price)
          .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<OrderItem>()
          .Property(p => p.TotalPrice)
          .HasColumnType("decimal(18,2)");

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

        modelBuilder.Entity<Cart>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Product>()
           .Property(p => p.DeliveryCharge)
           .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<GstTax>()
          .Property(p => p.CGST)
          .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<GstTax>()
          .Property(p => p.SGST)
          .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<RazorpayOrderModel>()
           .Property(p => p.Amount)
           .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<RefundDetailsModel>()
           .Property(p => p.Amount)
           .HasColumnType("decimal(18,2)");

        // order and refund razorpay 
        modelBuilder.Entity<RefundDetailsModel>()
              .HasOne(r => r.Order)
              .WithMany(o => o.RefundDetails) 
              .HasForeignKey(r => r.OrderId)
              .OnDelete(DeleteBehavior.Cascade);

        // order razor pay relationship 
        modelBuilder.Entity<Order>()
            .HasOne(o => o.RazorpayOrder)
            .WithOne(ro => ro.Order)
            .HasForeignKey<RazorpayOrderModel>(ro => ro.ApplicationOrderId)
            .OnDelete(DeleteBehavior.Cascade); // When an Order is deleted, the RazorpayOrder is also deleted

        // Product - Category Relationship 
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Subcategory and Gst
        modelBuilder.Entity<Subcategory>()
              .HasOne(s => s.Taxes)
              .WithOne(g => g.Subcategory)
              .HasForeignKey<GstTax>(g => g.SubcategoryId)
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

        // Cart and user relationship
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithMany(p => p.Carts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // product cart relationship
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.Product)
            .WithMany(p => p.Carts)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Userlogin and Orders (One-to-Many)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Userlogin and DeliveryAddresses (One-to-Many)
        modelBuilder.Entity<DeliveryAddress>()
            .HasOne(d => d.User)
            .WithMany(u => u.DeliveryAddresses)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product and OrderItem (One-to-Many)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Order and OrderItem (One-to-Many)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
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

public DbSet<Task1LoginRegister.Models.GstTax> GstTax { get; set; } = default!;
}
