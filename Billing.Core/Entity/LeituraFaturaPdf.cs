using System.Text.Json.Serialization;

namespace MoveEnergia.Billing.Core.Entity
{
    public class LeituraFaturaPdf
    {
        public int Id { get; set; } 
        public string? UC { get; set; }
        public string? MesReferencia { get; set; }
        public DateTime Vencimento { get; set; }
        public Decimal Valor { get; set; }
      
    }
}
