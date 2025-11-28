namespace MoveEnergia.Billing.Core.Dto.General
{
    public class FaturaPdfLinha
    {

        //Unid - Qtd - Preço Unit c/trib - Valor(R$) - COFINS/PIS - Base Calc ICMS - Aliq ICMS - ICMS - Tarifa Unit
        //KWH 1.828,000 0,408884 747,44 31,89 747,44 17,00 127,06 0,321930

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

    }
}
