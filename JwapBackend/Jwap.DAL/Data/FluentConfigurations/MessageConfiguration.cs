using Jwap.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jwap.DAL.Data.FluentConfigurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Reciever).WithMany().HasForeignKey(x => x.RecieverId);
            builder.HasOne(x => x.Sender).WithMany().HasForeignKey(x => x.SenderId);
            builder.HasIndex(x => x.Messege);
         //   builder.Property(m =>m.MessegeType).HasConversion(mType => mType.ToString(), mType => (MessegeTypes)Enum.Parse(typeof(MessegeTypes), mType));
        }
    }
}
