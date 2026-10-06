# Controle de Materiais

Projeto full stack para controle de materiais, estoque e movimentações entre unidades.

O projeto utiliza dados fictícios e foi desenvolvido com foco em aprendizado prático, arquitetura de software, API REST, persistência em banco de dados, autenticação, testes automatizados e boas práticas de desenvolvimento.

## Stack

### Backend

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- Npgsql
- ASP.NET Core Authentication
- JWT Bearer
- ASP.NET Core Identity PasswordHasher
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
- Usuario;
- UnidadeMedida;
- TipoMovimentacao;
- cálculo de saldo;
- validações de movimentação;
- validações de usuário;
- normalização de e-mail;
- ativação e desativação de usuário.

### Application

Coordena os casos de uso do sistema.

Atualmente possui:

- cadastro de unidades;
- cadastro de materiais;
- registro de entradas;
- registro de saídas;
- consulta de saldo;
- histórico de movimentações;
- filtros por unidade e material;
- resumo de estoque;
- cadastro de usuários;
- autenticação de usuários;
- contratos para geração e verificação de hash de senha;
- contrato para geração de token;
- contratos de persistência;
- DTOs de entrada e saída.

A camada Application depende de abstrações e não conhece diretamente Entity Framework Core, PostgreSQL ou os detalhes de implementação do JWT.

### Infrastructure

Implementa persistência e serviços de infraestrutura.

Atualmente possui:

- ControleMateriaisDbContext;
- configurações das entidades com Fluent API;
- integração com Entity Framework Core;
- integração com PostgreSQL através do Npgsql;
- migrations;
- RepositorioUnidade;
- RepositorioMaterial;
- RepositorioMovimentacaoEstoque;
- RepositorioUsuario;
- consultas filtradas de movimentações;
- ordenação de histórico por data;
- hash e verificação segura de senha com PasswordHasher.

### API

Expõe os casos de uso da aplicação através de endpoints HTTP.

Atualmente possui:

- configuração do DbContext;
- configuração da conexão com PostgreSQL;
- injeção de dependência;
- serialização de enums como texto;
- controllers REST;
- tratamento global de exceções;
- respostas HTTP adequadas para erros de negócio e recursos inexistentes;
- histórico de movimentações;
- filtros de consulta;
- resumo de estoque;
- cadastro de usuários;
- login;
- geração de JWT;
- autenticação JWT Bearer;
- autorização de endpoints através de `[Authorize]`.

## API REST

### Status

Endpoint público:

    GET /api/status

Resposta:

    {
      "status": "online",
      "aplicacao": "Controle de Materiais"
    }

### Criar usuário

Endpoint público:

    POST /api/usuarios

Exemplo:

    {
      "nome": "Bruno Lopes",
      "email": "bruno@exemplo.com",
      "senha": "MinhaSenha123!"
    }

Resposta esperada:

    HTTP 201 Created

Exemplo:

    {
      "id": "UUID_DO_USUARIO",
      "nome": "Bruno Lopes",
      "email": "bruno@exemplo.com",
      "ativo": true
    }

A senha original e o hash da senha não são retornados pela API.

O e-mail é normalizado antes do armazenamento e possui índice único no PostgreSQL.

### Login

Endpoint público:

    POST /api/autenticacao/login

Exemplo:

    {
      "email": "bruno@exemplo.com",
      "senha": "MinhaSenha123!"
    }

Resposta esperada:

    HTTP 200 OK

Exemplo:

    {
      "id": "UUID_DO_USUARIO",
      "nome": "Bruno Lopes",
      "email": "bruno@exemplo.com",
      "token": "JWT_GERADO_PELA_API"
    }

Credenciais inválidas retornam:

    HTTP 401 Unauthorized

Exemplo:

    {
      "title": "Não autorizado.",
      "status": 401,
      "detail": "E-mail ou senha inválidos."
    }

