namespace MoveEnergia.Billing.Core.Dto.General
{
    public class FaturaPdfData
    {

        public string? NomeDistribuidora { get; set; }
        public string? UC { get; set; }

        public string? MesRef { get; set; }
        public DateTime? Vcto { get; set; }
        public Decimal? Valor { get; set; }
        public DateTime? LeituraAnterior { get; set; }
        public DateTime? LeituraAtual { get; set; }
        public DateTime? DataEmissao { get; set; }
        public int EnergiaConsumida { get; set; }
        public int EnergiaCompensada { get; set; }
        public int EnergiaSaldo { get; set; }
        public string? CodBarras { get; set; }

        public string? ErrorMessage { get; set; }

        public List<FaturaPdfLinha>? Linhas { get; set; }

    }
}
