# Persona: User com Farm Membership

## Objetivo
- Navegar no marketplace, comparar ofertas e registrar Purchase Intent para uma oferta.
- Acompanhar Purchase Intent existentes.

## Pre-requisitos
- User autenticado.
- Farm Membership ativa (contexto Farm).

## Jornada recomendada (MVP)
1) Entrar no contexto Farm (selecionar Membership do tipo Farm).
2) Acessar `/farmer/marketplace` para listar produtos.
3) Abrir detalhes em `/farmer/marketplace/products/:productId`.
4) Selecionar oferta e criar Purchase Intent em `/farmer/marketplace/products/:productId/intent` (com `sellerProductId`).
5) Acompanhar lista em `/farmer/purchase-intents`.

## Jornadas de suporte
- Trocar contexto de Membership.
- Alterar senha em `/security`.

## Variacoes e excecoes
- Produto ou oferta indisponivel: exibir estado vazio/erro.
- API de Purchase Intent falha: manter usuario na tela e exibir erro.

## Status atual (UI)
- Implementado: marketplace list/detail, criacao e listagem de Purchase Intent.
- Nao implementado: dashboard `/farmer`, Members `/farmer/members`.

## Referencias
- `docs/features/frontend/F3.6 - PURCHASE INTENT UI.md` (secao Marketplace e Purchase Intent)
- `docs/features/frontend/F3.3 - MARKETPLACE PRODUCT DETAIL AND ORDER.md`
- `docs/features/frontend/F3.2 - MARKETPLACE PRODUCT LISTING.md`
- `docs/features/frontend/F3.4 - PURCHASE INTENT.md`

## Nota de terminologia
- Termos em uso na UI que nao aparecem na Linguagem Ubiqua: "Marketplace", "Purchase Intent", "Organization/organizacao ativa", rota `/farmer`.
- Se quiser manter esses termos, vale atualizar `docs/UBIQUOUS_LANGUAGE.md`.
