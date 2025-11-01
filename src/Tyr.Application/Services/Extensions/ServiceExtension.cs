using Tyr.Application.Services.Dtos;
using Tyr.Domain.ServicoAggregate;

namespace Tyr.Application.Services.Extensions
{
    public static class ServiceExtension
    {
        public static ServiceDto ParseDTO(this Servico servico)
            => new(servico.Id, servico.Nome, servico.Preco, servico.DuracaoMinutos);

        public static List<ServiceDto> ParseDTOList(this List<Servico> servicos)
            => servicos.Select(s => s.ParseDTO()).ToList();
    }
}