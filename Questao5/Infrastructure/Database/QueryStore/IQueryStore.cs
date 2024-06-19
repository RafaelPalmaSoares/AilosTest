using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public interface IQueryStore
    {
        Task<GetIdempotenciaByIdResponse> GetIdempotenciaByIdAsync(GetIdempotenciaByIdRequest request);
        Task<GetContaCorrenteByNumeroResponse> GetContaCorrenteByNumeroAsync(GetContaCorrenteByNumeroRequest request);
        Task<IEnumerable<GetMovimentosByIdContaCorrenteResponse>> GetMovimentosByIdContaCorrente(GetMovimentosByIdContaCorrenteRequest request);


    }
}
