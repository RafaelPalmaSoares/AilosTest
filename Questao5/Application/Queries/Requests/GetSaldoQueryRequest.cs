using MediatR;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.BaseEntity;

namespace Questao5.Application.Queries.Requests
{
   public record GetSaldoQueryRequest(int conta) : IRequest<BaseResponse<GetSaldoQueryResponse>>;

}
