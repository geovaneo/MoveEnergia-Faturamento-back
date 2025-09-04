using System.Security.Cryptography.X509Certificates;

namespace MoveEnergia.Billing.Core.Entity
{
    public class Deals
    {
        public Guid Id { get; set; }
        public string DealId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public decimal Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreationTime { get; set; }
        public Int32 CreatorUserID { get; set; }
        public DateTime LastModificationTime { get; set; }
        public Int32 LastModificationUserID { get; set; }
        public Byte IsDeleted { get; set; }
        public Int32 DeleterUserId { get; set; }
        public DateTime DeletionTime { get; set; }
        public string CNPJCPF { get; set; }
        public string Distribuidora {  get; set; }
        public string SubOrigem { get; set; }
        public string UC {  get; set; }
        public string Pipeline {  get; set; }
        public string Stage { get; set; }
        public string DealsWin { get; set; }
        public string FechadoEm { get; set; }
        public string CsAutoConsumo { get; set; }
        public string CoopConsumoMedioKWh {  get; set; }
        public string Titularidade {  get; set; }
        public string CEP {  get; set; }
        public string CoopModalidadeDesconto { get; set; }
        public string DescAdicionalPercent { get; set; }
        public string DescAmarela { get; set; }
        public string DescVerde { get; set; }
        public string DescVermelha1 { get; set; }
        public string DescVermelha2 { get;set; }



    }
}
