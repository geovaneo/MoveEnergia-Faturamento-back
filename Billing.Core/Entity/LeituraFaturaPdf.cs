using System.Text.Json.Serialization;

namespace MoveEnergia.Billing.Core.Entity
{
    public class LeituraFaturaPdf
    {
        public int Id { get; set; } 
        public string? UC { get; set; }
        public string? MesReferencia { get; set; }
        public DateTime? Vencimento { get; set; }
        public DateTime? DataEmissao { get; set; }
        public Decimal? Valor { get; set; }
        public string? NomeDistribuidora { get; set; }

        public DateTime? LeituraAnterior { get; set; }
        public DateTime? LeituraAtual { get; set; }
        public string? FileName { get; set; }
        public string? FileMD5 { get; set; }
        public string? FolderName { get; set; }

        public int? EnergiaConsumida { get; set; }
        public int? EnergiaCompensada { get; set; }
        public int? EnergiaSaldo { get; set; }

        public string? CodBarras { get; set; }

        public virtual List<LeituraFaturaLinha> Linhas { get; set; }

    }
}
