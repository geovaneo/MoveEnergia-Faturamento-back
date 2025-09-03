namespace MoveEnergia.RdStation.Adapter.Dto.Response
{
    public class RdCustomerUserResponseDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? TenantId { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public  bool TwoFactorEnabled { get; set; }
        public  bool LockoutEnabled { get; set; }
        public  int AccessFailedCount { get; set; }
        public  string UserName { get; set; }
        public  string NormalizedUserName { get; set; }
        public  string NormalizedEmail { get; set; }
    }
}
