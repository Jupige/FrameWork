using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BaseFramework.Persistence;
using CustomerBaseSolution;

namespace CB.Service.Persistence
{
    public class CBDbContext : DbContext
    {
        public DbSet<TestEntity> TestEntitys { get; set; }

        public CBDbContext()
            : this("CBDevDb")
        {

        }

        public CBDbContext(string connection)
            : base(connection)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}