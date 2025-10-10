# ⚔️ Projeto Tyr

*Uma API de agendamento forjada para trazer ordem e precisão, inspirada no deus nórdico da lei.*

Este projeto é o meu laboratório pessoal para estudo e aplicação de conceitos de desenvolvimento backend com .NET. O objetivo é construir, passo a passo, uma API robusta e bem arquitetada para um sistema de agendamento de barbearia, seguindo as melhores práticas do mercado.

---

## 📜 Funcionalidades (Missões da Jornada)

Este projeto está sendo construído em missões, adicionando novas funcionalidades e melhorando a arquitetura a cada passo.

#### Implementado ✅
-   **CRUD de Serviços:** Gerenciamento completo dos serviços oferecidos.
-   **CRUD de Profissionais:** Gerenciamento completo dos profissionais.
-   **Organização de Endpoints:** Separação de responsabilidades usando Métodos de Extensão.
-   **Relacionamentos 1-para-N:** Associação entre `Profissional` e `Service`.
-   **Uso de DTOs:** Implementação de Data Transfer Objects para evitar referências cíclicas e expor dados de forma controlada.

---

## ⚙️ Tecnologias Utilizadas

* **.NET 9**
* **ASP.NET Core (Minimal APIs)**
* **Entity Framework Core 9**
* **PostgreSQL** (com o provedor Npgsql)
* **Swagger/OpenAPI** para documentação de API

---

## 🚀 Como Executar o Projeto

Siga os passos abaixo para configurar e executar o ambiente de desenvolvimento local.

### Pré-requisitos
* [.NET 9 SDK](https://dotnet.microsoft.com/download)
* [PostgreSQL](https://www.postgresql.org/download/) (recomendo rodar via Docker)
* [Git](https://git-scm.com/)

### 1. Clonar o Repositório
```bash
git clone [https://github.com/Gustavo-Resende/Tyr.git](https://github.com/Gustavo-Resende/Tyr.git)
cd Tyr
