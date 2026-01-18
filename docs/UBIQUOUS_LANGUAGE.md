# Linguagem Ubíqua — AgroConnect

Este documento define a **linguagem ubíqua oficial** do sistema AgroConnect.

Ele deve ser utilizado como **referência única** para:

* nomes de classes, métodos, eventos e comandos
* comunicação entre time técnico e stakeholders
* decisões de arquitetura e modelagem de domínio

> **Regra de ouro:** se um termo não estiver neste documento, ele **não deve aparecer no código** sem discussão prévia.

---

## 1. Princípios da Linguagem Ubíqua

1. Um conceito do negócio → **um único nome no código**
2. O mesmo termo vale para:

   * domínio
   * banco de dados
   * APIs
   * eventos
3. Não usamos sinônimos técnicos para o mesmo conceito
4. O nome reflete **o significado do negócio**, não a implementação

---

## 2. Atores Principais

### User

Pessoa física que acessa o sistema.

* Representa **identidade digital**
* Pode atuar em nome de diferentes organizações
* Nunca representa a organização em si

**Exemplos de uso:**

* `User`
* `UserId`
* `UserRegistered`

---

## 3. Organizações

### Seller (Revenda)

Entidade jurídica que comercializa produtos agrícolas.

* Possui CNPJ / TaxId
* Publica produtos
* Cria campanhas de venda coletiva

**Nunca**:

* autentica
* possui senha
* toma decisões de acesso

**Exemplos:**

* `Seller`
* `SellerRegistered`
* `RegisterSeller`

---

### Farm (Produtor Rural)

Entidade jurídica ou física que compra produtos.

* Possui CNPJ ou CPF
* Realiza pedidos
* Recebe cashback

**Exemplos:**

* `Farm`
* `RegisterFarm`
* `FarmRegistered`

---

## 4. Vínculos e Papéis

### Membership

Representa o **vínculo entre um User e uma organização**.

* Sempre contextual (User ↔ Seller ou User ↔ Farm)
* Define papéis e permissões

**Exemplos:**

* `SellerMembership`
* `FarmMembership`

---

### Role

Define o papel de um User dentro de uma organização.

Papéis iniciais:

| Contexto | Papel   | Significado                     |
| -------- | ------- | ------------------------------- |
| Seller   | Owner   | Responsável máximo pela revenda |
| Seller   | Manager | Gerencia operações              |
| Seller   | Viewer  | Apenas leitura                  |
| Farm     | Owner   | Responsável pela fazenda        |
| Farm     | Buyer   | Pode realizar pedidos           |

**Exemplos:**

* `SellerRole.Owner`
* `FarmRole.Buyer`

---

## 5. Autenticação e Segurança

### Authentication

Processo de validar credenciais de um User.

* Baseado em email + senha
* Controlado pela própria aplicação

---

### Authorization

Processo de validar se um User **pode executar uma ação**.

* Sempre depende de:

  * Membership
  * Role

**Nunca** baseado apenas no User.

---

### JWT (Json Web Token)

Token de autenticação emitido pela aplicação.

* Contém apenas identidade
* Não carrega papéis de domínio

---

## 6. Catálogo e Produtos

### Product

Item comercializado por um Seller.

* Pertence a um Seller
* Possui preço base

**Exemplos:**

* `Product`
* `ProductPublished`

---

## 7. Pedidos

### Order

Representa uma compra realizada por uma Farm.

* Criado por um User
* Sempre pertence a uma Farm e a um Seller

**Exemplos:**

* `Order`
* `OrderPlaced`
* `OrderConfirmed`

---

## 8. Venda Coletiva

### CollectiveDeal

Campanha de venda coletiva criada por um Seller.

* Possui período de validade
* Define faixas de volume

---

### Tier

Faixa de volume dentro de um CollectiveDeal.

* Determina benefício progressivo

---

## 9. Financeiro

### FarmWallet

Carteira financeira de uma Farm.

* Recebe cashback
* Pode ser usada como forma de pagamento

---

### Cashback

Crédito financeiro gerado a partir de uma venda coletiva.

* Nunca altera pedidos retroativamente
* Sempre vira saldo na FarmWallet

---

## 10. Eventos de Domínio (padrão de nomenclatura)

Todos os eventos devem:

* Estar no passado
* Representar algo que **já aconteceu**

**Exemplos válidos:**

* `UserRegistered`
* `SellerRegistered`
* `SellerMembershipGranted`
* `FarmWalletCredited`

**Exemplos inválidos:**

* `RegisterSeller`
* `CreateUser`

---

## 11. Comandos (padrão de nomenclatura)

Comandos sempre representam **intenção**.

**Exemplos:**

* `RegisterSellerWithOwner`
* `GrantSellerMembership`
* `PlaceOrder`

---

## 12. Decisões explícitas (não-negociáveis)

* ❌ Não existe "usuário da revenda"
* ❌ Não existe "perfil fixo"
* ❌ Não existe desconto direto em pedido por pool
* ✅ Cashback sempre via FarmWallet
* ✅ Membership é conceito de domínio
* ✅ Eventos fazem parte do domínio

---

## 13. Governança

Este documento:

* deve evoluir junto com o domínio
* **não pode ser alterado sem revisão arquitetural**

Qualquer nova feature deve:

1. Referenciar este documento
2. Validar se novos termos são realmente necessários
3. Atualizar a linguagem ubíqua **antes do código**

---

**Este documento é parte do código.**
