using Tyr.Data;
using Tyr.Interfaces;
using Tyr.Models;

namespace Tyr.Repository
{
    public class ClienteRepository : IClienteRepository
    {

        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ClienteExisteAsync(int clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente is null)
            {
                return false;
            }
            return true;
        }

        public async Task<Cliente?> ObterClientePorIdAsync(int clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente is null) {
                return null;
            }

            return cliente;
        }
    }
}
