using Microsoft.EntityFrameworkCore;
using Tyr.Data;
using Tyr.DTOs;
using Tyr.Models;

namespace Tyr.Endpoints
{
    public static class AgendamentoEndpoint
    {
        public static void MapAgendamentoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/Agendamentos", async (AppDbContext context) =>
            {
                var agendamentos = await context.Agendamentos
                    .Include(a => a.Cliente)
                    .Include(a => a.Profissional)
                    .Include(a => a.Servico)
                    .Select(a => new AgendamentoOutputDto(a.Id, a.Horario, a.Status, a.ClienteId, a.ProfissionalId, a.ServicoId))
                    .ToListAsync();

                return Results.Ok(agendamentos);
            });

            app.MapPost("/Agendamentos", async (AgendamentoInputDto inputDto, AppDbContext context) =>
            {
                var verificarCliente = await context.Clientes.AnyAsync(c => c.Id == inputDto.ClienteId);
                if (!verificarCliente)
                {
                    return Results.BadRequest($"ClienteId {inputDto.ClienteId} não encontrado.");
                }

                var verificarProfissional = await context.Profissionais.AnyAsync(p => p.Id == inputDto.ProfissionalId);
                if (!verificarProfissional)
                {
                    return Results.BadRequest($"ProfissionalId {inputDto.ProfissionalId} não encontrado.");
                }

                var verificarServico = await context.Servicos.AnyAsync(s => s.Id == inputDto.ServicoId);
                if (!verificarServico)
                {
                    return Results.BadRequest($"ServicoId {inputDto.ServicoId} não encontrado.");
                }

                TimeSpan duracaoDoServico = TimeSpan.FromMinutes(30);
                DateTimeOffset novoHorarioFim = inputDto.Horario.Value.Add(duracaoDoServico);

                var conflitoEncontrado = await context.Agendamentos.AnyAsync(a => a.ProfissionalId == inputDto.ProfissionalId && a.Horario.HasValue && a.Duracao.HasValue && inputDto.Horario < (a.Horario.Value + a.Duracao.Value) && novoHorarioFim > a.Horario.Value);

                if (conflitoEncontrado)
                {
                    return Results.Conflict("Horário não disponível para este profissional. O agendamento solicitado se sobrepõe a um agendamento existente.");
                }

                var agendamento = new Agendamento
                {
                    Horario = inputDto.Horario,
                    Duracao = duracaoDoServico,
                    ClienteId = inputDto.ClienteId,
                    ProfissionalId = inputDto.ProfissionalId,
                    ServicoId = inputDto.ServicoId,
                };

                context.Agendamentos.Add(agendamento);
                await context.SaveChangesAsync();


                var outputDto = new AgendamentoOutputDto(agendamento.Id, agendamento.Horario, agendamento.Status, agendamento.ClienteId, agendamento.ProfissionalId, agendamento.ServicoId);

                return Results.Created($"/Agendamentos/{agendamento.Id}", outputDto);
            });

            app.MapDelete("/Agendamentos/{id}", async (int id, AppDbContext context) =>
            {
                var agendamento = await context.Agendamentos.FirstOrDefaultAsync(x => x.Id == id);

                if (agendamento == null)
                {
                    return Results.NotFound();
                }

                context.Agendamentos.Remove(agendamento);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}