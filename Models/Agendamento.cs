namespace Tyr.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        public DateTime Horario { get; set; }
        public string? Status { get; set; } // Ex: "Agendado", "Concluído", "Cancelado"

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int ProfissionalId { get; set; }
        public Professional? Profissional { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
