using Tyr.Domain.ClienteAggregate;
using Tyr.Domain.ServicoAggregate;
using Tyr.DTOs;

namespace Tyr.Api.Extensions
{
    public static class ServiceExtension
    {
        public static ServicoDto ParseDTO(this Servico servico)
            => new(servico.Id, servico.Nome, servico.Preco);
    }
}
