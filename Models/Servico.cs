namespace Tyr.Models
{
    public class Servico
    {
        public int Id { get; set; }
        public string? Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }

        public int ProfissionalId { get; set; }
        public Profissional? Profissional { get; set; }
    }
}