# ARCHITECTURE.md

Este documento orienta **como o código deve ser escrito** neste repositório, para humanos e para agentes de IA.

Ele complementa:

* `Linguagem Ubíqua — Agronomia`
* `AGENTS.md`

---

## 1. Princípios Arquiteturais

1. **DDD (Domain-Driven Design)**
2. **Clean Architecture**
3. **CQRS** (separação clara entre comandos e queries)
4. **Vertical Slice Architecture** (feature/use-case como unidade)
5. **Mensageria/Wolverine** como infraestrutura de comandos/eventos
6. **Dapper para leitura (read models)**
7. **Repositórios de escrita por Aggregate**

---

## 2. Stack Técnica

* **Backend:** .NET 10
* **Mensageria / CQRS:** Wolverine
* **ORM de escrita:** EF Core (ou similar) para agregados de domínio
* **Leitura:** Dapper em read-only repositories
* **Autenticação:** JWT próprio

---

## 3. Estrutura de Pastas

```text
src/
├── Agronomia.Api               # HTTP / Endpoints / Autenticação
│   └── Features                  # Vertical Slices (camada de borda)
│
├── Agronomia.Application       # Casos de uso / Orquestração
│   ├── Abstractions              # Interfaces comuns
│   ├── Behaviors                 # Pipeline (logging, validação, etc.)
│   └── Features                  # Vertical Slices (Application)
│
├── Agronomia.Domain            # Modelo de domínio
│   ├── Common                    # Entity, AggregateRoot, ValueObject, Events
│   ├── Identity                  # User, Auth
│   ├── Organizations             # Seller, Farm
│   ├── Memberships               # SellerMembership, FarmMembership
│   ├── Catalog                   # Product, Price
│   ├── Orders                    # Order
│   ├── CollectiveDeals           # CollectiveDeal, DealLedger
│   └── Finance                   # FarmWallet
│
├── Agronomia.Infrastructure    # Persistência, Wolverine, integrações
│   ├── Persistence               # DbContext, mappings, migrations
│   ├── Repositories              # Implementações de repositório
│   └── Messaging                 # Wolverine configuration
│
└── Agronomia.Tests             # Testes de domínio e aplicação
```

---

## 4. Conceito de Feature / Use Case

### 4.1. Definição

Uma **Feature** (ou **Use Case**) é a **unidade mínima de funcionalidade** implementada ponta a ponta:

* Endpoint HTTP (Api)
* Command/Query + Handler (Application)
* Interações com domínio (Domain)
* Persistência (Infrastructure)

### 4.2. Estrutura de uma Feature

Exemplo: `RegisterSellerWithOwner`

```text
Agronomia.Api
 └── Features
     └── Sellers
         └── RegisterSellerWithOwner
             ├── RegisterSellerWithOwnerEndpoint.cs
             └── RegisterSellerWithOwnerRequest.cs

Agronomia.Application
 └── Features
     └── Sellers
         └── RegisterSellerWithOwner
             ├── RegisterSellerWithOwnerCommand.cs
             ├── RegisterSellerWithOwnerResponse.cs
             └── RegisterSellerWithOwnerHandler.cs
```

Regra:

* **Uma pasta por feature**, replicada em Api e Application.
* Nomes alinhados à Linguagem Ubíqua.

---

## 5. CQRS: Commands vs Queries

### 5.1. Commands

* Representam **intenção de mudança de estado**
* Devem ser **side-effecting**
* Podem retornar um ID, um resumo ou nada (por convenção, retornam algo útil)

Padrão:

```csharp
public sealed record RegisterSellerWithOwnerCommand( ... ) : ICommand<RegisterSellerWithOwnerResult>;
```

### 5.2. Queries

* Não alteram estado
* Retornam DTOs de leitura
* Usam Dapper ou consultas otimizadas

Padrão:

```csharp
public sealed record GetSellerDetailsQuery(Guid SellerId) : IQuery<SellerDetailsDto>;
```

### 5.3. Interfaces base (Application)

```csharp
public interface ICommand<TResult> { }
public interface IQuery<TResult> { }

public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken ct);
}

public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken ct);
}
```

> Wolverine pode ser integrado implementando suas interfaces/marcadores equivalentes ou adaptadores.

---

## 6. Uso de Wolverine

Wolverine será utilizado como **infraestrutura de mensageria e pipeline de CQRS**.

### 6.1. Regras

