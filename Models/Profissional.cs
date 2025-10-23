namespace Tyr.Models
{
    public class Profissional
    {
        public int Id { get; set; }
        public string? Nome { get; set; } = string.Empty;

        public List<Servico> Servicos { get; set; } = new();
    }
}
