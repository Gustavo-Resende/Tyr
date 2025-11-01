using Tyr.Application.DTOs;
using Tyr.Domain.ClienteAggregate;

namespace Tyr.Api.Extensions
{
    public static class ClientExtension
    {
        public static ClienteOutputDto ParseDTO(this Cliente cliente)
            => new(cliente.Id, cliente.Nome, cliente.Telefone);

        public static IEnumerable<ClienteOutputDto> ParseDTOList(this IEnumerable<Cliente> clientes)
            => clientes.Select(cliente => cliente.ParseDTO());
    }
}
