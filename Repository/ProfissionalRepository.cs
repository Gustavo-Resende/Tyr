using Tyr.Data;
using Tyr.Interfaces;
using Tyr.Models;

namespace Tyr.Repository
{
    public class ProfissionalRepository : IProfissionalRepository
    {

        private readonly AppDbContext _context;
        public ProfissionalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ProfissionalExisteAsync(int profissionalId)
        {
            var profissional = await _context.Profissionais.FindAsync(profissionalId);
            if (profissional is null)
            {
                return false;
            }

            return true;
        }

        public async Task<Profissional?> ObterProfissionalPorIdAsync(int profissionalId)
        {
            var profissional = await _context.Profissionais.FindAsync(profissionalId);
            if (profissional is null)
            {
                return null;
            }

            return profissional;
        }

    }
}