A API utiliza a mesma mensagem para usuário inexistente, senha incorreta ou usuário inativo.

## Autenticação JWT

Os endpoints de negócio exigem token JWT válido.

O token deve ser enviado no cabeçalho:

    Authorization: Bearer SEU_TOKEN

Exemplo:

    curl http://localhost:5062/api/movimentacoes \
      -H "Authorization: Bearer SEU_TOKEN"

Foram validados manualmente os seguintes cenários:

    sem token     -> HTTP 401
    token inválido -> HTTP 401
    token válido   -> HTTP 200

Atualmente permanecem públicos:

    GET  /api/status
    POST /api/usuarios
    POST /api/autenticacao/login

Os seguintes grupos de endpoints exigem autenticação:

    /api/unidades
    /api/materiais
    /api/movimentacoes
    /api/estoque

### Criar unidade

Requer autenticação.

    POST /api/unidades

Exemplo:

    {
      "nome": "Prédio Central"
    }

Resposta esperada:

    HTTP 201 Created

### Criar material

Requer autenticação.

    POST /api/materiais

Exemplo:

    {
      "nome": "Álcool 70%",
      "unidadeMedida": "Litro"
    }

Resposta esperada:

    HTTP 201 Created

### Registrar movimentação

Requer autenticação.

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

Requer autenticação.

    GET /api/estoque/saldo?unidadeId=UUID_DA_UNIDADE&materialId=UUID_DO_MATERIAL

Resposta de exemplo:

    {
      "unidadeId": "UUID_DA_UNIDADE",
      "unidadeNome": "Prédio Central",
      "materialId": "UUID_DO_MATERIAL",
      "materialNome": "Álcool 70%",
      "saldo": 7
    }

### Histórico de movimentações

Requer autenticação.

Listar todas:

    GET /api/movimentacoes

Filtrar por unidade:

    GET /api/movimentacoes?unidadeId=UUID_DA_UNIDADE

Filtrar por material:

    GET /api/movimentacoes?materialId=UUID_DO_MATERIAL

Filtrar por unidade e material:

    GET /api/movimentacoes?unidadeId=UUID_DA_UNIDADE&materialId=UUID_DO_MATERIAL

As movimentações são retornadas da mais recente para a mais antiga.

Exemplo:

    [
      {
        "id": "UUID_DA_MOVIMENTACAO",
        "unidadeId": "UUID_DA_UNIDADE",
        "materialId": "UUID_DO_MATERIAL",
        "tipo": "Saida",
        "quantidade": 3,
        "dataMovimentacao": "DATA_HORA"
      },
      {
        "id": "UUID_DA_MOVIMENTACAO",
        "unidadeId": "UUID_DA_UNIDADE",
        "materialId": "UUID_DO_MATERIAL",
        "tipo": "Entrada",
        "quantidade": 10,
        "dataMovimentacao": "DATA_HORA"
      }
    ]

### Resumo de estoque

Requer autenticação.

Resumo geral:

    GET /api/estoque/resumo

Resumo filtrado por unidade:

    GET /api/estoque/resumo?unidadeId=UUID_DA_UNIDADE

Resumo filtrado por material:

    GET /api/estoque/resumo?materialId=UUID_DO_MATERIAL

Resumo filtrado por unidade e material:

    GET /api/estoque/resumo?unidadeId=UUID_DA_UNIDADE&materialId=UUID_DO_MATERIAL

Exemplo:

    {
      "unidadeId": "UUID_DA_UNIDADE",
      "materialId": "UUID_DO_MATERIAL",
      "totalMovimentacoes": 2,
      "totalEntradas": 10,
      "totalSaidas": 3,
      "saldo": 7
    }

O resumo apresenta:

- quantidade total de movimentações;
- total de entradas;
- total de saídas;
- saldo calculado.

## Respostas HTTP

A API utiliza os seguintes códigos principais:

    200 OK
    201 Created
    400 Bad Request
    401 Unauthorized
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

