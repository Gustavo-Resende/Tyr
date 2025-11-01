# Projeto Tyr - API de Agendamento

Uma API REST para um sistema de agendamento de barbearia, desenvolvida em .NET 9. Este projeto serve como um laboratório prático para estudo, design e implementação de padrões de arquitetura de software avançados, incluindo **Clean Architecture**, **Domain-Driven Design (DDD)** e **CQRS**.

---

## 🏛️ Visão Geral da Arquitetura

Este projeto **não é uma Minimal API monolítica**. Ele é estruturado intencionalmente seguindo os princípios da **Clean Architecture** para garantir uma separação clara de responsabilidades (SoC), alta testabilidade e manutenibilidade a longo prazo.

As dependências fluem estritamente para o centro (`Api` -> `Application` -> `Domain`), garantindo que a lógica de negócio (`Domain`) seja pura e independente de detalhes de infraestrutura ou frameworks.

A solução é dividida nas seguintes camadas (projetos):

### 1. `Tyr.Domain` (O Coração do Negócio)
* **Propósito:** Contém a lógica de negócio pura e as regras do domínio.
* **Características:** É a camada mais interna e "limpa". Não possui dependências de nenhum outro projeto da solução nem de frameworks de infraestrutura (como Entity Framework Core).
* **Conteúdo Principal:**
    * **Entidades** e **Agregados** (Ex: `Agendamento`, `Profissional`, `Cliente`).
    * **Interfaces de Repositório** (`IRepository<T>`, `IReadRepository<T>`), que definem os *contratos* de persistência.
    * **(Futuro)** Objetos de Valor (Value Objects) e Eventos de Domínio (Domain Events).

### 2. `Tyr.Application` (O Orquestrador)
* **Propósito:** Contém a lógica de orquestração dos casos de uso da aplicação. Atua como o "cérebro" que coordena o `Domain` e a `Infrastructure`.
* **Características:** Implementa o padrão **CQRS (Command Query Responsibility Segregation)** usando a biblioteca **MediatR**.
* **Conteúdo Principal:**
    * **Comandos (Commands):** Objetos que representam uma intenção de *mudar* o estado do sistema (Ex: `CreateAgendamentoCommand`).
    * **Consultas (Queries):** Objetos que representam uma intenção de *ler* dados do sistema (Ex: `GetClienteByIdQuery`).
    * **Handlers:** As classes que processam os Comandos e Queries, contendo a lógica de aplicação.
    * **DTOs (Data Transfer Objects):** Objetos que definem o contrato de dados com a camada de API (Ex: `AgendamentoOutputDto`).
    * **Mapeamento:** Extensões ou perfis de AutoMapper/Mapster para converter Entidades em DTOs.

### 3. `Tyr.Infrastructure` (A Forja Técnica)
* **Propósito:** Implementa os "detalhes" técnicos e interage com o mundo exterior (banco de dados, APIs externas, etc.).
* **Características:** Implementa os contratos (interfaces) definidos no `Domain`.
* **Conteúdo Principal:**
    * **`AppDbContext`:** O DbContext do Entity Framework Core.
    * **`Migrations`:** Scripts de migração do banco de dados.
    * **Repositórios (`Repositories`):** Implementações concretas das interfaces do `Domain` (Ex: `ClienteRepository`). Utiliza **Ardalis.Specification** para criar consultas complexas, limpas e reutilizáveis.

### 4. `Tyr.Api` (O Portão de Entrada)
* **Propósito:** A camada de apresentação, responsável por expor a aplicação via HTTP.
* **Características:** É uma camada "magra" (thin). Sua única responsabilidade é receber requisições HTTP, enviar Comandos ou Queries para o MediatR e retornar as respostas.
* **Conteúdo Principal:**
    * **Endpoints (Minimal APIs):** Pontos de entrada da API.
    * **`Program.cs`:** Ponto de entrada da aplicação, onde ocorre a **Injeção de Dependência** (DI), configurando `DbContext`, Repositórios, MediatR, etc.
    * **Swagger/OpenAPI:** Configuração da documentação da API.

---

## 🛠️ Stack de Tecnologias

* **Framework:** .NET 9 (com ASP.NET Core para Minimal APIs)
* **Banco de Dados:** PostgreSQL
* **ORM:** Entity Framework Core 9
* **Padrões de Arquitetura:** Clean Architecture, Domain-Driven Design (DDD), CQRS
* **Bibliotecas-Chave:**
    * **MediatR:** Para implementação do padrão Mediator e CQRS.
    * **Ardalis.Specification:** Para encapsular a lógica de consulta (queries) de forma limpa.
    * **Ardalis.Result:** (Planejado) Para padronizar os retornos da camada de Aplicação.
* **Ferramentas:** Git, GitHub, Swagger (OpenAPI)

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
* [.NET 9 SDK](https://dotnet.microsoft.com/download)
* [PostgreSQL](https://www.postgresql.org/download/) (servidor local ou via Docker)
* Um cliente de API (como Postman, Insomnia, ou o arquivo `.http` do projeto)

### 1. Clonar o Repositório
```bash
git clone [https://github.com/Gustavo-Resende/Tyr.git](https://github.com/Gustavo-Resende/Tyr.git)
cd Tyr
