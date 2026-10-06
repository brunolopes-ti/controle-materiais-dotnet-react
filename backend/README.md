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

Expõe os casos de uso da aplicação através de endpoints HTTP.

Atualmente possui:

- configuração do DbContext;
- configuração da conexão com PostgreSQL;
- injeção de dependência;
- serialização de enums como texto;
- controllers REST;
- tratamento global de exceções;
- respostas HTTP adequadas para erros de negócio e recursos inexistentes.

## API REST

### Status

    GET /api/status

Resposta:

    {
      "status": "online",
      "aplicacao": "Controle de Materiais"
    }

### Criar unidade

    POST /api/unidades

Exemplo:

    {
      "nome": "Prédio Central"
    }

Resposta esperada:

    HTTP 201 Created

### Criar material

    POST /api/materiais

Exemplo:

    {
      "nome": "Álcool 70%",
      "unidadeMedida": "Litro"
    }

Resposta esperada:

    HTTP 201 Created

### Registrar movimentação

    POST /api/movimentacoes

Entrada:

    {
      "unidadeId": "UUID_DA_UNIDADE",
      "materialId": "UUID_DO_MATERIAL",
      "tipo": "Entrada",
      "quantidade": 10
    }

Saída:

    {
      "unidadeId": "UUID_DA_UNIDADE",
      "materialId": "UUID_DO_MATERIAL",
      "tipo": "Saida",
      "quantidade": 3
    }

Resposta esperada:

    HTTP 201 Created

### Consultar saldo

    GET /api/estoque/saldo?unidadeId=UUID_DA_UNIDADE&materialId=UUID_DO_MATERIAL

Resposta de exemplo:

    {
      "unidadeId": "UUID_DA_UNIDADE",
      "unidadeNome": "Prédio Central",
      "materialId": "UUID_DO_MATERIAL",
      "materialNome": "Álcool 70%",
      "saldo": 7
    }

## Respostas HTTP

A API utiliza os seguintes códigos principais:

    200 OK
    201 Created
    400 Bad Request
    404 Not Found
    500 Internal Server Error

Erros conhecidos de negócio são tratados globalmente pela API.

Exemplo de tentativa de saída maior que o saldo:

    {
      "title": "Operação inválida.",
      "status": 400,
      "detail": "Estoque insuficiente para realizar a saída."
    }

Exemplo de recurso inexistente:

    {
      "title": "Recurso não encontrado.",
      "status": 404,
      "detail": "Unidade não encontrada."
    }

Erros inesperados retornam uma mensagem genérica, sem exposição de detalhes internos ou stack trace ao cliente.

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
- Saida.

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

Restaurar as ferramentas:

    dotnet tool restore

Aplicar migrations:

    dotnet tool run dotnet-ef database update \
      --project src/ControleMateriais.Infrastructure/ControleMateriais.Infrastructure.csproj \
      --startup-project src/ControleMateriais.Api/ControleMateriais.Api.csproj

## Testes automatizados

O projeto utiliza xUnit.

Atualmente existem:

    36 testes unitários
    1 teste de integração com PostgreSQL
    37 testes no total

Executar:

    cd backend
    dotnet test ControleMateriais.slnx

Sem `TEST_CONNECTION_STRING`, o teste PostgreSQL é ignorado propositalmente.

Para executar também o teste de integração:

    export TEST_CONNECTION_STRING="Host=SEU_HOST;Port=5432;Database=controle_materiais_testes;Username=SEU_USUARIO;Password=SUA_SENHA"

    dotnet test ControleMateriais.slnx

Depois:

    unset TEST_CONNECTION_STRING

O teste de integração utiliza transação e rollback para não manter os dados de teste gravados após sua execução.

## Validação manual da API

O fluxo principal já foi validado utilizando requisições HTTP reais contra PostgreSQL:

    criar unidade
        ↓
    criar material
        ↓
    registrar entrada
        ↓
    consultar saldo
        ↓
    registrar saída
        ↓
    consultar novo saldo

Também foram validados:

- bloqueio de saída maior que o saldo;
- resposta HTTP 400 para operação inválida;
- resposta HTTP 404 para recurso inexistente;
- tratamento global de exceções.

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

### Bloco 5 — API REST

Concluído.

Foram implementados:

- controllers REST;
- cadastro de unidades;
- cadastro de materiais;
- registro de entradas e saídas;
- consulta de saldo;
- enums em formato textual no JSON;
- códigos HTTP adequados;
- tratamento global de exceções;
- validação do fluxo completo utilizando PostgreSQL real.

### Próxima etapa

Bloco 6 — Consultas, histórico e relatórios.

## Autor

Bruno Ramos Lopes
