using System.ComponentModel.DataAnnotations;

namespace MoveEnergia.Billing.Core.Enum
{
    public enum TipoCustomer : int
    {
        [Display(Name = "Pessoa Física (CPF)")]
        Fisica = 0,
        [Display(Name = "Pessoa Jurídica (CNPJ)")]
        Juridica = 1
    }
}
