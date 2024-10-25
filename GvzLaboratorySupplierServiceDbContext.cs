using Gvz.Laboratory.SupplierService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.SupplierService
{
    public class GvzLaboratorySupplierServiceDbContext : DbContext
    {
        public DbSet<SupplierEntity> Suppliers { get; set; }

        public GvzLaboratorySupplierServiceDbContext(DbContextOptions<GvzLaboratorySupplierServiceDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configuration db
        }
    }
}
