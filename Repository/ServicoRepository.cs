using Tyr.Data;
using Tyr.Interfaces;
using Tyr.Models;

namespace Tyr.Repository
{
    public class ServicoRepository : IServicoRepository
    {

        private readonly AppDbContext _context;
        public ServicoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Servico?> ObterServicoPorIdAsync(int servicoId)
        {
            var servico = await _context.Servicos.FindAsync(servicoId);
            if (servico is null)
            {
                return null;
            }

            return servico;
        }

        public async Task<bool> ServicoExisteAsync(int servicoId)
        {
            var servico = await _context.Servicos.FindAsync(servicoId);
            if (servico is null)
            {
                return false;
            }
            return true;
        }
    }
}
