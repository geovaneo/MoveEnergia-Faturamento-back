using System.ComponentModel.DataAnnotations.Schema;

namespace MoveEnergia.Billing.Core.Entity
{
    public class Distributor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NomeDeApresentacao { get; set; }
        public string Cnpj { get; set; }
        public string Sigla { get; set; }
        public string UF { get; set; }
        public bool IsActive { get; set; }
        
        [Column(TypeName = "DECIMAL(18,3)")]
        public decimal ICMSTUSDc { get; set; }
        [Column(TypeName = "DECIMAL(18,3)")]
        public decimal ICMSTE { get; set; }
        [Column(TypeName = "DECIMAL(18,3)")]
        public decimal ICMSComum { get; set; }
        [Column(TypeName = "DECIMAL(18,3)")]
        public decimal PISComum { get; set; }
        [Column(TypeName = "DECIMAL(18,3)")]
        public decimal COFINSComum { get; set; }
        [Column(TypeName = "DECIMAL(18,3)")]
        public decimal ICMSInjetada { get; set; }
        [Column(TypeName = "DECIMAL(18,3)")]
        public decimal PISInjetada { get; set; }
        [Column(TypeName = "DECIMAL(18,3)")]
        public decimal COFINSInjetada { get; set; }

        public DateTime DataReajustePrevisto { get; set; }
        public DateTime DataReajusteRealizado { get; set; }
        public int? IdCooperativa { get; set; }
    }
}
