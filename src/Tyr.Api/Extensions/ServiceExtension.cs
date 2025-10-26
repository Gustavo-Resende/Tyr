using Tyr.Domain.ClienteAggregate;
using Tyr.Domain.ServicoAggregate;
using Tyr.DTOs;

namespace Tyr.Api.Extensions
{
    public static class ServiceExtension
    {
        public static ServicoOutputDto ParseDTO(this Servico servico)
            => new(servico.Id, servico.Nome, servico.Preco, servico.ProfissionalId);

        public static List<ServicoOutputDto> ParseDTOList(this List<Servico> servicos)
            => servicos.Select(s => s.ParseDTO()).ToList();
    }
}
