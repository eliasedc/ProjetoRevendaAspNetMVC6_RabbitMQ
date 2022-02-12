using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjetoRevenda.Models.DB
{
    public partial class BDDesenvolContext : DbContext
    {
        public BDDesenvolContext()
        {
        }

        public BDDesenvolContext(DbContextOptions<BDDesenvolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Marca> Marcas { get; set; } = null!;
        public virtual DbSet<Proprietario> Proprietarios { get; set; } = null!;
        public virtual DbSet<Veiculo> Veiculos { get; set; } = null!;
        public virtual DbSet<VeiculoStatus> VeiculoStatuses { get; set; } = null!;

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=BDDesenvol;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Marca>(entity =>
            {
                entity.Property(e => e.Nome).HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Proprietario>(entity =>
            {
                entity.Property(e => e.Complemento).HasMaxLength(100);

                entity.Property(e => e.Documento).HasMaxLength(20);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Endereco).HasMaxLength(250);

                entity.Property(e => e.Nome).HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Veiculo>(entity =>
            {
                entity.Property(e => e.Modelo).HasMaxLength(100);

                entity.Property(e => e.Valor).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.Marca)
                    .WithMany(p => p.Veiculos)
                    .HasForeignKey(d => d.MarcaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Marca_Veiculos");

                entity.HasOne(d => d.Proprietario)
                    .WithMany(p => p.Veiculos)
                    .HasForeignKey(d => d.ProprietarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proprietario_Veiculo");

                entity.HasOne(d => d.StatusVeiculo)
                    .WithMany(p => p.Veiculos)
                    .HasForeignKey(d => d.StatusVeiculoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VeiculoStatus_Veiculos");
            });

            modelBuilder.Entity<VeiculoStatus>(entity =>
            {
                entity.ToTable("VeiculoStatus");

                entity.Property(e => e.DescricaoStatus).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
