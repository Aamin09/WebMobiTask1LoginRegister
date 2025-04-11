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
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<ProductVariant> ProductVariants { get; set; }
    public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }
    public virtual DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
    public virtual DbSet<VariantAttributeValue> VariantAttributeValues { get; set; }
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<GstTax> GstTax { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        ConfigureUserEntity(modelBuilder);

        ConfigureComputedColumns(modelBuilder);

        ConfigureUniqueIndexes(modelBuilder);

        ConfigureDecimalProperties(modelBuilder);

        ConfigureEntityRelationship(modelBuilder);

        ConfigureCreatedAtDateProperties(modelBuilder);

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


        OnModelCreatingPartial(modelBuilder);
    }

    private void ConfigureUserEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.ToTable("userlogin");
            entity.HasIndex(e => e.Id, "email_unique_userlogin").IsUnique();
            entity.Property(e => e.Email).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.Gender).HasMaxLength(6).IsUnicode(false);
            entity.Property(e => e.LastName).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.Phone).HasMaxLength(10).IsFixedLength();
            entity.Property(e => e.Photo).IsUnicode(false);
        });
    }

    private void ConfigureComputedColumns(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>()
       .Property(o => o.TotalPrice)
       .HasComputedColumnSql("[SnapshotPrice] * [Quantity]");
    }

    private void ConfigureUniqueIndexes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>()
           .HasIndex(c => c.UserId)
            .IsUnique();

        modelBuilder.Entity<Order>()
            .HasIndex(c => c.OrderNumber)
             .IsUnique();

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
    }
    private void ConfigureCreatedAtDateProperties(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var createdAtProperties = entityType.GetProperties()
                .Where(p => p.Name == "CreatedAt" &&
                           (p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)));

            // Configure each CreatedAt property with default value of GETDATE() or CURRENT_TIMESTAMP
            foreach (var property in createdAtProperties)
            {
                property.SetDefaultValueSql("GETDATE()");
            }
        }
    }

    private void ConfigureDecimalProperties(ModelBuilder modelBuilder)
    {
        // useing reflection to automatically configure all decimal properties
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var decimalProperties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));

            // configure each element property with precision(18,2)
            foreach (var property in decimalProperties)
            {
                property.SetColumnType("decimal(18,2)");
            }
        }
    }

    private void ConfigureEntityRelationship(ModelBuilder modelBuilder)
    {
        // ProductVariant and ProductImage relationship
        modelBuilder.Entity<ProductImage>()
            .HasOne(pi => pi.ProductVariant)
            .WithMany()
            .HasForeignKey(pi => pi.VariantId)
            .OnDelete(DeleteBehavior.NoAction);

        // Product and ProductVariant relationship
        modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductVariants)
            .WithOne(pv => pv.Product)
            .HasForeignKey(pv => pv.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // ProductAttribute and ProductAttributeValue relationship
        modelBuilder.Entity<ProductAttribute>()
            .HasMany(pa => pa.ProductAttributeValue)
            .WithOne(pav => pav.Attribute)
            .HasForeignKey(pav => pav.AttributeId)
            .OnDelete(DeleteBehavior.Cascade);

        // ProductVariant and VariantAttributeValue relationship
        modelBuilder.Entity<ProductVariant>()
            .HasMany<VariantAttributeValue>()
            .WithOne(vav => vav.ProductVariant)
            .HasForeignKey(vav => vav.VarinatId)
            .OnDelete(DeleteBehavior.Cascade);

        // VariantAttributeValue and ProductAttributeValue relationship
        modelBuilder.Entity<VariantAttributeValue>()
                      .HasOne(vav => vav.ProductAttributeValue)
                      .WithMany()
                      .HasForeignKey(vav => vav.AttrbuteValueId)
                      .OnDelete(DeleteBehavior.Restrict);

        // ProductVariant and OrderItem relationship
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.ProductVariant)
            .WithMany()
            .HasForeignKey(oi => oi.ProductVariantId)
            .OnDelete(DeleteBehavior.NoAction);

        // ProductVariant and Cart relationship
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.ProductVariant)
            .WithMany()
            .HasForeignKey(c => c.ProductVariantId)
            .OnDelete(DeleteBehavior.NoAction);

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

        // Product - Review Relationship
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Reviews)
            .WithOne(r => r.Product)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Userlogin - Review Relationship
        modelBuilder.Entity<Userlogin>()
     .HasMany(u => u.Reviews)
     .WithOne(r => r.User)
     .HasForeignKey(r => r.UserId)
     .OnDelete(DeleteBehavior.Restrict);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
