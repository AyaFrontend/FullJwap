using Jwap.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jwap.DAL.Data.FluentConfigurations
{
    public class ConnectionConfiguration : IEntityTypeConfiguration<Connections>
    {
        public void Configure(EntityTypeBuilder<Connections> builder)
        {
           builder.HasOne(x => x.User).WithMany(x=> x.UserConnectionIds).HasForeignKey(x => x.UserId);
        }
    }
}
