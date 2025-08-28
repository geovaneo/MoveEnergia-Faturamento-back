using FluentValidation;
using MoveEnergia.Billing.Core.Dto.Request;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Validation
{
    public class DistributorValidation : AbstractValidator<Distributor>
    {
        public DistributorValidation()
        {
            RuleFor(x => x.ICMSTUSDc)
                .GreaterThan(0).WithMessage("ICMSTUSDc deve ser maior que zero");

            RuleFor(x => x.ICMSTE)
                .GreaterThan(0).WithMessage("ICMSTE deve ser maior que zero");

            RuleFor(x => x.ICMSComum)
                .GreaterThan(0).WithMessage("ICMS comum deve ser maior que zero");

            RuleFor(x => x.PISComum)
                .GreaterThan(0).WithMessage("PIS comum deve ser maior que zero");

            RuleFor(x => x.COFINSComum)
                .GreaterThan(0).WithMessage("COFINS comum deve ser maior que zero");

            RuleFor(x => x.ICMSInjetada)
                .GreaterThan(0).WithMessage("ICMS injetada deve ser maior que zero");

            RuleFor(x => x.PISInjetada)
                .GreaterThan(0).WithMessage("PIS injetada deve ser maior que zero");

            RuleFor(x => x.COFINSInjetada)
                .GreaterThan(0).WithMessage("COFINS injetada deve ser maior que zero");

            RuleFor(x => x.DataReajustePrevisto)
                .NotEmpty().WithMessage("Data de Reajuste Prevista é obrigatória");

            RuleFor(x => x.DataReajusteRealizado)
                .NotEmpty().WithMessage("Data de Reajuste Realizado é obrigatória");
        }
    }
}
