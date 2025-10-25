using Tyr.Domain.ClienteAggregate;

namespace Tyr.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<bool> ClienteExisteAsync(int clienteId);
        Task<Cliente?> ObterClientePorIdAsync(int clienteId);
    }
}
