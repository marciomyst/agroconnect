Perfeito. Vou te entregar uma **lista de features iniciais**, **ordenada**, **enxuta** e **alinhada com tudo que já foi definido** (Linguagem Ubíqua, ARCHITECTURE.md, AGENTS.md, DOMAIN_EVENTS.md, Wolverine + CQRS).

A ideia é: **cada feature constrói uma fundação real**, sem pular etapas, sem gerar dívida.

---

# 🧱 FASE 0 — FUNDAÇÃO (obrigatória)

> Sem essas features, o resto vira gambiarra.

### F0.1 — Shared Kernel

**Objetivo:** base técnica do domínio

* Entity
* AggregateRoot
* ValueObject
* DomainEvent
* Result / Error (opcional)
* Guard clauses

📌 *Não tem endpoint. Só código base.*

---

### F0.2 — Infraestrutura de CQRS (Wolverine)

**Objetivo:** pipeline consistente de commands/queries/events

* Configuração do Wolverine
* Convenções de handlers
* Integração com DI
* Logging / validation behaviors

📌 *Nenhuma feature de negócio ainda.*

---

### F0.3 — Persistência Base

**Objetivo:** separar write vs read desde o início

* DbContext para write model
* Migrations iniciais
* Setup de Dapper
* ConnectionFactory

---

# 🔐 FASE 1 — IDENTITY & ACCESS (IAM)

> O sistema só “existe” quando alguém consegue entrar.

### F1.1 — RegisterUser

* Criar usuário (email + senha)
* Hash de senha
* Evento: `UserRegistered`

---

### F1.2 — AuthenticateUser

* Login
* Validação de credenciais
* Geração de JWT
* Evento: `UserAuthenticated`

---

### F1.3 — ChangeUserPassword

* Alteração de senha
* Validações
* Evento: `UserPasswordChanged`

---

# 🏢 FASE 2 — ORGANIZAÇÕES + MEMBERSHIP

> Aqui o sistema começa a representar o mundo real.

### F2.1 — RegisterSellerWithOwner ⭐ (feature gênesis)

* Criar Seller
* Criar User (se não existir)
* Criar SellerMembership (Owner)
* Evento: `SellerRegistered`
* Evento: `SellerMembershipGranted`

---

### F2.2 — RegisterFarmWithOwner

* Criar Farm
* Criar FarmMembership (Owner)
* Evento: `FarmRegistered`
* Evento: `FarmMembershipGranted`

---

### F2.3 — GrantSellerMembership

* Adicionar User a Seller
* Definir Role
* Evento: `SellerMembershipGranted`

---

### F2.4 — GrantFarmMembership

* Adicionar User a Farm
* Definir Role
* Evento: `FarmMembershipGranted`

---

# 📦 FASE 3 — CATÁLOGO & PREÇOS (SELLER)

> Agora o Seller começa a operar.

### F3.1 — PublishProduct

* Criar produto
* Associar a Seller
* Evento: `ProductPublished`

---

### F3.2 — UnpublishProduct

* Desativar produto
* Evento: `ProductUnpublished`

---

### F3.3 — UpdateProductPrice

* Atualizar preço base
* Histórico de preços (opcional)
* Evento: `ProductPriceUpdated` *(se decidir criar)*

---

# 🧾 FASE 4 — FARM + WALLET (FINANCE)

> Preparação para vendas e cashback.

### F4.1 — CreateFarmWallet

* Criar carteira ao registrar Farm
* Evento: `FarmWalletCreated`

---

### F4.2 — CreditFarmWallet

* Crédito (cashback)
* Evento: `FarmWalletCredited`

---

### F4.3 — DebitFarmWallet

* Uso do saldo como pagamento
* Evento: `FarmWalletDebited`

---

# 🛒 FASE 5 — PEDIDOS

> Compra tradicional, sem pool ainda.

### F5.1 — PlaceOrder

* Criar pedido
* Validar Membership (Buyer)
* Evento: `OrderPlaced`

---

### F5.2 — ConfirmOrder

* Confirmar pedido
* Evento: `OrderConfirmed`

---

### F5.3 — CancelOrder

* Cancelar pedido
* Evento: `OrderCancelled`

---

# 👥 FASE 6 — VENDA COLETIVA (POOL)

> Só entra quando tudo acima estiver sólido.

### F6.1 — CreateCollectiveDeal

* Criar pool
* Definir tiers
* Evento: `CollectiveDealCreated`

---

### F6.2 — OpenCollectiveDeal

* Abrir pool
* Evento: `CollectiveDealOpened`

---

### F6.3 — CloseCollectiveDeal

* Encerrar pool
* Evento: `CollectiveDealClosed`

---

### F6.4 — SettleCollectiveDeal

* Calcular volume
* Gerar cashback
* Crédito em FarmWallet

---

# 📊 FASE 7 — QUERIES (READ MODELS)

> Só depois que os writes estiverem estáveis.

Exemplos:

* GetSellerDetails
* GetFarmDetails
* GetSellerProducts
* GetFarmWalletBalance
* GetOrderHistory

📌 Todas com **Dapper**, sem tocar em Aggregates.

---

# ✅ Ordem recomendada REAL (se eu estivesse no seu lugar)

1. F0.1 → F0.3
2. F1.1 → F1.2
3. **F2.1 (RegisterSellerWithOwner)**
4. F2.2
5. F4.1
6. F3.1
7. F5.1

A partir daí o sistema **já é utilizável**.

