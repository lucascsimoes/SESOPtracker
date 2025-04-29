using Microsoft.EntityFrameworkCore;
using SESOPtracker.Models.Entities;

namespace SESOPtracker.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }

        public DbSet<Sala> Salas { get; set; }
        public DbSet<Situacao> Situacoes { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<Historico> Historicos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseOracle(configuration.GetConnectionString("DataBaseHml"));
            //optionsBuilder.UseOracle(configuration.GetConnectionString("DataBasePrd"));
        }
    }
}
