using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public class QueryStore : IQueryStore
    {
        private readonly DatabaseConfig _config;

        public QueryStore(DatabaseConfig config)
        {
            _config = config;
        }

        public async Task<GetIdempotenciaByIdResponse> GetIdempotenciaByIdAsync(GetIdempotenciaByIdRequest request)
        {
            using (var connection = new SqliteConnection(_config.Name))
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<GetIdempotenciaByIdResponse>(
                    "SELECT chave_idempotencia as Id, requisicao, resultado FROM idempotencia WHERE chave_idempotencia = @Id",
                    new {Id = request.Id});

                return result;
            }
        }

        public async Task<GetContaCorrenteByNumeroResponse> GetContaCorrenteByNumeroAsync(GetContaCorrenteByNumeroRequest request)
        {
            using (var connection = new SqliteConnection(_config.Name))
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<GetContaCorrenteByNumeroResponse>(
                    "SELECT idcontacorrente as Id, numero, nome, ativo FROM contacorrente WHERE numero = @Numero",
                    new { Numero = request.NumeroConta });

                return result;
            }
        }

        public async Task<IEnumerable<GetMovimentosByIdContaCorrenteResponse>> GetMovimentosByIdContaCorrente(GetMovimentosByIdContaCorrenteRequest request)
        {
            using (var connection = new SqliteConnection(_config.Name))
            {
                connection.Open();
                var result = await connection.QueryAsync<GetMovimentosByIdContaCorrenteResponse>(
                    "SELECT tipomovimento, valor FROM movimento WHERE idcontacorrente = @IdContaCorrente",
                    new { IdContaCorrente = request.IdContaCorrente.ToString().ToUpper() });

                return result.ToList();
            }
        }
    }
}
