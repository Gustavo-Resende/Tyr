# Tyr — API de Agendamento

API REST para gerenciamento de agendamentos (ex.: barbearia), construída com .NET 9 e organizada conforme princípios de Clean Architecture, DDD e CQRS. Este README descreve arquitetura, regras de negócio, contratos de API, instruções de execução local, uso com Docker, migrações de banco, testes e contribuições.

## Sumário

- Visão geral e arquitetura
- Regras de negócio essenciais
- Contratos (DTOs) e mensagens de erro
- Como executar localmente
- Migrações e banco de dados
- Execução com Docker
- Testes
- Documentação e endpoints (Swagger)
- Boas práticas para o front-end
- Contribuindo

---

## Arquitetura (resumida)

Projeto dividido em camadas:

- `Tyr.Domain` — entidades, agregados e contratos (interfaces) do domínio. Sem dependências externas.
- `Tyr.Application` — casos de uso (Commands/Queries), handlers (MediatR), DTOs e regras de orquestração.
- `Tyr.Infrastructure` — implementação técnica: `AppDbContext` (EF Core), repositórios (EfRepository), migrations.
- `Tyr.Api` — Minimal API que expõe endpoints HTTP, configura DI e Swagger.

Dependências fluem para dentro: `Api` -> `Application` -> `Domain`.

---

## Regras de negócio (principais)

- Agendamento (Appointment):
  - `Customer` e `Service` devem existir para criar um agendamento.
  - Deve existir um `BusinessHour` ativo (`IsActive = true`) para o `DayOfWeek` do `StartDateTime`; caso contrário retorna erro.
  - O horário inicial (`StartDateTime.TimeOfDay`) deve satisfazer `StartTime <= TimeOfDay < EndTime` do `BusinessHour`.
  - `EndDateTime` é calculado como `StartDateTime + service.DurationInMinutes`.
  - Verifica-se conflito por sobreposição de intervalos; se houver conflito retorna erro.

- BusinessHour:
  - `StartTime` deve ser anterior a `EndTime`.

- Customer:
  - `Name` e `Phone` são obrigatórios; evita duplicidade por `phone` ou `email`.

- Service:
  - `durationInMinutes` determina o tempo de atendimento; exclusão proibida se existirem agendamentos vinculados.

Mensagens de erro retornadas (úteis para a UI):

- `Customer not found.`
- `Service not found.`
- `Business is closed on that day.`
- `Start time outside business hours.`
- `Time slot is not available.`
- `Start must be before end.`
- `Name is required.`
- `Phone is required.`
- `Customer already exists.`
- `Service has appointments and cannot be deleted.`

---

## Contratos (DTOs) — exemplos JSON

- `AppointmentInputDto` (POST /appointments)

```json
{
  "startDateTime": "2026-01-10T09:00:00Z",
  "customerId": "00000000-0000-0000-0000-000000000000",
  "serviceId": "00000000-0000-0000-0000-000000000000",
  "notes": "Observação opcional"
}
```

- `AppointmentOutputDto` (resposta)

```json
{
  "id": "...",
  "startDateTime": "2026-01-10T09:00:00Z",
  "endDateTime": "2026-01-10T09:30:00Z",
  "status": "Pending",
  "customerName": "Fulano",
  "serviceName": "Corte",
  "notes": "..."
}
```

- `BusinessHourInputDto`

```json
{
  "dayOfWeek": "Monday",
  "startTime": "08:00:00",
  "endTime": "18:00:00",
  "isActive": true
}
```

---

## Como executar localmente

Pré-requisitos:

- .NET 9 SDK
- PostgreSQL (local ou via Docker)
- `dotnet-ef` (para aplicar migrations, opcional)

1) Clonar repositório

```bash
git clone https://github.com/Gustavo-Resende/Tyr.git
cd Tyr
```

2) Configurar connection string

Atualize `src/Tyr.Api/appsettings.Development.json` ou configure variáveis de ambiente com a chave `ConnectionStrings:DefaultConnection`. Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=tyr_db;Username=tyr_user;Password=tyr_pass"
  }
}
```

3) Aplicar migrations (opcional, recomendado)

Instale a ferramenta caso necessário:

```bash
dotnet tool install --global dotnet-ef
```

Aplicar migrations:

```bash
cd src/Tyr.Infrastructure
dotnet ef database update --project Tyr.Infrastructure.csproj --startup-project ../Tyr.Api/Tyr.Api.csproj
```

4) Executar a API

```bash
cd src/Tyr.Api
dotnet run
```

Por padrão a aplicação será exposta na porta definida pela configuração; em desenvolvimento o Swagger estará disponível.

---

## Testes

- Executar todos os testes unitários/integrados:

```bash
dotnet test
```

- Observação: alguns testes usam banco em memória; não deve conflitar com sua instância PostgreSQL local.

---

## Documentação e exploração da API

- Em ambiente de desenvolvimento, o Swagger UI está disponível em `/swagger` (ex.: `http://localhost:5000/swagger`).
- Verifique os contratos dos endpoints (`MapServiceEndpoints`, `MapCustomerEndpoints`, `MapAppointmentEndpoints`, `MapBusinessHourEndpoints`) para rotas e verbos precisos.

---

## Boas práticas para front-end

- Enviar `StartDateTime` em UTC (ISO 8601) ou adotar política clara de fuso horário.
- Ao selecionar um `Service`, usar `durationInMinutes` para bloquear intervalos disponíveis.
- Carregar `BusinessHour` do dia para habilitar apenas horários válidos.
- Exibir mensagens de erro retornadas pela API para facilitar diagnóstico e depuração.

---

## Observações de desenvolvimento / performance

- `GetAppointmentsByDateQuery` atualmente busca todos os agendamentos e filtra em memória; para ambientes com grande volume, transformar em query filtrada no banco.
- Conflitos de agendamento são verificados por especificação (`AppointmentConflictSpec`) — confirmar se é necessário suportar múltiplos recursos/profissionais.

---

## Contribuindo

- Workflow recomendado: fork -> feature branch -> pull request com descrição clara e testes.
- Siga convenções de código (EditorConfig) e adicione testes para novas regras de negócio.

---

## Suporte

- Abra issues no repositório para bugs ou solicitações de melhoria.

---

Documento gerado para uso por desenvolvedores e integradores. Ajuste exemplos de configuração conforme ambiente de produção.
