using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KSCIRC.Models.Model
{
    public partial class hetmanlabdbContext : DbContext
    {
        public hetmanlabdbContext()
        {
        }

        public hetmanlabdbContext(DbContextOptions<hetmanlabdbContext> options)
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
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gene>(entity =>
            {
                entity.ToTable("Gene");

                entity.Property(e => e.EnsId)
                    .IsRequired()
                    .HasColumnName("Ens_Id")
                    .HasMaxLength(18);

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
                    .HasColumnName("EnrichmentQValue")
                    .HasColumnType("decimal(20, 15)");

                entity.Property(e => e.EnrichmentValue).HasColumnType("decimal(20, 15)");

                entity.Property(e => e.ImmunoprecipitateQvalue)
                    .HasColumnName("ImmunoprecipitateQValue")
                    .HasColumnType("decimal(20, 15)");

                entity.Property(e => e.ImmunoprecipitateValue).HasColumnType("decimal(20, 15)");

                entity.Property(e => e.InputQvalue)
                    .HasColumnName("InputQValue")
                    .HasColumnType("decimal(20, 15)");

                entity.Property(e => e.InputValue).HasColumnType("decimal(20, 15)");

                entity.Property(e => e.Interaction2x4Qvalue)
                    .HasColumnName("Interaction2x4QValue")
                    .HasColumnType("decimal(20, 15)");

                entity.Property(e => e.Interaction2x4Value).HasColumnType("decimal(20, 15)");

                entity.Property(e => e.InteractionQvalue)
                    .HasColumnName("InteractionQValue")
                    .HasColumnType("decimal(20, 15)");

                entity.Property(e => e.InteractionValue).HasColumnType("decimal(20, 15)");

                entity.Property(e => e.InputMedianReadCount).HasColumnType("decimal(35, 20)");

                entity.Property(e => e.ImmunoprecipitateMedianReadCount).HasColumnType("decimal(35, 20)");

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
