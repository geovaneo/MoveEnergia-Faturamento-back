using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoveEnergia.Billing.Core.Enum
{
    public enum Titularidade : int
    {
        [Description("")]
        NA = 0,

        [Description("Consumidor")]
        Consumidor = 1,

        [Description("Cooperativa")]
        Cooperativa = 2
    }
}