using Jwap.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jwap.DAL.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Message> Messages { set; get; }
        public DbSet<User> AppUsers { set; get; }
        public DbSet<Connections> Connections { set; get; }
        public DbSet<Friends> Friends { set; get; }
        public DbSet<CallOffer> CallOffers { set; get; }

    }
}
