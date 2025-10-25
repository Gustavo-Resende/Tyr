using Tyr.Domain.Entidades;

namespace Tyr.Domain.Interfaces
{
    public interface IProfissionalRepository
    {
        Task<bool> ProfissionalExisteAsync(int profissionalId);
        Task<Profissional?> ObterProfissionalPorIdAsync(int profissionalId);
    }
}
