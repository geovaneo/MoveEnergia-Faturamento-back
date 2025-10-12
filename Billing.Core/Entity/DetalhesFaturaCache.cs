using System.ComponentModel.DataAnnotations.Schema;

namespace MoveEnergia.Billing.Core.Entity
{
    public class DetalhesFaturaCache
    {
        public int Id { get; set; }
        public int FaturaId { get; set; }
        public string NomeConta { get; set; }
        public string Uc { get; set; }
        public string Status { get; set; }
        public string EmailProprietario { get; set; }
        public string DocumentoProprietario { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataVencimento { get; set; }
        [Column(TypeName = "DECIMAL(18,3)")]
        public decimal? Total { get; set; }
        public bool Emitida { get; set; }
        public bool TemAumento { get; set; }
        public bool TemDesconto { get; set; }
        public bool TemAnotacaoFatura { get; set; }
        public bool TemAnotacaoGerente { get; set; }
        public int MesReferencia { get; set; }
        public bool NecessitaRecalculo { get; set; }
        public bool TemEscritorioPagamentoAtivo { get; set; }
        public bool FaturaInterna { get; set; }
        public bool FaturaFraca { get; set; }
        public bool TemInjecaoEnergiaPropria { get; set; }
        public int IdConta { get; set; }
        [Column(TypeName = "DECIMAL(18,3)")]
        public decimal? Rateio { get; set; }
        public string AnotacaoFatura { get; set; }
        public string UrlArquivo { get; set; }
        public string SubOrigem { get; set; }
        public bool SemUCNoRD { get; set; }
        public string Hash { get; set; }
        public int QuantidadeFaturas { get; set; } = 1;
        public bool Excluida { get; set; }
        public string Distribuidora { get; set; }
        public string DistribuidoraRD { get; set; }
        public string QtEnergiaCompensadaHfp { get; set; }
        public string VlEconomia { get; set; }
        public string VlItemRegra { get; set; }
        public string VlSemConsorcio { get; set; }
        public string VlTarEnergiaInjetadaHfp { get; set; }
        public string VlTarEnergiainjetadaHfpDescontoAplicado{ get; set; }
        public DateTime? Atualizacao { get; set; }
        public DateTime Criacao { get; set; }
    }
}
