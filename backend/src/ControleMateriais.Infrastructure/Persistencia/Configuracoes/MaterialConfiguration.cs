using ControleMateriais.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleMateriais.Infrastructure.Persistencia.Configuracoes;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("materiais");

        builder.HasKey(material => material.Id);

        builder.Property(material => material.Id)
            .HasColumnName("id");

        builder.Property(material => material.Nome)
            .HasColumnName("nome")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(material => material.UnidadeMedida)
            .HasColumnName("unidade_medida")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(material => material.Ativo)
            .HasColumnName("ativo")
            .IsRequired();
    }
}
