namespace Questao5.Application.Queries.Responses
{
    public class GetSaldoQueryResponse
    {
        public int ContaCorrente { get; set; }
        public string NomeTitular { get; set; } = default!;
        public DateTime DataConsulta { get; set; }
        public decimal SaldoTotal { get; set; } = 0.00m;
    }
}
