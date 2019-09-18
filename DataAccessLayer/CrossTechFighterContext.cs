using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccessLayer
{
    public partial class CrossTechFighterContext : DbContext
    {
        public CrossTechFighterContext()
        {
        }

        public CrossTechFighterContext(DbContextOptions<CrossTechFighterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ability> Abilities { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Fight> Fights { get; set; }
        public virtual DbSet<FightLog> FightLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CrossTechFighter;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Ability>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Abilities)
                    .HasForeignKey(d => d.PlayerId);
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Fight>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Fights)
                    .HasForeignKey(d => d.PlayerId);
            });

            modelBuilder.Entity<FightLog>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Fight)
                .WithMany(p => p.FightLogs)
                .HasForeignKey(d => d.FightId);
            });
        }
    }
}
