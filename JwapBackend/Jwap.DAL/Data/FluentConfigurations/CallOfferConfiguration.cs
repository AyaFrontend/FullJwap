using Jwap.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jwap.DAL.Data.FluentConfigurations
{
    public class CallOfferConfiguration : IEntityTypeConfiguration<CallOffer>
    {
        public void Configure(EntityTypeBuilder<CallOffer> builder)
        {
            builder.HasOne(x => x.Caller).WithMany().HasForeignKey(x => x.CallerId);
            builder.HasOne(x => x.Callee).WithMany().HasForeignKey(x => x.CalleeId);
        }
    }
}
