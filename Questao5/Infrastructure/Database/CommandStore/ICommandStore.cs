using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public interface ICommandStore
    {
        Task AddIdempotenciaAsync(AddIdempotenciaRequest request);
        Task UpdateIdempotenciaAsync(string Id);
        Task<AddMovimentoResponse> AddMovimentoAsync(AddMovimentoRequest request);
    }
}
