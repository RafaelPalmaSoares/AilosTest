using Dapper;
using FluentAssertions.Equivalency;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class CommandStore : ICommandStore
    {
        private readonly DatabaseConfig _config;

        public CommandStore(DatabaseConfig config)
        {
            _config = config;
        }

        public async Task AddIdempotenciaAsync(AddIdempotenciaRequest request)
        {
            using (var connection = new SqliteConnection(_config.Name))
            {
                connection.Open();
                await connection.ExecuteAsync("INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@Id, @Requisicao, @Resultado)", 
                    new { Id = request.Idempotencia.Id, Requisicao = request.Idempotencia.Requisicao, Resultado = request.Idempotencia.Resultado});
            }
        }

        public async Task UpdateIdempotenciaAsync(string Id)
        {
            using (var connection = new SqliteConnection(_config.Name))
            {
                connection.Open();
                await connection.ExecuteAsync("UPDATE idempotencia SET resultado = 'Concluido' WHERE chave_idempotencia = @Id",
                    new { Id = Id});
            }
        }

        public async Task<AddMovimentoResponse> AddMovimentoAsync(AddMovimentoRequest request)
        {
            var response = new AddMovimentoResponse { IdMovimento = Guid.NewGuid()};
            using (var connection = new SqliteConnection(_config.Name))
            {
                connection.Open();
                await connection.ExecuteAsync("INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)" +
                                              "VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
                    new
                    {
                        IdMovimento = response.IdMovimento.ToString().ToUpper(),
                        IdContaCorrente = request.IdContaCorrente.ToString().ToUpper(),
                        DataMovimento = request.DataMovimento,
                        TipoMovimento = request.TipoMovimento.ToString(),
                        Valor = request.Valor
                    });

                return response;
            }
        }


    }
}
