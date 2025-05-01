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
        public DbSet<Permissao> Permissoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder
                .UseOracle(configuration.GetConnectionString("DataBaseHml"))
                .LogTo(Console.WriteLine);

            //optionsBuilder.UseOracle(configuration.GetConnectionString("DataBasePrd"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Equipamento>(entity =>
            {
                entity.ToTable("LUCAS_TRACKER_EQUIPAMENTOS");

                entity.Property(e => e.patrimonio)
                    .HasColumnName("PATRIMONIO");
                entity.Property(e => e.item)
                    .HasColumnName("ITEM");
                entity.Property(e => e.nome)
                    .HasColumnName("NOME");
                entity.Property(e => e.setor)
                    .HasColumnName("SETOR");
                entity.Property(e => e.subCategoria)
                    .HasColumnName("SUBCATEGORIA");
                entity.Property(e => e.categoria)
                    .HasColumnName("CATEGORIA");
                entity.Property(e => e.dataCriacao)
                    .HasColumnName("DATACRIACAO");
                entity.Property(e => e.tag)
                    .HasColumnName("TAG");
                entity.Property(e => e.sala)
                    .HasColumnName("SALA");
                entity.Property(e => e.situacao)
                    .HasColumnName("SITUACAO");
            });

            modelBuilder.Entity<Sala>(entity =>
            {
                entity.ToTable("LUCAS_TRACKER_SALAS");

                entity.Property(e => e.salaId)
                    .HasColumnName("SALAID");
                entity.Property(e => e.local)
                    .HasColumnName("LUGAR");
                entity.Property(e => e.descricao)
                    .HasColumnName("DESCRICAO");
            });

            modelBuilder.Entity<Situacao>(entity =>
            {
                entity.ToTable("LUCAS_TRACKER_SITUACOES");

                entity.Property(e => e.situacaoId)
                    .HasColumnName("SITUACAOID");
                entity.Property(e => e.descricao)
                    .HasColumnName("DESCRICAO");
                entity.Property(e => e.cor)
                    .HasColumnName("COR");
            });

            modelBuilder.Entity<Historico>(entity =>
            {
                entity.ToTable("LUCAS_TRACKER_HISTORICOS");

                entity.Property(e => e.historicoId)
                    .HasColumnName("HISTORICOID");
                entity.Property(e => e.patrimonio)
                    .HasColumnName("PATRIMONIO");
                entity.Property(e => e.dataAlteracao)
                    .HasColumnName("DATAALTERACAO");
                entity.Property(e => e.situacaoAtual)
                    .HasColumnName("SITUACAOATUAL");
                entity.Property(e => e.descricao)
                    .HasColumnName("DESCRICAO");
                entity.Property(e => e.observacao)
                    .HasColumnName("OBSERVACAO");
                entity.Property(e => e.alteradoPor)
                    .HasColumnName("ALTERADOPOR");
                entity.Property(e => e.importante)
                    .HasColumnName("IMPORTANTE")
                    .HasConversion<int>();
            });

            modelBuilder.Entity<Permissao>(entity =>
            {
                entity.ToTable("ADMINISTRADORES_ESAJ");
                entity.HasKey(e => e.codpess);
                entity.Property(e => e.codpess)
                    .HasColumnName("CODPESS");
                entity.Property(e => e.grupo)
                    .HasColumnName("GRUPO");
                entity.Property(e => e.permPatrimonio)
                    .HasColumnName("PERMPATRIMONIO");
            });
        }
    }
}
