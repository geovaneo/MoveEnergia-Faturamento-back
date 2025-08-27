using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Response;
using MoveEnergia.Billing.Core.Interface.Adapter;

namespace MoveEnergia.Billing.Adapter
{
    public class AdapterBase : IAdapterBase
    {
        public async Task<ReturnResponseDto> SetReturnResponse<T>(object resultData)
        {
            var response = new ReturnResponseDto();

            if (resultData is List<T> listObject)
            {
                if (listObject != null && listObject.Count > 0 )
                {
                    response.Error = false;
                    response.StatusCode = 200;
                    response.Data = listObject;
                }
                else
                {
                    response.Error = false;
                    response.StatusCode = 404;
                    response.Data = null;
                }
            }
            else if (resultData is T itemObject)
            {
                if (itemObject != null)
                {
                    response.Error = false;
                    response.StatusCode = 200;
                    response.Data = itemObject;
                }
                else
                {
                    response.Error = false;
                    response.StatusCode = 404;
                    response.Data = null;
                }
            }
            else
            {
                response.Error = true;
                response.StatusCode = 500;
                response.Data = null;
                response.Erros?.Add(new ReturnResponseErrorDto()
                {
                    ErrorCode = 500,
                    ErrorMessage = "Tipo não tratado entre em ontato com o suporte"
                });
            }
            return response;
        }
        public async Task<ReturnResponseDto> SetReturnResponseException(Exception exception)
        {

            List<ReturnResponseErrorDto> errorList = new List<ReturnResponseErrorDto>();
            errorList.Add(new ReturnResponseErrorDto()
            {
                ErrorCode = 500,
                ErrorMessage = exception.Message,  
                Source = exception.Source
            });

            return new ReturnResponseDto()
            {
                Error = true,
                StatusCode = 500,
                Data = null,
                Erros = errorList
            };
        }

        public async Task<ReturnResponseDto> SetReturnResponseErros(List<string> erros)
        {
            List<ReturnResponseErrorDto> errorList = new List<ReturnResponseErrorDto>();

            if (erros != null && erros.Count > 0)
            {
                foreach (var err in erros)
                {
                    errorList.Add(new ReturnResponseErrorDto()
                    {
                        ErrorCode = 500,
                        ErrorMessage = err
                    });
                }
            }

            return new ReturnResponseDto()
            {
                Error = true,
                StatusCode = 400,
                Data = null,
                Erros = errorList                
            };
        }
    }
}
