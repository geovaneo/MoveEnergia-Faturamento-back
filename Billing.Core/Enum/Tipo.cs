using System.ComponentModel.DataAnnotations;

namespace MoveEnergia.Billing.Core.Enum
{
    public enum Tipo : byte
    {
        [Display(Name = "Consumidores varejistas")]
        ConsumidoresVarejistas = 0,
        [Display(Name = "Consumidores geração compartilhada")]
        ConsumidoresGeracaoCompartilhada = 1,
        [Display(Name = "Usinas para arrendamento")]
        UsinasParaArrendamento = 2
    }
}
