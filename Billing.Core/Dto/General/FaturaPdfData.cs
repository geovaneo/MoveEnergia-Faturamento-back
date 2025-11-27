namespace MoveEnergia.Billing.Core.Dto.General
{
    public class FaturaPdfData
    {
        public string UC { get; set; }

        public string MesRef { get; set; }
        public DateTime Vcto { get; set; }
        public Decimal Valor { get; set; }

        public string ErrorMessage { get; set; }

    }
}
