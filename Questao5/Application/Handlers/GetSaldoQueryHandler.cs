using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.BaseEntity;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Application.Handlers
{
    public class GetSaldoQueryHandler : IRequestHandler<GetSaldoQueryRequest, BaseResponse<GetSaldoQueryResponse>>
    {
        private readonly IQueryStore _queryStore;

        public GetSaldoQueryHandler(IQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public async Task<BaseResponse<GetSaldoQueryResponse>> Handle(GetSaldoQueryRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GetSaldoQueryResponse>();
            response.AddData(new GetSaldoQueryResponse());

            var conta = await _queryStore.GetContaCorrenteByNumeroAsync(new GetContaCorrenteByNumeroRequest { NumeroConta = request.conta });
            if (conta is null)
            {
                response.AddError(ErrorReturnConstants.INVALID_ACCOUNT);
                response.SetStatusCode(400);

                return response;
            }

            if (!conta.Ativo)
            {
                response.AddError(ErrorReturnConstants.INACTIVE_ACCOUNT);
                response.SetStatusCode(400);

                return response;
            }

            var movimentos = await _queryStore.GetMovimentosByIdContaCorrente(new GetMovimentosByIdContaCorrenteRequest { IdContaCorrente = conta.Id});

            var result = new GetSaldoQueryResponse
            {
                ContaCorrente = request.conta,
                NomeTitular = conta.Nome,
                DataConsulta = DateTime.UtcNow,
                SaldoTotal = BalanceCalculation(movimentos)
            };

            response.AddData(result);
            response.SetStatusCode(200);

            return response;
        }

        private decimal BalanceCalculation(IEnumerable<GetMovimentosByIdContaCorrenteResponse> movimentacao)
        {
            var debito = movimentacao
            .Where(m => m.TipoMovimento == ETipoMovimento.D.ToString())
            .Sum(m => m.Valor);

            var credito = movimentacao
            .Where(m => m.TipoMovimento == ETipoMovimento.C.ToString())
            .Sum(m => m.Valor);

            return (credito - debito);
        }
    }
}
