# AGENTS.md

Este arquivo define **como agentes de IA (ex.: Codex / GPT‑5.2‑Codex)** devem atuar neste repositório.

Ele é **normativo**: qualquer agente automatizado deve seguir estas regras ao gerar, modificar ou sugerir código.

---

## 1. Papel dos Agentes

Agentes de IA neste repositório atuam **exclusivamente como executores técnicos**.

Eles **não são arquitetos**, **não são product owners** e **não tomam decisões de domínio**.

Seu papel é:

* implementar User Stories já definidas
* respeitar a linguagem ubíqua
* seguir a arquitetura estabelecida

---

## 2. Fonte da Verdade (ordem de precedência)

Antes de gerar qualquer código, o agente **DEVE considerar**, nesta ordem:

1. **Linguagem Ubíqua** (`Linguagem Ubíqua — Agronomia`)
2. **User Feature / User Stories** fornecidas
3. **Event Storm** associado à feature
4. **Estrutura de pastas do repositório**
5. **Código existente**

Se houver conflito:

* **NUNCA** inventar uma solução
* **NUNCA** renomear conceitos
* **SEMPRE** sinalizar o conflito

---

## 3. Arquitetura Obrigatória

### 3.1. Estilo

* DDD (Domain‑Driven Design)
* Clean Architecture
* Vertical Slices por feature
* Event‑driven (eventos explícitos)

### 3.2. Regras Inquebráveis

* Aggregates **não referenciam** outros aggregates
* Nenhuma lógica de negócio em Controllers
* Toda escrita passa por **Application Services**
* Eventos de domínio são explícitos
* Membership é um conceito de domínio

---

## 4. Estrutura de Pastas (obrigatória)

```text
src/
├── Agronomia.Api
│   └── Features
│
├── Agronomia.Application
│   └── Features
│
├── Agronomia.Domain
│   ├── Common
│   ├── Identity
│   ├── Organizations
│   ├── Memberships
│   ├── Catalog
│   ├── Orders
│   ├── CollectiveDeals
│   └── Finance
│
├── Agronomia.Infrastructure
│   └── Persistence
│
└── Agronomia.Tests
```

Agentes **não devem**:

* criar pastas fora desse padrão
* mover código existente sem instrução explícita

---

## 5. Linguagem Ubíqua (obrigatória)

Agentes devem usar **exatamente** os termos definidos no documento de Linguagem Ubíqua.

### Exemplos obrigatórios

* `Seller` (não usar `Reseller`, `Company`, `Vendor`)
* `Farm` (não usar `Producer`, `Customer`)
* `Membership` (não usar `Link`, `Relation`)
* `FarmWallet` (não usar `Balance`, `Account`)
* `CollectiveDeal` (não usar `GroupSale`)

Se um termo necessário **não existir**:

* parar a implementação
* solicitar definição explícita

---

## 6. Comandos, Eventos e Nomes

### 6.1. Commands

* Representam **intenção**
* Usam verbo no infinitivo

Exemplos:

* `RegisterSellerWithOwner`
* `GrantSellerMembership`

### 6.2. Domain Events

* Representam algo que **já aconteceu**
* Usam passado

Exemplos:

* `SellerRegistered`
* `SellerMembershipGranted`
* `FarmWalletCredited`

---

## 7. Código de Domínio

### 7.1. Aggregates

* Devem herdar de `AggregateRoot`
* Devem proteger invariantes internamente
* Devem emitir Domain Events

### 7.2. Value Objects

* Imutáveis
* Comparação por valor

---

## 8. Application Layer

* Orquestra fluxos
* Coordena múltiplos agregados
* Não contém regras de negócio profundas

Agentes **não devem**:

* mover regras para Controllers
* acessar EF Core diretamente

---

## 9. Persistência

* Repositórios por Aggregate
* Interfaces no Domain ou Application
* Implementações na Infrastructure

Agentes **não devem**:

* usar DbContext no domínio
* usar modelos de persistência como domínio

---

## 10. Testes

Agentes devem:

* criar testes unitários para regras de domínio
* criar testes de Application Services quando solicitado

Agentes **não devem**:

* pular testes sem autorização explícita

---

## 11. O que o agente **NÃO PODE FAZER**

* ❌ Inventar novos conceitos de negócio
* ❌ Renomear termos da linguagem ubíqua
* ❌ Refatorar arquitetura sem solicitação
* ❌ Criar dependência entre agregados
* ❌ Aplicar padrões “genéricos” sem contexto

---

## 12. Comunicação de Incertezas

Quando houver dúvida:

* parar
* explicar o problema
* sugerir opções
* aguardar decisão humana

Nunca assumir.

---

## 13. Objetivo Final

O objetivo dos agentes neste repositório é:

> **produzir código correto, legível, alinhado ao domínio e sustentável no longo prazo**.

Velocidade **não** é prioridade.
Clareza, consistência e fidelidade ao domínio **são**.

---

**Este arquivo é vinculante para todos os agentes de IA.**
