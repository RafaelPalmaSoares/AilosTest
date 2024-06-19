namespace Questao5.Domain.Entities
{
    public class ContaCorrente
    {
        public Guid Id  { get; init; }
        public int Numero { get; set; } = 0;
        public string Nome { get; set; } = default!;
        public bool Ativo { get; set; } = true;
    }
}
