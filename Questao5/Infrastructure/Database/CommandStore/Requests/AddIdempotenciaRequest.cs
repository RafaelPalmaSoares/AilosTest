using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class AddIdempotenciaRequest
    {
        public Idempotencia Idempotencia { get; set; } = new Idempotencia();
    }
}
