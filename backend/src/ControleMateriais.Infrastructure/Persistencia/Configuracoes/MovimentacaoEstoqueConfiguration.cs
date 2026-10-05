using ControleMateriais.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleMateriais.Infrastructure.Persistencia.Configuracoes;

public class MovimentacaoEstoqueConfiguration
    : IEntityTypeConfiguration<MovimentacaoEstoque>
{
    public void Configure(
        EntityTypeBuilder<MovimentacaoEstoque> builder)
    {
        builder.ToTable("movimentacoes_estoque");

        builder.HasKey(movimentacao => movimentacao.Id);

        builder.Property(movimentacao => movimentacao.Id)
            .HasColumnName("id");

        builder.Property(movimentacao => movimentacao.UnidadeId)
            .HasColumnName("unidade_id")
            .IsRequired();

        builder.Property(movimentacao => movimentacao.MaterialId)
            .HasColumnName("material_id")
            .IsRequired();

        builder.Property(movimentacao => movimentacao.Tipo)
            .HasColumnName("tipo")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(movimentacao => movimentacao.Quantidade)
            .HasColumnName("quantidade")
            .HasPrecision(18, 3)
            .IsRequired();

        builder.Property(movimentacao => movimentacao.DataMovimentacao)
            .HasColumnName("data_movimentacao")
            .IsRequired();

        builder.HasOne<Unidade>()
            .WithMany()
            .HasForeignKey(movimentacao => movimentacao.UnidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Material>()
            .WithMany()
            .HasForeignKey(movimentacao => movimentacao.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(movimentacao => new
        {
            movimentacao.UnidadeId,
            movimentacao.MaterialId
        });
    }
}
