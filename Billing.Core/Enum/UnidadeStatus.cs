using System.ComponentModel.DataAnnotations;

namespace MoveEnergia.Billing.Core.Enum
{
    public enum UnidadeStatus : int
    {
        [Display(Name = "Entrada")]
        Entrada = 0,
        [Display(Name = "Negociação")]
        Negociacao = 1,
        [Display(Name = "Fechado")]
        Fechado = 2
    }
}
