using FluentValidation;
using MoveEnergia.Billing.Core.Dto;

public static class Validation
{
    public static async Task<List<ReturnResponseErrorDto>?> ValidateAsync<T>(T entity, IValidator<T> validator)
    {
        var validation = await validator.ValidateAsync(entity);

        if (!validation.IsValid)
        {
            var erros = validation.Errors
                        .Select(e => new ReturnResponseErrorDto()
                        {
                            ErrorCode = 400,
                            ErrorMessage = e.ErrorMessage,
                            ErrorMessageDetail = $"{e.PropertyName}: {e.ErrorMessage}"

                        }).ToList();

            return erros;
        }

        return null; 
    }
}