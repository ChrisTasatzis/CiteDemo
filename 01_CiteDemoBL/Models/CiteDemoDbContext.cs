using Microsoft.EntityFrameworkCore;

namespace CiteDemoBL.Models
{
    public partial class CiteDemoDbContext : DbContext
    {
        public DbSet<CAttribute> CAttributes { get; set; }
        public DbSet<CEmployee> CEmployees { get; set; }

        public CiteDemoDbContext() : base() { }
        public CiteDemoDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Data Source = localhost; Initial Catalog = CiteDemo; User ID = sa; Password = admin!@#123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<EmployeeAttribute>().HasOne(e => e.Employee)
                .WithMany().HasForeignKey(e => e.EMPATTR_EmployeeID);

            modelBuilder.Entity<EmployeeAttribute>().HasOne(e => e.Attribute)
               .WithMany().HasForeignKey(e => e.EMPATTR_AttributeID);


            modelBuilder.Entity<CEmployee>()
                .HasMany(e => e.Attributes)
                .WithMany(a => a.Employees)
                .UsingEntity<EmployeeAttribute>();

        }
    }
}
