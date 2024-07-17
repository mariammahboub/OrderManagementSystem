using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Repository.Config
{
    internal class PaymentResultConfig : IEntityTypeConfiguration<PaymentResult>
    {
        public void Configure(EntityTypeBuilder<PaymentResult> builder)
        {
            builder.Property(PaymentResult => PaymentResult.ShippingPrice)
                .HasColumnType("decimal(18, 2)");
        }
    }
}
