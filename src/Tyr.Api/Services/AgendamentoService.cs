using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tyr.Domain.ClienteAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ProfissionalAggregate;
using Tyr.Domain.ServicoAggregate;

namespace Tyr.Domain.AgendamentoAggregate.Services
{
    public class AgendamentoService
    {
        private readonly IReadRepository<Cliente> _clienteRepo;
        private readonly IReadRepository<Profissional> _profissionalRepo;
        private readonly IRepository<Agendamento> _agendamentoRepo;
        private readonly IReadRepository<Servico> _servicoRepo;

        public AgendamentoService(
            IReadRepository<Cliente> clienteRepo,
            IReadRepository<Profissional> profissionalRepo,
            IRepository<Agendamento> agendamentoRepo,
            IReadRepository<Servico> servicoRepo)
        {
            _clienteRepo = clienteRepo;
            _profissionalRepo = profissionalRepo;
            _agendamentoRepo = agendamentoRepo;
            _servicoRepo = servicoRepo;
        }

        public async Task<bool> VerificarExistenciaClienteAsync(int clienteId)
        {
            var cliente = await _clienteRepo.GetByIdAsync(clienteId);
            return cliente != null;
        }

        public async Task<bool> VerificarExistenciaProfissionalAsync(int profissionalId)
        {
            var profissional = await _profissionalRepo.GetByIdAsync(profissionalId);
            return profissional != null;
        }

        public async Task<bool> VerificarExistenciaServicoAsync(int servicoId)
        {
            var servico = await _servicoRepo.GetByIdAsync(servicoId);
            return servico != null;
        }


    }
}
