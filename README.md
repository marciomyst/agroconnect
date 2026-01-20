# 🌱 Agronomia

**Agronomia** é uma **plataforma de e-commerce para o agronegócio**, construída para crescer de forma **incremental, previsível e segura**, suportando:

* múltiplos perfis de usuário (Public, Farmer, Seller, Admin)
* múltiplas organizações por usuário (Farms e Sellers)
* produtos comuns e **produtos controlados por receita agronômica**
* cashback e carteira para produtores
* evolução contínua sem refatorações estruturais

O projeto prioriza **domínio e arquitetura antes de funcionalidades**.

---

## 🎯 Visão do Produto

O Agronomia conecta:

### 🌾 Farmers

Produtores rurais que:

* compram insumos
* utilizam receitas aprovadas por agrônomos
* acumulam cashback em carteira

### 🏪 Sellers

Revendas, cooperativas e fabricantes que:

* cadastram produtos e ofertas
* criam pools (compras coletivas)
* acompanham faturamento

### 👥 Usuários Públicos

Usuários não autenticados que:

* navegam no marketplace
* pesquisam produtos
* montam carrinho antes do login

👉 **Toda operação ocorre dentro de um contexto organizacional explícito** (Farm ou Seller).

---

## 🧠 Princípios Arquiteturais

Princípios **não negociáveis** do projeto:

* Multi-organização é regra, não exceção
* Autenticação ≠ autorização ≠ contexto organizacional
* UI nunca decide permissões
* Guards vivem nas rotas, nunca em componentes
* Domínio não conhece infraestrutura
* Frontend espelha o modelo de Membership do backend
* Organização ativa é sempre explícita

---

## 🏗️ Arquitetura Geral

### Backend

* .NET 10
* ASP.NET Core
* DDD + Clean Architecture
* CQRS + Vertical Slice
* Wolverine
* EF Core (Write Model)
* Dapper (Read Model)
* JWT

### Frontend

* Angular moderno
* Standalone routes
* TailwindCSS
* Angular Signals
* Lazy loading por área
* Route guards com `canMatch`

---

## 📂 Estrutura do Repositório

```text
/src
 ├── Agronomia.Api
 ├── Agronomia.Application
 ├── Agronomia.Domain
 ├── Agronomia.Infrastructure
 ├── Agronomia.Crosscutting
 └── Agronomia.Angular        ← frontend (template padrão .NET)

/docs
 ├── event-storm/             ← event storming e descoberta de domínio
 ├── features/
 │    ├── backend/            ← features backend (F0.x, F1.x, F2.x…)
 │    └── frontend/           ← features frontend (F-Front 0.x, 1.x…)
 ├── prompts/                 ← prompts de IA / Codex
 ├── ARCHITECTURE.md          ← decisões arquiteturais consolidadas
 ├── DOMAIN_EVENTS.md         ← catálogo de eventos de domínio
 ├── UBIQUOUS_LANGUAGE.md     ← linguagem ubíqua oficial
 ├── ROADMAP_BACKEND.md       ← roadmap do backend
 └── ROADMAP_FRONTEND.md      ← roadmap do frontend
```

---

## 📝 Nota sobre o projeto `Agronomia.Angular`

O projeto **`Agronomia.Angular`** representa o **frontend Angular** da aplicação.

O nome é mantido **temporariamente** por conveniência técnica, pois deriva do **template padrão do .NET**.

* Conceitualmente tratado apenas como **Frontend do Agronomia**
* Nenhum conceito de domínio depende do nome `Angular`
* Acoplamento com backend ocorre **exclusivamente via contratos HTTP**
* Renomeação futura é segura e não impactante

---

## 🧩 Documentação Orientada a Features (Fonte de Verdade)

A documentação do projeto é **orientada a features**, não a camadas técnicas.

Cada feature possui um documento próprio contendo:

* objetivo
* escopo
* regras de negócio
* decisões arquiteturais
* critérios de aceitação

### 📁 Backend Features

Local:

```
docs/features/backend
```

Exemplos:

* `F0.1 - SHARED KERNEL.md`
* `F0.2 - CQRS INFRASTRUCTURE.md`
* `F0.3 - PERSISTENCE.md`
* `F1.1 - USER REGISTER.md`
* `F1.2 - AUTHENTICATE USER.md`
* `F1.3 - MEMBERSHIP CONTEXT.md`
* `F2.x - ORGANIZATIONS & MEMBERSHIP`

---

### 📁 Frontend Features

Local:

```
docs/features/frontend
```

Exemplos:

* `F-Front 0.1 - MODULE AND ROUTING.md`
* `F-Front 0.2 - AUTH AND CONTEXT STORE.md`
* `F-Front 0.3 - ORGANIZATION CONTEXT.md`

---

## 🧠 Artefatos de Domínio

Os seguintes documentos são **referências globais** e devem ser respeitados por todas as features:

* 📄 `ARCHITECTURE.md`
  Decisões arquiteturais não negociáveis

* 📄 `DOMAIN_EVENTS.md`
  Catálogo oficial de eventos de domínio

* 📄 `UBIQUOUS_LANGUAGE.md`
  Linguagem ubíqua do projeto

* 📄 `event-storm/`
  Descoberta, exploração e evolução do domínio

---

## 🗺️ Roadmaps Oficiais

* **Backend:** `docs/ROADMAP_BACKEND.md`
* **Frontend:** `docs/ROADMAP_FRONTEND.md`

Os roadmaps definem **ordem de evolução**, mas **não substituem** os documentos de feature.

---

## ✅ Estado Atual

### Backend

* F0.1 — Shared Kernel ✅
* F0.2 — CQRS Infrastructure ✅
* F0.3 — Persistence Base ⏳
* F1.x — Identity & Access em andamento

### Frontend

* F-Front 0.1 — Routing & Modules ✅
* F-Front 0.2 — Auth & Context Store ✅
* F-Front 0.3 — Organization Context ✅

---

## 🧪 Como Evoluir o Projeto

### Regras de ouro

1. Nenhuma feature altera decisões de F0.x
2. Toda feature nova:

   * começa por um documento em `docs/features`
   * respeita `ARCHITECTURE.md` e `UBIQUOUS_LANGUAGE.md`
3. Organização ativa é obrigatória em fluxos de negócio
4. Commits e PRs devem referenciar a feature (ex.: `F2.1`, `F-Front 1.0`)

---

## 🧭 Context Bootstrap (Novo Chat / IA)

Use este texto para iniciar um novo chat com IA:

> Estamos trabalhando no projeto Agronomia.
> Backend em .NET 10 com DDD, CQRS e Clean Architecture.
> Frontend em Angular moderno (projeto `Agronomia.Angular`) com standalone routes e Signals.
> F-Front 0.1–0.3 concluídos (routing, auth, current user, active organization).
> Documentação orientada a features em `docs/features`.
> Roadmaps, Architecture, Domain Events e Ubiquitous Language estão atualizados.
> Vamos continuar a partir do próximo item do roadmap.