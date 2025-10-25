namespace Tyr.Domain.Entidades
{
    public class Agendamento
    {
        public int Id { get; set; }
        public DateTimeOffset? Horario { get; set; }
        public TimeSpan? Duracao { get; set; } = TimeSpan.FromMinutes(30); // Duração de 30 minutos.
        public string? Status { get; set; } = "Agendado";

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int ProfissionalId { get; set; }
        public Profissional? Profissional { get; set; }
        
        public int ServicoId { get; set; }
        public Servico? Servico { get; set; }

    }
}