using System.Text.Json.Serialization;

namespace MoveEnergia.Billing.Core.Entity
{
    public class LeituraFaturaLinha
    {
        public int Id { get; set; } 
        public string? Descricao { get; set; }
        public string? Unidade { get; set; }
        public Decimal? Qtd { get; set; }
        public Decimal? PrecoUnit { get; set; }
        public Decimal? Valor { get; set; }
        public Decimal? CofinsPIS { get; set; }
        public Decimal? ICMSBaseCalc { get; set; }
        public Decimal? ICMSAliq { get; set; }
        public Decimal? ICMS { get; set; }
        public Decimal? TarifaUnit { get; set; }

        public int FaturaPDFId { get; set; }

        public virtual LeituraFaturaPdf FaturaPDF { get; set; }


    }
}
