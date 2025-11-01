using Tyr.Domain.Entities;
using Tyr.Domain.ProfissionalAggregate;

namespace Tyr.Domain.ServicoAggregate
{
    public class Servico : EntityBase<int>, IAggregateRoot
    {
        public Servico(string? nome, decimal preco, int duracaoMinutos)
        {
            Nome = nome;
            Preco = preco;
            DuracaoMinutos = duracaoMinutos;
        }

        public string? Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int DuracaoMinutos { get; set; }

        public List<Profissional> Profissionais { get; set; } = new();
    }
}