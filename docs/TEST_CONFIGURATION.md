Aqui está uma proposta de documentação estruturada, pronta para ser anexada ao `README.md` do projeto, wiki ou enviada para a equipe de QA/Testers.

Organizei o conteúdo para separar claramente a **tentativa de login** do **fluxo de cadastro** (fallback), facilitando o "copiar e colar" dos dados.

---

# 🚀 Bootstrap de Testes: Cadastro e Login Básico

Este documento descreve os passos para validar o fluxo de autenticação e realizar o *seed* inicial de usuários (Farmer e Seller) no ambiente de testes.

## 1. Credenciais Padrão (Login)

Tente realizar o acesso com as contas abaixo. Caso receba a mensagem **"Usuário ou senha inválidos"**, prossiga para a seção **[2. Fluxo de Cadastro](https://www.google.com/search?q=%232-fluxo-de-cadastro)**.

| Perfil | Email | Senha |
| --- | --- | --- |
| **Farmer (Produtor)** | `farmer@agroconnect.com.br` | `123456789` |
| **Seller (Vendedor)** | `seller@agroconnect.com.br` | `123456789` |

---

## 2. Fluxo de Cadastro

Caso as credenciais acima não existam, inicie o cadastro através da opção **"Cadastrar-se gratuitamente"**.

### 🚜 Cenário A: Cadastro de Farmer (Produtor Rural)

1. Na seleção de perfil, clique em: **Sou Produtor Rural**
2. Preencha os dados básicos:
* **Nome:** `Produtor Rural`
* **Email:** `farmer@agroconnect.com.br`
* **Senha:** `123456789`
* **Confirmar Senha:** `123456789`


3. Clique em **Continuar Cadastro**.
4. Preencha os dados complementares:
* **TaxId (CPF/CNPJ):** `0000000001-90`
* **Nome da Propriedade:** `Propriedade 01`


5. Clique em **Finalizar cadastro**.

---

### 💼 Cenário B: Cadastro de Seller (Vendedor)

1. Na seleção de perfil, clique em: **Sou Vendedor**
2. Preencha os dados básicos:
* **Nome:** `Vendedor`
* **Email:** `seller@agroconnect.com.br`
* **Senha:** `123456789`
* **Confirmar Senha:** `123456789`


3. Clique em **Continuar Cadastro**.
4. Preencha os dados complementares:
* **TaxId (CPF/CNPJ):** `0000000001-90`
* **Razão Social:** `Propriedade 01`


5. Clique em **Finalizar cadastro**.