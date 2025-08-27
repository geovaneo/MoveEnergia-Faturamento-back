namespace MoveEnergia.Billing.Core.Entity
{
    public class ConsumerUnit
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string UC { get; set; }
        public string Cnpj { get; set; }
        public string Apelido { get; set; }
        public DateTime? ExpectativaOperacao { get; set; }
        public DateTime? DataMigracao { get; set; }
        public string NomeUsina { get; set; }
        public decimal? FatorCapacidade { get; set; }
        public string Fonte { get; set; }
       
        public byte? ConnectionId { get; set; }
        public string Enquadramento { get; set; }
        public decimal? PotenciakWCA { get; set; }
        public decimal? DemandaContradada { get; set; }
        public decimal? DemandaContradadaForaPonta { get; set; }
        public decimal? DemandaContradadaPonta { get; set; }
        public decimal? ValorGestao { get; set; }
        public byte UnidadeStatusId { get; set; }
        public byte StateId { get; set; }
        public byte SubgroupId { get; set; }
        public byte TariffModalityId { get; set; }
        public int DistributorId { get; set; }
        public int? CustomerId { get; set; }
        public long UserId { get; set; }
        public int TenantId { get; set; }        
        public byte Tipo { get; set; }
        public DateTime? DataAssinatura { get; set; }
        public string SenhaDist { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }

        //public override DateTime CreationTime { get; set; } = DateTimeExtension.Agora();


        public virtual Customer? Customer { get; set; }
        public virtual State? State { get; set; }
        public virtual Subgroup? Subgroup { get; set; }
        public virtual TariffModality? TariffModality { get; set; }
        public virtual Distributor? Distributor { get; set; }
        public virtual List<ConsumerUnitMeasurement>? ConsumerUnitMeasurements { get; set; }
        public virtual User? User { get; set; }

    }
}

