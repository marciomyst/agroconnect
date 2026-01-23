# AgroConnect – Roadmap Bank

Este documento consolida o **roadmap técnico e funcional** do AgroConnect, servindo como guia de execução incremental.

---

## 1. Fase Fundacional (F0.x)

### F0.1 — Shared Kernel ✅

- Entity
- AggregateRoot
- ValueObject
- DomainEvent
- Result / Guard

---

### F0.2 — CQRS Infrastructure ✅

- Commands / Queries
- Handlers
- Cross-cutting behaviors
- Wolverine runtime

---

### F0.3 — Persistence Base ⏳

- EF Core write model
- Dapper read model
- Unit of Work
- Infrastructure DI

---

## 2. Identity & Access (F1.x)

### F1.1 — RegisterUser ⏳

- User aggregate
- UserRegistered event
- POST /api/users/register

---

### F1.2 — AuthenticateUser ⏳

- JWT generation
- POST /api/auth/login
- JWT Bearer configuration

---

## 3. Organization & Membership (F2.x)

- F2.1 — RegisterSellerWithOwner
- F2.2 — RegisterFarmWithOwner
- F2.3 — GrantSellerMembership
- F2.4 — GrantFarmMembership

---

## 4. Commerce (F3.x – F5.x)

- Products
- Orders
- Checkout

---

## 5. Collective Deals (F6.x)

- CreateCollectiveDeal
- OpenCollectiveDeal
- CloseCollectiveDeal
- SettleCollectiveDeal (cashback)

---

## 6. Finance (F7.x)

- FarmWallet
- Cashback usage
- Wallet transactions

---

## 7. Diretrizes de Execução

- Implementar sempre por Feature
- Validar eventos antes de codar
- Evitar refatorações prematuras
- Consolidar aprendizados em documentos

---

## 8. Pontos de Retomada em Nova Conversa

Sugestões:
- "Vamos continuar do F0.3"
- "Implementar F1.2 — AuthenticateUser"
- "Iniciar F2.1 — RegisterSellerWithOwner"

