using System.Text.Json.Serialization;

namespace MoveEnergia.Billing.Core.Entity
{
    public class LeituraFaturaPdfProcesso
    {
        public int Id { get; set; } 
        public DateTime? Inicio { get; set; }
        public DateTime? Termino { get; set; }

    }
}
