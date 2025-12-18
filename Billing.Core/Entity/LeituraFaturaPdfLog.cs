using System.Text.Json.Serialization;

namespace MoveEnergia.Billing.Core.Entity
{
    public class LeituraFaturaPdfLog
    {
        public int Id { get; set; } 
        public string? NomeDistribuidora { get; set; }

        public string? FileName { get; set; }
        public string? FileMD5 { get; set; }
        public string? FolderName { get; set; }

        public DateTime DataHora { get; set; }

        public string? Mensagem { get; set; }

        public int Processo { get; set; }

        public virtual LeituraFaturaPdfProcesso ProcessoEntity { get; set; }
    }
}
