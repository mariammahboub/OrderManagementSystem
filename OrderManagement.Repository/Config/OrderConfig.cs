using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Core.Entities;

namespace OrderManagement.Repository.Config
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Primary Key
            builder.HasKey(o => o.Id);

            // Properties
            builder.Property(o => o.OrderDate)
                   .IsRequired();

            builder.Property(o => o.TotalAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(o => o.PaymentMethod)
                   .IsRequired();

            builder.Property(o => o.Status)
                   .HasConversion(
                        v => v.ToString(),
                        v => Enum.Parse<Order.OrderStatus>(v))
                   .IsRequired();

            // Relationships
            builder.HasOne(o => o.Customer)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade); // Optional: Set cascade delete

            builder.HasOne(o => o.Invoice)
                   .WithOne(i => i.Order)
                   .HasForeignKey<Invoice>(i => i.OrderId)
                   .OnDelete(DeleteBehavior.Cascade); // Optional: Set cascade delete

            builder.HasMany(o => o.OrderItems)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade); // Optional: Set cascade delete
        }
    }
}
