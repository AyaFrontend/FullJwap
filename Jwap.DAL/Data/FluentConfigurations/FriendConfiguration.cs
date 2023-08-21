using Jwap.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jwap.DAL.Data.FluentConfigurations
{
    public class FriendConfiguration : IEntityTypeConfiguration<Friends>
    {
        public void Configure(EntityTypeBuilder<Friends> builder)
        {
            builder.HasKey(k => new { k.FreindId, k.UserId });
            
               
            
        }
    }
}
