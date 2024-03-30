using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Orion.Data
{
    public class DataContext : DbContext
    {
        private IConfiguration _configuration;

        public DataContext()
        {

        }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserData> Users { get; set; }
        public DbSet<AdminUserData> AdminUsers { get; set; }
        public DbSet<ProjectData> ProjectData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (options.IsConfigured)
            {
                //means that context has been added during dependency injection and no further action required.

            }
            else
            {

                Console.WriteLine(Assembly.GetExecutingAssembly().Location);
                var appSettingsJson =
                    Path.Combine(Assembly.GetExecutingAssembly().Location,
                        @"..\..\..\..\..\Orion\appsettings.json");

                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>(entity => entity.HasIndex(p => p.Email).IsUnique());
            modelBuilder.Entity<ProjectData>(entity => entity.HasIndex(p => p.Id).IsUnique());
            modelBuilder.Entity<AdminUserData>(entity => entity.HasIndex(p => p.Id).IsUnique());
        }
    }
}
