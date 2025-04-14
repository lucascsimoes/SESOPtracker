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
    }
}
