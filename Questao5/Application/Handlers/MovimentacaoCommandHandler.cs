using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.BaseEntity;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoCommandHandler : IRequestHandler<MovimentacaoCommandRequest, BaseResponse<MovimentacaoCommandResponse>>
    {
        private readonly ICommandStore _commandStore;
        private readonly IQueryStore _queryStore;

        public MovimentacaoCommandHandler(ICommandStore commandStore, IQueryStore queryStore)
        {
            _commandStore = commandStore;
            _queryStore = queryStore;
        }

        public async Task<BaseResponse<MovimentacaoCommandResponse>> Handle(MovimentacaoCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<MovimentacaoCommandResponse>();
            response.AddData(new MovimentacaoCommandResponse());

            var idempotencia = await _queryStore.GetIdempotenciaByIdAsync(new GetIdempotenciaByIdRequest { Id = request.Id });
            if (idempotencia is null)
            {
                var addidempotencia = new AddIdempotenciaRequest();
                addidempotencia.Idempotencia.Id = request.Id;
                addidempotencia.Idempotencia.Requisicao = JsonConvert.SerializeObject(request);
                addidempotencia.Idempotencia.Resultado = EStatusRequisicao.Pendente.ToString();

                await _commandStore.AddIdempotenciaAsync(addidempotencia);
            }
            else if (idempotencia.Resultado.Equals(EStatusRequisicao.Concluido.ToString()))
            {
                response.AddData(new MovimentacaoCommandResponse { IdMovimento = request.Id });
                response.SetStatusCode(200);

                return response;
            }

            var conta = await _queryStore.GetContaCorrenteByNumeroAsync(new GetContaCorrenteByNumeroRequest { NumeroConta = request.Conta });
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

            if (request.Valor < 0)
            {
                response.AddError(ErrorReturnConstants.INVALID_VALUE);
                response.SetStatusCode(400);

                return response;
            }

            if (!IsValidETipoMovimento<ETipoMovimento>(request.TipoMovimento))
            {
                response.AddError(ErrorReturnConstants.INVALID_TYPE);
                response.SetStatusCode(400);

                return response;
            }

            var addMovimentoRequest = new AddMovimentoRequest
            {
                IdContaCorrente = Guid.Parse(conta.Id),
                DataMovimento = DateTime.Now,
                TipoMovimento = (ETipoMovimento)Enum.Parse(typeof(ETipoMovimento), request.TipoMovimento),
                Valor = request.Valor,
            };

            var result =  await _commandStore.AddMovimentoAsync(addMovimentoRequest);

            await _commandStore.UpdateIdempotenciaAsync(request.Id.ToString().ToUpper());

            response.AddData(new MovimentacaoCommandResponse { IdMovimento = result.IdMovimento});
            response.SetStatusCode(200);
            return response;
        }

        private static bool IsValidETipoMovimento<TEnum>(string value) where TEnum : struct, Enum
        {
            return Enum.TryParse(value, out TEnum _);
        }
    }
}
