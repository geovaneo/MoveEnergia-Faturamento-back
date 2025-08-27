namespace MoveEnergia.Billing.Core.Entity
{
    public class FaturaCache
    {
        public int Id { get; set; }
        public int FaturaId { get; set; }
        public bool EnviadoWhatsapp { get; set; }
        public string NumeroEnviado { get; set; }
        public string UrlBoleto { get; set; }
        public int Tentativa { get; set; }
        public DateTime DataEnviadoWhatsapp { get; set; }
        public int MesReferencia { get; set; }
        public string Status { get; set; }
        public string StatusEnviado { get; set; }
        public string SubOrigem { get; set; }
        public string Token { get; set; }
        public string Hash { get; set; }
    }
}
