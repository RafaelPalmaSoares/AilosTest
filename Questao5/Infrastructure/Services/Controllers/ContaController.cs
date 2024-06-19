using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Swashbuckle.AspNetCore.Annotations;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("v1/conta")]
    public class ContaController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ContaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpPost("movimentacao")]
        [SwaggerOperation(Summary = "Adiciona movimentações de crédito e débito para a conta corrente.")]
        [SwaggerResponse(200, "Movimentação adicionada com sucesso !", typeof(MovimentacaoCommandResponse))]
        [SwaggerResponse(400, "Conta não cadastrada.")]
        [SwaggerResponse(400, "Conta inativa.")]
        [SwaggerResponse(400, "O valor da movimentação deve ser maior que ZERO.")]
        [SwaggerResponse(400, "Tipo de conta inválido para essa transação.")]
        public async Task<IActionResult> Movimentacao([FromBody] MovimentacaoCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode(response.StatusCode, response);
        }


        [HttpGet("saldo/{conta}")]
        [SwaggerOperation(Summary = "Busca o saldo da conta corrente.")]
        [SwaggerResponse(200, "Saldo encontrado e calculado", typeof(MovimentacaoCommandResponse))]
        [SwaggerResponse(400, "Conta não cadastrada.")]
        [SwaggerResponse(400, "Conta inativa.")]
        public async Task<IActionResult> Saldo([FromRoute] GetSaldoQueryRequest contaCorrente)
        {
            var response = await _mediator.Send(contaCorrente);
            return StatusCode(response.StatusCode, response);
        }
    }
}
