namespace Tyr.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int ProfessionalId { get; set; }
        public Professional? Professional { get; set; }
    }
}