Exemplo de credenciais inválidas:

    {
      "title": "Não autorizado.",
      "status": 401,
      "detail": "E-mail ou senha inválidos."
    }

Erros inesperados retornam mensagem genérica, sem exposição de detalhes internos ou stack trace ao cliente.

## Persistência

As tabelas atualmente existentes incluem:

- `unidades`;
- `materiais`;
- `movimentacoes_estoque`;
- `usuarios`.

O histórico de migrations é controlado pelo Entity Framework Core através da tabela:

- `__EFMigrationsHistory`.

### Usuários

A tabela `usuarios` armazena:

- identificador;
- nome;
- e-mail;
- hash da senha;
- status ativo.

O e-mail possui índice único.

A senha original não é armazenada.

A migration responsável pela inclusão da estrutura de usuários foi criada e aplicada através do Entity Framework Core.

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
- material inativo não pode ser movimentado;
- usuário deve possuir nome;
- usuário deve possuir e-mail válido;
- e-mail é normalizado para minúsculas;
- e-mail duplicado é bloqueado;
- senha de cadastro deve possuir pelo menos 8 caracteres;
- senha é transformada em hash antes da persistência;
- usuário inativo não pode autenticar;
- credenciais inválidas não geram token;
- endpoints protegidos exigem JWT válido.

## Consultas e relatórios

O sistema atualmente permite:

- consultar saldo por unidade e material;
- listar histórico completo;
- filtrar histórico por unidade;
- filtrar histórico por material;
- combinar filtros de unidade e material;
- ordenar movimentações pelas mais recentes;
- obter total de entradas;
- obter total de saídas;
- obter quantidade de movimentações;
- calcular saldo no resumo de estoque.

Os filtros são aplicados pela camada de persistência antes da materialização dos resultados.

## Segurança

### Senhas

O projeto utiliza `PasswordHasher` para geração e verificação do hash de senha.

Fluxo simplificado:

    senha original
        ↓
    PasswordHasher
        ↓
    hash seguro
        ↓
    PostgreSQL

A senha original não é armazenada no banco.

### JWT

A autenticação utiliza JWT Bearer.

O token contém informações de identificação do usuário e é assinado pela API.

A chave utilizada para assinatura não é mantida no repositório.

O tempo de expiração é configurável.

## Configuração local

A aplicação não mantém senha do PostgreSQL nem chave JWT no repositório.

Para desenvolvimento local, esses valores podem ser configurados através do .NET User Secrets.

### Connection string

A partir da pasta `backend`:

    dotnet user-secrets set \
      "ConnectionStrings:ControleMateriais" \
      "Host=SEU_HOST;Port=5432;Database=controle_materiais;Username=SEU_USUARIO;Password=SUA_SENHA" \
      --project src/ControleMateriais.Api/ControleMateriais.Api.csproj

### JWT

Gerar e armazenar uma chave aleatória:

    dotnet user-secrets set \
      "Jwt:Key" \
      "$(openssl rand -base64 48)" \
      --project src/ControleMateriais.Api/ControleMateriais.Api.csproj

Configurar emissor:

    dotnet user-secrets set \
      "Jwt:Issuer" \
      "ControleMateriais.Api" \
      --project src/ControleMateriais.Api/ControleMateriais.Api.csproj

Configurar audiência:

    dotnet user-secrets set \
      "Jwt:Audience" \
      "ControleMateriais.Client" \
      --project src/ControleMateriais.Api/ControleMateriais.Api.csproj

Configurar expiração em minutos:

    dotnet user-secrets set \
      "Jwt:ExpirationMinutes" \
      "120" \
      --project src/ControleMateriais.Api/ControleMateriais.Api.csproj

## Entity Framework Core

Restaurar as ferramentas:

    dotnet tool restore

Aplicar migrations:

    dotnet tool run dotnet-ef database update \
      --project src/ControleMateriais.Infrastructure/ControleMateriais.Infrastructure.csproj \
      --startup-project src/ControleMateriais.Api/ControleMateriais.Api.csproj

