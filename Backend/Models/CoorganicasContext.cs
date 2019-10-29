using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Backend.Models
{
    public partial class CoorganicasContext : DbContext
    {
        public CoorganicasContext()
        {
        }

        public CoorganicasContext(DbContextOptions<CoorganicasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Endereco> Endereco { get; set; }
        public virtual DbSet<Oferta> Oferta { get; set; }
        public virtual DbSet<Produto> Produto { get; set; }
        public virtual DbSet<Receita> Receita { get; set; }
        public virtual DbSet<Reserva> Reserva { get; set; }
        public virtual DbSet<Telefone> Telefone { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LAB08DESK6901\\SQLEXPRESS; Database=Coorganicas; User Id=sa; Password=132");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.Property(e => e.Cep)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Endereco1).IsUnicode(false);

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Endereco)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK__Endereco__Usuari__45F365D3");
            });

            modelBuilder.Entity<Oferta>(entity =>
            {
                entity.Property(e => e.Cidade).IsUnicode(false);

                entity.Property(e => e.Regiao).IsUnicode(false);

                entity.HasOne(d => d.Produto)
                    .WithMany(p => p.Oferta)
                    .HasForeignKey(d => d.ProdutoId)
                    .HasConstraintName("FK__Oferta__Produto___49C3F6B7");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Oferta)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK__Oferta__Usuario___48CFD27E");
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.Property(e => e.Descricao).IsUnicode(false);

                entity.Property(e => e.ImagemProduto).IsUnicode(false);

                entity.Property(e => e.Nome).IsUnicode(false);
            });

            modelBuilder.Entity<Receita>(entity =>
            {
                entity.Property(e => e.ImagemReceita).IsUnicode(false);

                entity.Property(e => e.Titulo).IsUnicode(false);

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Receita)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK__Receita__Usuario__4316F928");
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.Property(e => e.DataReserva).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StatusReserva).IsUnicode(false);

                entity.HasOne(d => d.Oferta)
                    .WithMany(p => p.Reserva)
                    .HasForeignKey(d => d.OfertaId)
                    .HasConstraintName("FK__Reserva__Oferta___4E88ABD4");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Reserva)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK__Reserva__Usuario__4D94879B");
            });

            modelBuilder.Entity<Telefone>(entity =>
            {
                entity.Property(e => e.Telefone1).IsUnicode(false);

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Telefone)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK__Telefone__Usuari__403A8C7D");
            });

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.Property(e => e.Tipo).IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Cnpj)
                    .HasName("UQ__Usuario__AA57D6B46ED250CE")
                    .IsUnique();

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Usuario__A9D10534E02CADD2")
                    .IsUnique();

                entity.Property(e => e.Cnpj)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.ImagemUsuario).IsUnicode(false);

                entity.Property(e => e.Nome).IsUnicode(false);

                entity.Property(e => e.Senha).IsUnicode(false);

                entity.HasOne(d => d.TipoUsuario)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.TipoUsuarioId)
                    .HasConstraintName("FK__Usuario__Tipo_us__3D5E1FD2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
