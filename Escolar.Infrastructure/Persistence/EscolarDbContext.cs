using Escolar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Escolar.Infrastructure.Persistence;

public class EscolarDbContext : DbContext
{
    public EscolarDbContext(DbContextOptions<EscolarDbContext> options) : base(options)
    {
    }

    public DbSet<Aluno> Alunos => Set<Aluno>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Aluno>(entity =>
        {
            entity.ToTable("alunos");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Cpf)
                .IsRequired()
                .HasMaxLength(11);

            entity.Property(x => x.Email)
                .HasMaxLength(150);

            entity.Property(x => x.Status)
                .IsRequired();

            entity.HasIndex(x => x.Cpf).IsUnique();
        });
    }
}
