using ControleMateriais.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleMateriais.Infrastructure.Persistencia.Configuracoes;

public class UnidadeConfiguration : IEntityTypeConfiguration<Unidade>
{
    public void Configure(EntityTypeBuilder<Unidade> builder)
    {
        builder.ToTable("unidades");

        builder.HasKey(unidade => unidade.Id);

        builder.Property(unidade => unidade.Id)
            .HasColumnName("id");

        builder.Property(unidade => unidade.Nome)
            .HasColumnName("nome")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(unidade => unidade.Ativa)
            .HasColumnName("ativa")
            .IsRequired();
    }
}