## Testes automatizados

O projeto utiliza xUnit.

Situação atual:

    54 testes executados com sucesso
    1 teste de integração com PostgreSQL ignorado quando não configurado
    55 testes no total
    0 testes com falha

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

Os testes automatizados incluem cenários de:

- entidades de domínio;
- regras de estoque;
- casos de uso;
- cálculo de saldo;
- criação de unidades;
- criação de materiais;
- registro de movimentações;
- persistência PostgreSQL;
- resumo de estoque;
- filtros de histórico;
- ordenação de movimentações;
- entidade Usuario;
- validação e normalização de e-mail;
- geração de hash de senha;
- verificação de senha correta;
- rejeição de senha incorreta;
- cadastro de usuário;
- bloqueio de e-mail duplicado;
- validação de tamanho mínimo da senha;
- autenticação com credenciais válidas;
- rejeição de credenciais inválidas;
- rejeição de usuário inexistente;
- rejeição de usuário inativo;
- geração de token através de abstração testável.

## Validação manual da API

O fluxo principal foi validado utilizando requisições HTTP reais contra PostgreSQL:

    criar usuário
        ↓
    login
        ↓
    receber JWT
        ↓
    criar unidade autenticado
        ↓
    criar material autenticado
        ↓
    registrar entrada
        ↓
    consultar saldo
        ↓
    registrar saída
        ↓
    consultar novo saldo
        ↓
    consultar histórico
        ↓
    aplicar filtros
        ↓
    consultar resumo

Também foram validados:

- hash da senha persistido no PostgreSQL;
- ausência de senha e hash nas respostas de cadastro;
- bloqueio de e-mail duplicado;
- login com credenciais válidas;
- resposta HTTP 401 para senha incorreta;
- resposta HTTP 401 para usuário inexistente;
- endpoint protegido sem token retornando HTTP 401;
- endpoint protegido com token inválido retornando HTTP 401;
- endpoint protegido com JWT válido retornando HTTP 200;
- bloqueio de saída maior que o saldo;
- resposta HTTP 400 para operação inválida;
- resposta HTTP 404 para recurso inexistente;
- tratamento global de exceções;
- histórico ordenado por data;
- filtros por unidade e material;
- cálculo de entradas, saídas e saldo no resumo.

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

### Bloco 6 — Consultas, histórico e relatórios

Concluído.

Foram implementados:

- histórico de movimentações;
- ordenação das movimentações por data;
- filtros opcionais por unidade;
- filtros opcionais por material;
- combinação de filtros;
- resumo de estoque;
- total de movimentações;
- total de entradas;
- total de saídas;
- saldo consolidado;
- testes automatizados do histórico;
- testes automatizados do resumo.

### Bloco 7 — Usuários, autenticação e JWT

Concluído.

Foram implementados:

- entidade Usuario;
- validação e normalização de e-mail;
- tabela `usuarios`;
- índice único de e-mail;
- migration de usuários;
- repositório de usuários;
- hash seguro de senha;
- cadastro de usuário;
- bloqueio de cadastro duplicado;
- autenticação por e-mail e senha;
- bloqueio de usuário inativo;
- geração de JWT;
- configuração de issuer e audience;
- expiração configurável;
- armazenamento da chave JWT fora do Git;
- autenticação JWT Bearer;
- proteção dos endpoints de negócio com `[Authorize]`;
- tratamento HTTP 401;
- testes automatizados de usuário, senha e autenticação;
- validação manual de token ausente, inválido e válido.

### Próxima etapa

Bloco 8 — Consolidação do backend.

O Bloco 8 será dedicado à revisão técnica final do backend, correção de pontos identificados, consolidação da API, execução da suíte completa de testes e fechamento da documentação.

O frontend permanece planejado para uma etapa futura.

## Autor

Bruno Ramos Lopes
