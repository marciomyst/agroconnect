# Persona: User (autenticacao e conta)

## Objetivo
- Entrar no sistema com Authentication, selecionar o contexto de Membership e manter a conta.

## Pre-requisitos
- User registrado com credenciais validas.
- Pelo menos uma Membership valida (Seller Membership ou Farm Membership).

## Jornada recomendada (MVP)
1) Acessar `/login` e autenticar com email e senha.
2) Sistema salva token e carrega o CurrentUserContext.
3) Se houver 1 Membership, seleciona automaticamente o contexto ativo.
4) Se houver varias Memberships, escolher na tela `/select-organization`.
5) Navegar para a area correspondente ao contexto (Seller ou Farm).

## Jornadas de suporte
- Trocar contexto de Membership via acao "Trocar organizacao" no AppLayout.
- Alterar senha em `/security`.

## Variacoes e excecoes
- Credenciais invalidas: manter na tela de login e exibir erro.
- Token ausente ou expirado: redirecionar para `/login`.
- Membership ausente: rotas que exigem contexto ativo devem bloquear acesso.

## Status atual (UI)
- Implementado: login, carregamento de CurrentUserContext, selecao de contexto (UI basica), guards de autenticacao e contexto, change password.
- Planejado: My Profile (`/profile`) conforme `docs/features/frontend/F2.2 - USER PROFILE.md`.

## Referencias
- `docs/features/frontend/F2.1 - LOGIN PAGE UX.md`
- `docs/features/frontend/F0.3 - ORGANIZATION CONTEXT.md`
- `docs/features/frontend/F1.2 - GUARDS AND NAVIGATION.md`
- `docs/features/frontend/F2.3 - CHANGE PASSWORD.md`

## Nota de terminologia
- Termos em uso na UI que nao aparecem na Linguagem Ubiqua: "Organization/organizacao ativa", rota `/select-organization`.
- Se quiser manter esses termos, vale atualizar `docs/UBIQUOUS_LANGUAGE.md`.
