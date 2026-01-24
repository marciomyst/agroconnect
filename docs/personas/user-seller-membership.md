# Persona: User com Seller Membership

## Objetivo
- Gerenciar o catalogo de produtos do Seller ativo e manter a conta.

## Pre-requisitos
- User autenticado.
- Seller Membership ativa (contexto Seller).

## Jornada recomendada (MVP)
1) Entrar no contexto Seller (selecionar Membership do tipo Seller).
2) Acessar `/seller/catalog`.
3) Listar itens do catalogo, usar busca e paginacao.
4) Criar item: `/seller/catalog/new` -> selecionar Product -> definir preco e disponibilidade -> salvar.
5) Editar item: `/seller/catalog/:sellerProductId/edit` -> atualizar preco ou disponibilidade.

## Jornadas de suporte
- Trocar contexto de Membership.
- Alterar senha em `/security`.

## Variacoes e excecoes
- Produto duplicado no catalogo: exibir erro (409) e manter formulario editavel.
- Seller Membership ausente: guard bloqueia rota e redireciona para selecao de contexto.

## Status atual (UI)
- Implementado: listagem, criacao e edicao do Seller Catalog; busca de Product via API.
- Nao implementado: dashboard `/seller`, Members `/seller/members`.

## Referencias
- `docs/features/frontend/F3.6 - PURCHASE INTENT UI.md` (secao Seller Catalog)
- `docs/features/frontend/F3.1 - SELLER CATALOG MANAGEMENT.md`
- `docs/features/frontend/F0.3 - ORGANIZATION CONTEXT.md`

## Nota de terminologia
- Termos em uso na UI que nao aparecem na Linguagem Ubiqua: "Catalog", "Organization/organizacao ativa", rota `/seller`.
- Se quiser manter esses termos, vale atualizar `docs/UBIQUOUS_LANGUAGE.md`.
