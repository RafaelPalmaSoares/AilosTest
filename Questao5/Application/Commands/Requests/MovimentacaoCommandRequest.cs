using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.BaseEntity;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public sealed record MovimentacaoCommandRequest(Guid Id, int Conta, decimal Valor, string TipoMovimento) : IRequest<BaseResponse<MovimentacaoCommandResponse>>;
}
