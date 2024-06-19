using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public Guid Id { get; set; }
        public Guid IdContaCorrente { get; set; }
        public DateTime DataMovimento { get; set; }
        public ETipoMovimento TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}
