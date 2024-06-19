using System.Globalization;

namespace Questao1
{
    class ContaBancaria {

        public int Numero { get; init; } = 0;
        public string Titular { get; private set; } = default;
        public double Saldo{ get; private set; } = 0.00;

        public ContaBancaria(int numero, string titular, double saldo = 0.00)
        {
            Numero = numero;
            Titular = titular;
            Saldo = saldo;
        }

        public void Deposito(double quantia) =>
            this.Saldo += quantia;

        public void Saque(double quantia) =>
            this.Saldo -= (quantia + 3.50);

        public override string ToString() =>
            $"Conta: {this.Numero}, Titular: {this.Titular}, Saldo: $ {this.Saldo.ToString("F2", CultureInfo.InvariantCulture)}";

    }
}
