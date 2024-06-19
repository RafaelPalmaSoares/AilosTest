using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class GetContaCorrenteByNumeroResponse: ContaCorrente
    {
        public string Id { get; set; } = default!;
    }
}
