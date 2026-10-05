# Controle de Materiais

Projeto full stack para controle de materiais, estoque e movimentações entre unidades.

O projeto utiliza dados fictícios e foi desenvolvido com foco em aprendizado prático, arquitetura de software, API REST, persistência, testes automatizados e boas práticas de desenvolvimento.

## Stack

### Backend

- C#
- .NET 10
- ASP.NET Core Web API
- xUnit

### Banco de dados

Planejado para a próxima etapa:

- PostgreSQL
- Entity Framework Core

### Frontend

Planejado para uma etapa posterior:

- React
- TypeScript
- Vite

## Estrutura do projeto

    controle-materiais-dotnet-react/
    ├── backend/
    │   ├── src/
    │   │   ├── ControleMateriais.Api/
    │   │   ├── ControleMateriais.Application/
    │   │   ├── ControleMateriais.Domain/
    │   │   └── ControleMateriais.Infrastructure/
    │   └── tests/
    │       └── ControleMateriais.Tests/
    └── frontend/

## Arquitetura

    Cliente
       ↓
    ASP.NET Core API
       ↓
    Application
       ↓
    Domain
       ↑
    Infrastructure
       ↓
    PostgreSQL

### Domain

Contém entidades e regras centrais do negócio.

Atualmente possui:

- Unidade;
- Material;
- MovimentacaoEstoque;
- UnidadeMedida;
- TipoMovimentacao;
- cálculo de saldo;
- validações de movimentação.

### Application

Coordena os casos de uso do sistema.

Atualmente possui:

- cadastro de unidades;
- cadastro de materiais;
- registro de entradas;
- registro de saídas;
- consulta de saldo;
- contratos de persistência;
- DTOs de entrada e saída.

### Infrastructure

Será responsável pela implementação dos contratos da Application e integração com PostgreSQL.

### API

Responsável pelos endpoints HTTP.

Atualmente possui um endpoint de status da aplicação.

## Regras de negócio implementadas

- unidade deve possuir nome;
- material deve possuir nome;
- unidade pode ser desativada;
- material pode ser desativado;
- movimentações exigem unidade e material;
- quantidade deve ser maior que zero;
- movimentação pode ser de entrada ou saída;
- entrada aumenta o saldo;
- saída reduz o saldo;
- saída maior que o estoque disponível é bloqueada;
- unidade inativa não pode movimentar estoque;
- material inativo não pode ser movimentado.

## Status da API

Endpoint:

    GET /api/status

Resposta:

    {
      "status": "online",
      "aplicacao": "Controle de Materiais"
    }

## Testes

O projeto utiliza xUnit.

Atualmente:

    36 testes automatizados
    36 aprovados
    0 falhas

Executar:

    cd backend
    dotnet test ControleMateriais.slnx

## Build

    cd backend
    dotnet build ControleMateriais.slnx

## Executar a API

    cd backend
    dotnet run --project src/ControleMateriais.Api/ControleMateriais.Api.csproj --urls http://127.0.0.1:5070

Testar:

    curl http://127.0.0.1:5070/api/status

## Status do projeto

### Bloco 1 — Ambiente e estrutura

Concluído.

### Bloco 2 — Domínio e regras de estoque

Concluído.

### Bloco 3 — Application e casos de uso

Concluído.

### Próxima etapa

Bloco 4 — PostgreSQL e Entity Framework Core.

Nesta etapa serão implementados:

- Entity Framework Core;
- driver PostgreSQL;
- DbContext;
- mapeamento das entidades;
- migrations;
- repositórios reais;
- persistência em PostgreSQL;
- testes com banco de dados.

## Autor

Bruno Ramos Lopes
