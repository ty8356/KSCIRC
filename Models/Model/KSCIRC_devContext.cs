using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace KSCIRC.Models.Model
{
    public partial class KSCIRC_devContext : DbContext
    {
        public KSCIRC_devContext()
        {
        }

        public KSCIRC_devContext(DbContextOptions<KSCIRC_devContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gene> Genes { get; set; }
        public virtual DbSet<Publication> Publications { get; set; }
        public virtual DbSet<StatValue> StatValues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=KSCIRC_dev;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Gene>(entity =>
            {
                entity.ToTable("Gene");

                entity.Property(e => e.EnsId)
                    .IsRequired()
                    .HasMaxLength(18)
                    .HasColumnName("Ens_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(18);
            });

            modelBuilder.Entity<Publication>(entity =>
            {
                entity.ToTable("Publication");

                entity.Property(e => e.Pmid).HasColumnName("PMID");
            });

            modelBuilder.Entity<StatValue>(entity =>
            {
                entity.ToTable("StatValue");

                entity.Property(e => e.EnrichmentQvalue)
                    .HasColumnType("decimal(20, 15)")
                    .HasColumnName("EnrichmentQValue");

                entity.Property(e => e.EnrichmentValue).HasColumnType("decimal(20, 15)");

                entity.Property(e => e.ImmunoprecipitateQvalue)
                    .HasColumnType("decimal(20, 15)")
                    .HasColumnName("ImmunoprecipitateQValue");

                entity.Property(e => e.ImmunoprecipitateValue).HasColumnType("decimal(20, 15)");

                entity.Property(e => e.InputQvalue)
                    .HasColumnType("decimal(20, 15)")
                    .HasColumnName("InputQValue");

                entity.Property(e => e.InputValue).HasColumnType("decimal(20, 15)");

                entity.Property(e => e.InteractionQvalue)
                    .HasColumnType("decimal(20, 15)")
                    .HasColumnName("InteractionQValue");

                entity.Property(e => e.InteractionValue).HasColumnType("decimal(20, 15)");

                entity.HasOne(d => d.Gene)
                    .WithMany(p => p.StatValues)
                    .HasForeignKey(d => d.GeneId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.StatValues)
                    .HasForeignKey(d => d.PublicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
