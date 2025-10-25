using Microsoft.EntityFrameworkCore;
using Tyr.Data;
using Tyr.DTOs;
using Tyr.Interfaces;
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
                    .Select(a => new AgendamentoOutputDto(
                        a.Id,
                        a.Horario,
                        a.Status,
                        a.Cliente != null ? a.Cliente.Nome : null,
                        a.Profissional != null ? a.Profissional.Nome : null,
                        a.Servico != null ? a.Servico.Nome : null
                    ))
                    .ToListAsync();

                return Results.Ok(agendamentos);
            });

            app.MapPost("/Agendamentos", async (AgendamentoInputDto inputDto, IClienteRepository clienteRepo, IProfissionalRepository profissionalRepo, IServicoRepository servicoRepo ,AppDbContext context) =>
            {
                // Aprender sobre configurar o program.cs para usar injeçao de dependencia (scoped, transient, singleton)
                // Aprender sobre Injeçao de Dependencia para melhorar esse codigo
                // Como funciona o [fromServices] do .net
                // Separar a logica de validacao em outra classe

                var clienteExiste = await clienteRepo.ObterClientePorIdAsync(inputDto.ClienteId);
                if (clienteExiste is null)
                {
                    return Results.BadRequest($"ClienteId {inputDto.ClienteId} não encontrado.");
                }

                var profissionalExiste = await profissionalRepo.ObterProfissionalPorIdAsync(inputDto.ProfissionalId);
                if (profissionalExiste is null)
                {
                    return Results.BadRequest($"ProfissionalId {inputDto.ProfissionalId} não encontrado.");
                }

                var servicoExiste = await servicoRepo.ObterServicoPorIdAsync(inputDto.ServicoId);
                if (servicoExiste is null)
                {
                    return Results.BadRequest($"ServicoId {inputDto.ServicoId} não encontrado.");
                }

                var duracaoDoServico = TimeSpan.FromMinutes(30);
                DateTimeOffset novoHorarioFim = inputDto.Horario.Value.Add(duracaoDoServico);

                var conflitoEncontrado = await context.Agendamentos.AnyAsync(a =>
                    a.ProfissionalId == inputDto.ProfissionalId &&
                    a.Horario.HasValue && a.Duracao.HasValue &&
                    inputDto.Horario < (a.Horario.Value + a.Duracao.Value) &&
                    novoHorarioFim > a.Horario.Value
                );

                if (conflitoEncontrado)
                {
                    return Results.Conflict("Horário não disponível para este profissional.");
                }

                var agendamento = new Agendamento
                {
                    Horario = inputDto.Horario,
                    ClienteId = inputDto.ClienteId,
                    ProfissionalId = inputDto.ProfissionalId,
                    ServicoId = inputDto.ServicoId,
                };

                context.Agendamentos.Add(agendamento);
                await context.SaveChangesAsync();

                var outputDto = new AgendamentoOutputDto(
                    agendamento.Id,
                    agendamento.Horario,
                    agendamento.Status,
                    clienteExiste.Nome,
                    profissionalExiste.Nome,
                    servicoExiste.Nome
                );

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