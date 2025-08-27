namespace MoveEnergia.Billing.Core.Dto.Response
{
    public class TenantsReponseDto
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public Int32 CreatorUserId { get; set; }
        public DateTime LastModificationTime { get; set; }
        public Int32 LastModifierUserId { get; set; }
        public bool IsDeleted { get; set; }
        public Int32 DeleterUserId { get; set; }
        public DateTime DeletionTime { get; set; }
        public string TenancyName { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public bool IsActive { get; set; }
        public int EditionId { get; set; }
        public string RdStationSubOrigem { get; set; }
        public string RdStationToken { get; set; }
    }
}
