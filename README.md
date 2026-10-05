# Controle de Materiais

Projeto full stack para controle de materiais, estoque e movimentações entre unidades.

O projeto utiliza dados fictícios e foi desenvolvido com foco em aprendizado prático, arquitetura de software, API REST, persistência em banco de dados, testes automatizados e boas práticas de desenvolvimento.

## Stack

### Backend

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- Npgsql
- xUnit

### Banco de dados

- PostgreSQL
- Entity Framework Core
- migrations

### Frontend

Planejado para etapa posterior:

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
    Entity Framework Core
       ↓
    PostgreSQL

A solução foi dividida em camadas para manter as regras de negócio independentes dos detalhes de infraestrutura.

### Domain

Contém as entidades e regras centrais do negócio.

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

A camada Application depende de abstrações de repositório, sem conhecer diretamente Entity Framework Core ou PostgreSQL.

### Infrastructure

Implementa a persistência da aplicação.

Atualmente possui:

- ControleMateriaisDbContext;
- configurações das entidades com Fluent API;
- integração com Entity Framework Core;
- integração com PostgreSQL por meio do Npgsql;
- migrations;
- RepositorioUnidade;
- RepositorioMaterial;
- RepositorioMovimentacaoEstoque.

### API

Responsável pela aplicação ASP.NET Core e futura exposição dos casos de uso por endpoints HTTP.

Atualmente possui:

- configuração do DbContext;
- configuração da conexão com PostgreSQL;
- registro dos repositórios por injeção de dependência;
- endpoint de status da aplicação.

## Persistência

As principais tabelas criadas pela migration inicial são:

- `unidades`;
- `materiais`;
- `movimentacoes_estoque`.

O histórico de migrations é controlado pelo Entity Framework Core através da tabela:

- `__EFMigrationsHistory`.

### Movimentações de estoque

Cada movimentação registra:

- unidade;
- material;
- tipo da movimentação;
- quantidade;
- data e hora.

Os tipos atualmente disponíveis são:

- Entrada;
- Saída.

A quantidade é armazenada no PostgreSQL como `numeric(18,3)`.

Também existem relacionamentos entre movimentações, unidades e materiais, além de índice para consultas por unidade e material.

## Regras de negócio implementadas

- unidade deve possuir nome;
- material deve possuir nome;
- unidade pode ser desativada;
- material pode ser desativado;
- movimentações exigem unidade e material existentes;
- quantidade deve ser maior que zero;
- movimentação pode ser de entrada ou saída;
- entrada aumenta o saldo;
- saída reduz o saldo;
- saída maior que o estoque disponível é bloqueada;
- unidade inativa não pode movimentar estoque;
- material inativo não pode ser movimentado.

## Configuração do banco

A aplicação não mantém senha de banco de dados no repositório.

Para desenvolvimento local, a connection string pode ser configurada utilizando .NET User Secrets.

A partir da pasta `backend`:

    dotnet user-secrets set \
      "ConnectionStrings:ControleMateriais" \
      "Host=SEU_HOST;Port=5432;Database=controle_materiais;Username=SEU_USUARIO;Password=SUA_SENHA" \
      --project src/ControleMateriais.Api/ControleMateriais.Api.csproj

Os valores devem ser adaptados ao ambiente local.

## Entity Framework Core

O projeto utiliza uma ferramenta local do .NET para execução do Entity Framework Core CLI.

Restaurar as ferramentas:

    dotnet tool restore

Aplicar migrations:

    dotnet tool run dotnet-ef database update \
      --project src/ControleMateriais.Infrastructure/ControleMateriais.Infrastructure.csproj \
      --startup-project src/ControleMateriais.Api/ControleMateriais.Api.csproj

## Status da API

Endpoint:

    GET /api/status

Resposta:

    {
      "status": "online",
      "aplicacao": "Controle de Materiais"
    }

## Testes automatizados

O projeto utiliza xUnit.

Atualmente existem:

    36 testes unitários
    1 teste de integração com PostgreSQL
    37 testes no total

Os 36 testes unitários podem ser executados normalmente:

    cd backend
    dotnet test ControleMateriais.slnx

O teste de integração utiliza um banco PostgreSQL separado e somente é executado quando a variável `TEST_CONNECTION_STRING` está configurada.

Exemplo:

    export TEST_CONNECTION_STRING="Host=SEU_HOST;Port=5432;Database=controle_materiais_testes;Username=SEU_USUARIO;Password=SUA_SENHA"

    dotnet test ControleMateriais.slnx

Depois da execução:

    unset TEST_CONNECTION_STRING

O teste de integração valida um fluxo real de persistência:

    criação de unidade
        ↓
    criação de material
        ↓
    entrada de estoque
        ↓
    saída de estoque
        ↓
    consulta das movimentações
        ↓
    cálculo do saldo
        ↓
    PostgreSQL

O teste utiliza transação e rollback para não manter os dados de teste gravados após a execução.

## Build

A partir da pasta `backend`:

    dotnet build ControleMateriais.slnx

## Executar a API

A partir da pasta `backend`:

    dotnet run --project src/ControleMateriais.Api/ControleMateriais.Api.csproj

## Status do projeto

### Bloco 1 — Ambiente e estrutura

Concluído.

### Bloco 2 — Domain e regras de negócio

Concluído.

### Bloco 3 — Application e casos de uso

Concluído.

### Bloco 4 — PostgreSQL e Entity Framework Core

Concluído.

Foram implementados:

- Entity Framework Core;
- Npgsql;
- PostgreSQL;
- DbContext;
- mapeamento das entidades;
- migration inicial;
- chaves estrangeiras;
- índices;
- repositórios reais;
- injeção de dependência;
- configuração segura da connection string;
- banco separado para testes;
- teste de integração com PostgreSQL real.

### Próxima etapa

Bloco 5 — API REST.

A próxima etapa irá expor os casos de uso já existentes através de endpoints HTTP.

## Autor

Bruno Ramos Lopes
