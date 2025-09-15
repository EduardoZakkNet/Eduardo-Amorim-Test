using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sale");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(u => u.SaleDate)
            .IsRequired();
        
        builder.Property(u => u.TotalSaleAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        builder.HasOne(u => u.Branch)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(u => u.Customer)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.Products)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "SaleProduct",
                j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Sale>().WithMany().HasForeignKey("SaleId").OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("SaleId", "ProductId");
                    j.ToTable("SaleProduct");
                });
        
        builder.Property(u => u.IsCancelled)
            .IsRequired();
        
        builder.Property(b => b.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()");

        builder.Property(b => b.UpdatedAt)
            .HasColumnType("timestamp without time zone");
        
        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}