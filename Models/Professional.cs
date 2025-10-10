namespace Tyr.Models
{
    public class Professional
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Specialty { get; set; } = string.Empty;

        public List<Service> Services { get; set; } = new();
    }
}
