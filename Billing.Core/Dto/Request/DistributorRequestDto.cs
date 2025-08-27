namespace MoveEnergia.Billing.Core.Dto.Request
{
    public class DistributorRequestDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NomeDeApresentacao { get; set; }
        public string Cnpj { get; set; }
        public string Sigla { get; set; }
        public string UF { get; set; }
        public bool IsActive { get; set; }
        public decimal ICMSTUSDc { get; set; }
        public decimal ICMSTE { get; set; }
        public decimal ICMSComum { get; set; }
        public decimal PISComum { get; set; }
        public decimal COFINSComum { get; set; }
        public decimal ICMSInjetada { get; set; }
        public decimal PISInjetada { get; set; }
        public decimal COFINSInjetada { get; set; }
        public DateTime DataReajustePrevisto { get; set; }
        public DateTime DataReajusteRealizado { get; set; }
        public int? IdCooperativa { get; set; }
    }
}
