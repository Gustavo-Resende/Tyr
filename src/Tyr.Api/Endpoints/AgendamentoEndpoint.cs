using Microsoft.EntityFrameworkCore;
using Tyr.Api.Extensions;
using Tyr.Domain.AgendamentoAggregate;
using Tyr.Domain.AgendamentoAggregate.Specifications;
using Tyr.Domain.Interfaces;
using Tyr.DTOs;

namespace Tyr.Endpoints
{
    public static class AgendamentoEndpoint
    {
        public static void MapAgendamentoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/Agendamentos", async (IReadRepository<Agendamento> repository, CancellationToken cancellationToken) =>
            {
                var spec = new AgendamentosComDetalhesSpec();

                var agendamentos = await repository.ListAsync(spec, cancellationToken);

                return Results.Ok(agendamentos.ParseDTOList());

            });

            //app.MapPost("/Agendamentos", async (AgendamentoInputDto inputDto, IAgendamentoService repository, CancellationToken cancellationToken) =>
            //{
            //    var clienteExiste = await repository.VerificarExistenciaClienteAsync(inputDto.ClienteId);
            //    if (!clienteExiste)
            //    {
            //        return Results.NotFound();
            //    }

            //    var profissionalExiste = await repository.VerificarExistenciaProfissionalAsync(inputDto.ProfissionalId);
            //    if (!profissionalExiste)
            //    {
            //        return Results.NotFound();
            //    }

            //    var servicoExiste = await repository.VerificarExistenciaServicoAsync(inputDto.ServicoId);
            //    if (!servicoExiste)
            //    {
            //        return Results.NotFound();
            //    }

            //    var duracaoDoServico = TimeSpan.FromMinutes(30);
            //    var horarioInicio = inputDto.Horario.Value;
            //    var profissionalId = inputDto.ProfissionalId;

            //    var conflitoSpec = new AgendamentoComConflitoSpec(
            //        profissionalId, horarioInicio, duracaoDoServico);

            //    var conflitoEncontrado = await repository.

            //    DateTimeOffset novoHorarioFim = inputDto.Horario.Value.Add(duracaoDoServico);

            //    var conflitoEncontrado = await context.Agendamentos.AnyAsync(a =>
            //        a.ProfissionalId == inputDto.ProfissionalId &&
            //        a.Horario.HasValue && a.Duracao.HasValue &&
            //        inputDto.Horario < (a.Horario.Value + a.Duracao.Value) &&
            //        novoHorarioFim > a.Horario.Value
            //    );

            //    if (conflitoEncontrado)
            //    {
            //        return Results.Conflict("Horário não disponível para este profissional.");
            //    }

            //    var agendamento = new Agendamento
            //    {
            //        Horario = inputDto.Horario,
            //        ClienteId = inputDto.ClienteId,
            //        ProfissionalId = inputDto.ProfissionalId,
            //        ServicoId = inputDto.ServicoId,
            //    };

            //    context.Agendamentos.Add(agendamento);
            //    await context.SaveChangesAsync();

            //    var outputDto = new AgendamentoOutputDto(
            //        agendamento.Id,
            //        agendamento.Horario,
            //        agendamento.Status,
            //        clienteExiste.Nome,
            //        profissionalExiste.Nome,
            //        servicoExiste.Nome
            //    );

            //    return Results.Created($"/Agendamentos/{agendamento.Id}", outputDto);
            //});

            app.MapDelete("/Agendamentos/{id}", async (int id, IRepository<Agendamento> repository, CancellationToken cancellationToken) =>
            {
                var agendamento = await repository.GetByIdAsync(id, cancellationToken);
                if (agendamento == null)
                {
                    return Results.NotFound();
                }

                await repository.DeleteAsync(agendamento, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            });
        }
    }
}