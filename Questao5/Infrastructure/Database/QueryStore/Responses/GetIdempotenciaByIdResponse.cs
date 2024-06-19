using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class GetIdempotenciaByIdResponse : Idempotencia
    {
        public string Id { get; set; } = default!;
    }
}