1. Commands e Queries podem ser mensagens Wolverine quando fizer sentido.
2. Handlers de Commands/Queries devem seguir o padrão **Request/Response** descrito acima.
3. Wolverine é **infraestrutura**, não domínio:

   * A lógica de negócio não depende de Wolverine diretamente.
   * Se Wolverine for removido, o domínio e a aplicação continuam válidos.

### 6.2. Integração típica

* Commands e Queries podem ser:

  * tratados via Wolverine Handlers
  * publicados de forma assíncrona quando necessário
* Domain Events relevantes podem ser "levantados" no domínio e depois **convertidos em mensagens Wolverine** na camada de Infrastructure.

---

## 7. Persistência: EF Core (write) + Dapper (read)

### 7.1. Escrita (Write Model)

* Agregados de domínio são persistidos via **EF Core** ou similar.
* Repositórios expostos por interfaces na camada de domínio/aplicação.

Exemplo de interface:

```csharp
public interface ISellerRepository
{
    Task<Seller?> GetByIdAsync(SellerId id, CancellationToken ct = default);
    Task AddAsync(Seller seller, CancellationToken ct = default);
}
```

### 7.2. Leitura (Read Model)

* Queries utilizam **Dapper** para montar DTOs específicos
* Não expõem agregados de domínio, apenas modelos de leitura

Exemplo:

```csharp
public sealed class SellerReadRepository : ISellerReadRepository
{
    private readonly IDbConnection _connection;

    public Task<SellerDetailsDto?> GetDetailsAsync(Guid sellerId)
    {
        const string sql = "SELECT ... FROM Sellers WHERE Id = @SellerId";
        return _connection.QuerySingleOrDefaultAsync<SellerDetailsDto>(sql, new { SellerId = sellerId });
    }
}
```

Regras:

* **Nunca** mapear Aggregate Root diretamente com Dapper.
* Dapper só para leitura, pensando em performance e projeções.

---

## 8. Request / Response (HTTP vs Application)

### 8.1. Nível HTTP (Api)

* Request/Response alinhados ao contrato HTTP
* Podem ser diferentes dos Commands/Queries internos

Exemplo:

```csharp
public sealed record RegisterSellerWithOwnerHttpRequest(
    string TaxId,
    string CorporateName,
    string TradeName,
    string OwnerName,
    string OwnerEmail,
    string Password
);

public sealed record RegisterSellerWithOwnerHttpResponse(
    Guid SellerId,
    Guid UserId
);
```

### 8.2. Nível Application

* Commands/Queries podem ter estrutura mais rica (VOs, IDs fortes)
* Devem usar nomes da Linguagem Ubíqua

Exemplo:

```csharp
public sealed record RegisterSellerWithOwnerCommand(
    TaxId TaxId,
    string CorporateName,
    string TradeName,
    string OwnerName,
    Email OwnerEmail,
    string Password
);

public sealed record RegisterSellerWithOwnerResult(
    Guid SellerId,
    Guid UserId
);
```

Regra:

* O mapeamento entre HTTP Request ↔ Command é responsabilidade das **Features na camada Api**.

---

## 9. Domínio: Aggregates, Value Objects e Events

### 9.1. Aggregates

* Herdam de `AggregateRoot`
* Expõem apenas métodos que mantenham invariantes válidos
* Levantam eventos via `AddDomainEvent`

### 9.2. Value Objects

* Imutáveis
* Sem identidade própria
* Comparados por valor

### 9.3. Domain Events

* Classe base `DomainEvent`
* Nomenclatura alinhada à Linguagem Ubíqua
* Disparados a partir dos Aggregates

---

## 10. Convenções de Nomenclatura

* **Commands:** verbo no infinitivo + contexto

  * `RegisterSellerWithOwnerCommand`
  * `GrantSellerMembershipCommand`

* **Queries:** `Get` + alvo + `Query`

  * `GetSellerDetailsQuery`

* **Handlers:** `<NomeDoCommandOuQuery>Handler`

* **HTTP Endpoints:** `<FeatureName>Endpoint`

  * `RegisterSellerWithOwnerEndpoint`

---

## 11. Testes

* **Domínio:** testar invariantes de Aggregates e Value Objects
* **Application:** testar Handlers isolados (mockando repositórios)
* **Api (opcional):** testes de integração para fluxos críticos

Regra:

* Features relevantes devem ter ao menos testes de domínio ou aplicação.

---

## 12. Como evoluir este documento

* Toda nova decisão arquitetural deve ser refletida aqui.
* Mudanças estruturais exigem revisão arquitetural.
* Agentes de IA devem consultar este arquivo antes de gerar código.

---

**Este arquivo é vinculante para humanos e agentes.**
