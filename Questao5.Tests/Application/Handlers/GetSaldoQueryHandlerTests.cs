using Moq;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Questao5.Tests.Application.Handlers
{
    public class GetSaldoQueryHandlerTests
    {
        private readonly Mock<IQueryStore> _queryStoreMock;
        private readonly GetSaldoQueryHandler _handler;

        public GetSaldoQueryHandlerTests()
        {
            _queryStoreMock = new Mock<IQueryStore>();
            _handler = new GetSaldoQueryHandler(_queryStoreMock.Object);
        }

        [Fact]
        public async Task InvalidAccount_ReturnsError()
        {
            // Arrange
            var request = new GetSaldoQueryRequest(123);
            _queryStoreMock.Setup(q => q.GetContaCorrenteByNumeroAsync(It.IsAny<GetContaCorrenteByNumeroRequest>()))
                .ReturnsAsync((GetContaCorrenteByNumeroRequest _) => null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(400, response.StatusCode);
            Assert.Equal(ErrorReturnConstants.INVALID_ACCOUNT, response.ErrorMessage);
        }

        [Fact]
        public async Task InactiveAccount_ReturnsError()
        {
            // Arrange
            var request = new GetSaldoQueryRequest(123);
            _queryStoreMock.Setup(q => q.GetContaCorrenteByNumeroAsync(It.IsAny<GetContaCorrenteByNumeroRequest>()))
                           .ReturnsAsync(new GetContaCorrenteByNumeroResponse { Ativo = false });

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(400, response.StatusCode);
            Assert.Equal(ErrorReturnConstants.INACTIVE_ACCOUNT, response.ErrorMessage);
        }


        [Fact]
        public async Task ValidAndActiveAccount_ReturnsSuccess()
        {
            // Arrange
            var request = new GetSaldoQueryRequest(1234); 
            var contaCorrente = new GetContaCorrenteByNumeroResponse { Ativo = true, Nome = "Nome do Titular" };
            _queryStoreMock.Setup(q => q.GetContaCorrenteByNumeroAsync(It.IsAny<GetContaCorrenteByNumeroRequest>()))
                           .ReturnsAsync(contaCorrente);

            var movimentos = new List<GetMovimentosByIdContaCorrenteResponse>
            {
                new GetMovimentosByIdContaCorrenteResponse { TipoMovimento = ETipoMovimento.C.ToString(), Valor = 100 },
                new GetMovimentosByIdContaCorrenteResponse { TipoMovimento = ETipoMovimento.D.ToString(), Valor = 50 },
                new GetMovimentosByIdContaCorrenteResponse { TipoMovimento = ETipoMovimento.C.ToString(), Valor = 75 }
            };

            _queryStoreMock.Setup(q => q.GetMovimentosByIdContaCorrente(It.IsAny<GetMovimentosByIdContaCorrenteRequest>()))
                           .ReturnsAsync(movimentos);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(response.Data);
            Assert.Equal("Nome do Titular", response.Data.NomeTitular);
        }
    }
}

