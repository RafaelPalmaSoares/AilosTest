using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Xunit;

namespace Questao5.Tests.Application.Handlers
{
    public class MovimentacaoCommandHandlerTests
    {
        private readonly Mock<ICommandStore> _commandStoreMock;
        private readonly Mock<IQueryStore> _queryStoreMock;
        private readonly MovimentacaoCommandHandler _handler;

        public MovimentacaoCommandHandlerTests()
        {
            _commandStoreMock = new Mock<ICommandStore>();
            _queryStoreMock = new Mock<IQueryStore>();
            _handler = new MovimentacaoCommandHandler(_commandStoreMock.Object, _queryStoreMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new MovimentacaoCommandRequest(Guid.NewGuid(), 123, 100, "C");

            _queryStoreMock.Setup(q => q.GetIdempotenciaByIdAsync(It.IsAny<GetIdempotenciaByIdRequest>()))
                           .ReturnsAsync((GetIdempotenciaByIdRequest _) => null);
            _queryStoreMock.Setup(q => q.GetContaCorrenteByNumeroAsync(It.IsAny<GetContaCorrenteByNumeroRequest>()))
                           .ReturnsAsync(new GetContaCorrenteByNumeroResponse { Ativo = true, Numero = 1234, Id = Guid.NewGuid().ToString() });
            _commandStoreMock.Setup(q => q.AddIdempotenciaAsync(It.IsAny<AddIdempotenciaRequest>()))
                           .Returns(Task.CompletedTask);
            _commandStoreMock.Setup(q => q.AddMovimentoAsync(It.IsAny<AddMovimentoRequest>()))
                           .ReturnsAsync(new AddMovimentoResponse { IdMovimento = Guid.NewGuid() });
            _commandStoreMock.Setup(q => q.UpdateIdempotenciaAsync(It.IsAny<string>()))
                           .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(response.Data);
        }

        [Fact]
        public async Task Handle_InvalidAccount_ReturnsError()
        {
            // Arrange
            var request = new MovimentacaoCommandRequest(Guid.NewGuid(), 123, 100, "C");

            _queryStoreMock.Setup(q => q.GetContaCorrenteByNumeroAsync(It.IsAny<GetContaCorrenteByNumeroRequest>()))
                           .ReturnsAsync((GetContaCorrenteByNumeroRequest _) => null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(400, response.StatusCode);
            Assert.Contains(ErrorReturnConstants.INVALID_ACCOUNT, response.ErrorMessage);
        }

        [Fact]
        public async Task Handle_InvalidValue_ReturnsError()
        {
            // Arrange
            var request = new MovimentacaoCommandRequest(Guid.NewGuid(), 123, -100, "C");

            _queryStoreMock.Setup(q => q.GetContaCorrenteByNumeroAsync(It.IsAny<GetContaCorrenteByNumeroRequest>()))
               .ReturnsAsync(new GetContaCorrenteByNumeroResponse { Ativo = true });

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(400, response.StatusCode);
            Assert.Contains(ErrorReturnConstants.INVALID_VALUE, response.ErrorMessage);
        }

        [Fact]
        public async Task Handle_InativeAccount_ReturnsError()
        {
            // Arrange
            var request = new MovimentacaoCommandRequest(Guid.NewGuid(), 123, 100, "C");

            _queryStoreMock.Setup(q => q.GetContaCorrenteByNumeroAsync(It.IsAny<GetContaCorrenteByNumeroRequest>()))
               .ReturnsAsync(new GetContaCorrenteByNumeroResponse { Ativo = false });

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(400, response.StatusCode);
            Assert.Contains(ErrorReturnConstants.INACTIVE_ACCOUNT, response.ErrorMessage);
        }

        [Fact]
        public async Task Handle_InvalidMoviment_ReturnsError()
        {
            // Arrange
            var request = new MovimentacaoCommandRequest(Guid.NewGuid(), 123, 100, "X");

            _queryStoreMock.Setup(q => q.GetContaCorrenteByNumeroAsync(It.IsAny<GetContaCorrenteByNumeroRequest>()))
               .ReturnsAsync(new GetContaCorrenteByNumeroResponse { Ativo = true });

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(400, response.StatusCode);
            Assert.Contains(ErrorReturnConstants.INVALID_TYPE, response.ErrorMessage);
        }
    }
}
