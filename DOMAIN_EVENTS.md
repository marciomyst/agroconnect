# DOMAIN_EVENTS.md

Este documento cataloga **todos os eventos de domínio oficiais** do sistema Agronomia.

Ele é **normativo** e complementa:

* `Linguagem Ubíqua — Agronomia`
* `ARCHITECTURE.md`
* `AGENTS.md`

> **Regra:** se um evento não estiver neste documento, ele **não deve ser criado no código** sem atualização prévia aqui.

---

## 1. Princípios para Eventos de Domínio

1. Eventos representam **fatos que já aconteceram**
2. Sempre nomeados no **passado**
3. Expressam algo **relevante para o negócio**
4. Não representam intenções ou comandos
5. Devem ser emitidos por **Aggregates**

---

## 2. Convenções Gerais

### 2.1. Nomenclatura

* Formato: `<Entidade><Acontecimento>`

* Exemplos válidos:

  * `UserRegistered`
  * `SellerRegistered`
  * `FarmWalletCredited`

* Exemplos inválidos:

  * `RegisterUser`
  * `CreateSeller`
  * `CreditWallet`

---

### 2.2. Estrutura Básica

Todos os eventos devem herdar de `DomainEvent`.

```csharp
public abstract record DomainEvent(
    Guid EventId,
    DateTime OccurredAt
);
```

---

## 3. Identity & Access (IAM)

### UserRegistered

Disparado quando um novo **User** é criado.

**Origem:** Aggregate `User`

**Payload mínimo:**

* `UserId`
* `Email`

---

### UserAuthenticated

Disparado quando um User autentica com sucesso.

**Origem:** Application Layer

**Payload mínimo:**

* `UserId`
* `AuthenticatedAt`

---

### UserPasswordChanged

Disparado quando um User altera sua senha.

**Origem:** Aggregate `User`

**Payload mínimo:**

* `UserId`

---

## 4. Organizations

### SellerRegistered

Disparado quando uma **Seller (Revenda)** é registrada no sistema.

**Origem:** Aggregate `Seller`

**Payload mínimo:**

* `SellerId`
* `TaxId`
* `CorporateName`

---

### FarmRegistered

Disparado quando uma **Farm (Produtor)** é registrada.

**Origem:** Aggregate `Farm`

**Payload mínimo:**

* `FarmId`
* `TaxId`

---

## 5. Memberships

### SellerMembershipGranted

Disparado quando um User recebe um papel em uma Seller.

**Origem:** Aggregate `SellerMembership`

**Payload mínimo:**

* `SellerId`
* `UserId`
* `Role`

---

### FarmMembershipGranted

Disparado quando um User recebe um papel em uma Farm.

**Origem:** Aggregate `FarmMembership`

**Payload mínimo:**

* `FarmId`
* `UserId`
* `Role`

---

## 6. Catalog & Pricing

### ProductPublished

Disparado quando um produto é publicado por um Seller.

**Origem:** Aggregate `Product`

**Payload mínimo:**

* `ProductId`
* `SellerId`

---

### ProductUnpublished

Disparado quando um produto deixa de estar disponível.

**Origem:** Aggregate `Product`

**Payload mínimo:**

* `ProductId`
* `SellerId`

---

## 7. Orders

### OrderPlaced

Disparado quando um pedido é criado por uma Farm.

**Origem:** Aggregate `Order`

**Payload mínimo:**

* `OrderId`
* `SellerId`
* `FarmId`

---

### OrderConfirmed

Disparado quando um pedido é confirmado.

**Origem:** Aggregate `Order`

**Payload mínimo:**

* `OrderId`

---

### OrderCancelled

Disparado quando um pedido é cancelado.

**Origem:** Aggregate `Order`

**Payload mínimo:**

* `OrderId`

---

## 8. Collective Deals

### CollectiveDealCreated

Disparado quando um Seller cria um pool de vendas.

**Origem:** Aggregate `CollectiveDeal`

**Payload mínimo:**

* `CollectiveDealId`
* `SellerId`

---

### CollectiveDealOpened

Disparado quando um pool é aberto para participação.

**Origem:** Aggregate `CollectiveDeal`

**Payload mínimo:**

* `CollectiveDealId`

---

### CollectiveDealClosed

Disparado quando um pool é encerrado.

**Origem:** Aggregate `CollectiveDeal`

**Payload mínimo:**

* `CollectiveDealId`

---

## 9. Finance / Wallet

### FarmWalletCreated

Disparado quando a carteira de uma Farm é criada.

**Origem:** Aggregate `FarmWallet`

**Payload mínimo:**

* `FarmId`

---

### FarmWalletCredited

Disparado quando a carteira de uma Farm recebe crédito (cashback).

**Origem:** Aggregate `FarmWallet`

**Payload mínimo:**

* `FarmId`
* `Amount`
* `Origin`

---

### FarmWalletDebited

Disparado quando a carteira de uma Farm é utilizada como pagamento.

**Origem:** Aggregate `FarmWallet`

**Payload mínimo:**

* `FarmId`
* `Amount`
* `Origin`

---

## 10. Eventos Transversais (opcionais)

### SellerOnboardingStarted

Disparado após o cadastro completo de uma Seller.

**Origem:** Application Layer

**Payload mínimo:**

* `SellerId`

---

## 11. Governança

* Este documento **deve evoluir junto com o domínio**
* Novos eventos exigem:

  1. atualização deste arquivo
  2. validação com a Linguagem Ubíqua
  3. revisão arquitetural

---

**Este documento é parte do código.**
