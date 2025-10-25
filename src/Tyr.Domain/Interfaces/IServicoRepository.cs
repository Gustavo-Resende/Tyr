using Tyr.Domain.Entidades;

namespace Tyr.Domain.Interfaces
{
    public interface IServicoRepository
    {
        Task<bool> ServicoExisteAsync(int servicoId);
        Task<Servico?> ObterServicoPorIdAsync(int servicoId);
    }
}
