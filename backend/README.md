# Controle de Materiais

Projeto full stack para controle de materiais, estoque e movimentações entre unidades.

O projeto utiliza dados fictícios e foi desenvolvido com foco em aprendizado prático, arquitetura de software, API REST, persistência e testes.

## Stack

### Backend

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- xUnit

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

## Arquitetura inicial

    Cliente
       ↓
    ASP.NET Core API
       ↓
    Application
       ↓
    Domain
       ↓
    Infrastructure
       ↓
    PostgreSQL

### Domain

Contém as entidades e as regras centrais do negócio.

### Application

Contém os casos de uso e coordena as operações da aplicação.

### Infrastructure

Responsável por persistência, banco de dados e integrações externas.

### API

Responsável por receber e responder requisições HTTP.

## Funcionalidades atuais

### Status da API

Endpoint:

    GET /api/status

Resposta:

    {
      "status": "online",
      "aplicacao": "Controle de Materiais"
    }

## Domínio planejado

O núcleo inicial do sistema terá:

- unidades;
- materiais;
- movimentações de estoque;
- entradas;
- saídas;
- consulta de saldo.

Fluxo básico:

    Unidade
       ↓
    Material
       ↓
    Movimentação
       ↓
    Entrada / Saída
       ↓
    Saldo

## Build

Dentro da pasta `backend`:

    dotnet build ControleMateriais.slnx

## Testes

    dotnet test ControleMateriais.slnx

## Executar a API

Dentro da pasta `backend`:

    dotnet run --project src/ControleMateriais.Api/ControleMateriais.Api.csproj --urls http://127.0.0.1:5070

Para testar o endpoint:

    curl http://127.0.0.1:5070/api/status

## Status do projeto

Primeira fase em desenvolvimento:

- estrutura inicial da solução criada;
- arquitetura em camadas definida;
- primeiro endpoint ASP.NET Core funcionando;
- testes xUnit configurados;
- integração com PostgreSQL será realizada nas próximas etapas.

## Autor

Bruno Ramos Lopes
