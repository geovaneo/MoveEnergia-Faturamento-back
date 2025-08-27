using System.ComponentModel.DataAnnotations;

namespace MoveEnergia.Billing.Core.Enum
{
    public enum Mercado : int
    {
        [Display(Name = "N/A")]
        NA = 0,
        [Display(Name = "Residencial")]
        Residencial = 1,
        [Display(Name = "Comercial")]
        Comercial = 2,
        [Display(Name = "Industrial")]
        Industrial = 3,
        [Display(Name = "Rural")]
        Rural = 4,
    }
}
