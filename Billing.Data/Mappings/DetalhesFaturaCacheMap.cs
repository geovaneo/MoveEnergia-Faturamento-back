using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class DetalhesFaturaCacheMap : IEntityTypeConfiguration<DetalhesFaturaCache>
    {
        public void Configure(EntityTypeBuilder<DetalhesFaturaCache> builder)
        {
            builder.ToTable("DetalhesFaturaCache", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.NomeConta)
                   .HasColumnName("NomeConta")
                   .IsRequired(false);

            builder.Property(x => x.Uc)
                   .HasColumnName("Uc")
                   .IsRequired(false);

            builder.Property(x => x.Status)
                   .HasColumnName("Status")
                   .IsRequired(false);

            builder.Property(x => x.EmailProprietario)
                   .HasColumnName("EmailProprietario")
                   .IsRequired(false);

            builder.Property(x => x.DocumentoProprietario)
                   .HasColumnName("DocumentoProprietario")
                   .IsRequired(false);

            builder.Property(x => x.DataEmissao)
                   .HasColumnName("DataEmissao")
                   .IsRequired(false);

            builder.Property(x => x.DataVencimento)
                   .HasColumnName("DataVencimento")
                   .IsRequired(false);

            builder.Property(x => x.Total)
                   .HasColumnName("Total")
                   .IsRequired(false);

            builder.Property(x => x.Emitida)
                   .HasColumnName("Emitida")
                   .IsRequired(true);

            builder.Property(x => x.TemAumento)
                   .HasColumnName("TemAumento")
                   .IsRequired(true);

            builder.Property(x => x.TemDesconto)
                   .HasColumnName("TemDesconto")
                   .IsRequired(true);

            builder.Property(x => x.TemAnotacaoFatura)
                   .HasColumnName("TemAnotacaoFatura")
                   .IsRequired(true);

            builder.Property(x => x.TemAnotacaoGerente)
                   .HasColumnName("TemAnotacaoGerente")
                   .IsRequired(true);

            builder.Property(x => x.MesReferencia)
                   .HasColumnName("MesReferencia")
                   .IsRequired(true);

            builder.Property(x => x.NecessitaRecalculo)
                   .HasColumnName("NecessitaRecalculo")
                   .IsRequired(true);

            builder.Property(x => x.TemEscritorioPagamentoAtivo)
                   .HasColumnName("TemEscritorioPagamentoAtivo")
                   .IsRequired(true);

            builder.Property(x => x.FaturaInterna)
                   .HasColumnName("FaturaInterna")
                   .IsRequired(true);

            builder.Property(x => x.FaturaFraca)
                   .HasColumnName("FaturaFraca")
                   .IsRequired(true);

            builder.Property(x => x.TemInjecaoEnergiaPropria)
                   .HasColumnName("TemInjecaoEnergiaPropria")
                   .IsRequired(true);

            builder.Property(x => x.IdConta)
                   .HasColumnName("IdConta")
                   .IsRequired(true);

            builder.Property(x => x.Rateio)
                   .HasColumnName("Rateio")
                   .IsRequired(false);

            builder.Property(x => x.AnotacaoFatura)
                   .HasColumnName("AnotacaoFatura")
                   .IsRequired(false);

            builder.Property(x => x.UrlArquivo)
                   .HasColumnName("UrlArquivo")
                   .IsRequired(false);

            builder.Property(x => x.SubOrigem)
                   .HasColumnName("SubOrigem")
                   .IsRequired(false);

            builder.Property(x => x.FaturaId)
                   .HasColumnName("FaturaId")
                   .IsRequired(true);

            builder.Property(x => x.Hash)
                   .HasColumnName("Hash")
                   .IsRequired(false);

            builder.Property(x => x.SemUCNoRD)
                   .HasColumnName("SemUCNoRD")
                   .IsRequired(true);

            builder.Property(x => x.Excluida)
                   .HasColumnName("Excluida")
                   .IsRequired(true);

            builder.Property(x => x.QuantidadeFaturas)
                   .HasColumnName("QuantidadeFaturas")
                   .IsRequired(true);

            builder.Property(x => x.Distribuidora)
                   .HasColumnName("Distribuidora")
                   .IsRequired(false);

            builder.Property(x => x.QtEnergiaCompensadaHfp)
                   .HasColumnName("QT_ENERGIA_COMPENSADA_HFP")
                   .IsRequired(false);

            builder.Property(x => x.VlEconomia)
                   .HasColumnName("VL_ECONOMIA")
                   .IsRequired(false);

            builder.Property(x => x.VlItemRegra)
                   .HasColumnName("VL_ITEM_REGRA")
                   .IsRequired(false);

            builder.Property(x => x.VlSemConsorcio)
                   .HasColumnName("VL_SEM_CONSORCIO")
                   .IsRequired(false);

            builder.Property(x => x.VlTarEnergiaInjetadaHfp)
                   .HasColumnName("VL_TAR_ENERGIA_INJETADA_HFP")
                   .IsRequired(false);

            builder.Property(x => x.VlTarEnergiainjetadaHfpDescontoAplicado)
                   .HasColumnName("VL_TAR_ENERGIA_INJETADA_HFP_DESCONTO_APLICADO")
                   .IsRequired(false);

            builder.Property(x => x.DistribuidoraRD)
                   .HasColumnName("DistribuidoraRD")
                   .IsRequired(false);

            builder.Property(x => x.Atualizacao)
                   .HasColumnName("Atualizacao")
                   .IsRequired(false);

            builder.Property(x => x.Criacao)
                   .HasColumnName("Criacao")
                   .IsRequired(true);

        }
    }
}
