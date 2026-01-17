# Design System - AgroConnect UI

Este documento descreve os tokens, utilitarios e padroes base do frontend.

## Fundamentos

- Stack: Angular + Tailwind CSS (dark mode via `dark` class).
- Tipografia base: `font-display` (Manrope).
- Tokens vivem em `src/Agronomia.Angular/tailwind.config.js`.
- Utilitarios de componentes em `src/Agronomia.Angular/src/styles.css`.

## Tokens de cor

Use sempre classes Tailwind com tokens, evitando hex solto.

Brand
- `primary`, `primary-dark`, `primary-hover`, `text-on-primary`

Backgrounds
- `background-light`, `background-dark`

Surfaces
- `surface-light`, `surface-dark`
- `surface-light-muted`, `surface-dark-muted`, `surface-dark-soft`

Textos
- `text-main`, `text-main-light`, `text-main-dark`
- `text-muted`, `text-muted-light`, `text-placeholder`
- `text-secondary-light`, `text-secondary-dark`

Bordas
- `border-color`, `border-light`, `border-dark`, `border-strong`
- `border-muted`, `border-subtle`

Accents
- `accent-lime`, `accent-olive`

Exemplos

```html
<div class="bg-surface-light text-text-main border border-border-color">
  <p class="text-text-muted">Descricao secundaria</p>
</div>
<button class="bg-primary text-text-on-primary hover:bg-primary-hover">CTA</button>
```

## Tokens de tipografia

Font families
- `font-display`, `font-body`

Font sizes (classes Tailwind)
- `text-display-xl`, `text-display-lg`, `text-display-md`
- `text-heading-lg`, `text-heading-md`
- `text-body-lg`, `text-body-md`, `text-body-sm`
- `text-caption`

Exemplo

```html
<h1 class="text-display-lg font-black tracking-tight">Titulo principal</h1>
<p class="text-body-md text-text-muted">Texto de apoio.</p>
```

## Elevacao e raio

Raios
- `rounded-lg`, `rounded-xl`, `rounded-2xl`

Sombras
- `shadow-card`, `dark:shadow-card-dark`

## Componentes utilitarios

Definidos em `src/Agronomia.Angular/src/styles.css`.

Botoes
- Base: `btn`
- Variantes: `btn-primary`, `btn-outline`, `btn-ghost`
- Tamanhos: `btn-sm`, `btn-lg`

Inputs
- Base: `input`
- Tamanho: `input-lg`
- Icones: `input-icon-left`, `input-icon-right`

Cards
- Base: `card`
- Suave: `card-muted`

Exemplos

```html
<button class="btn btn-primary btn-lg w-full">Entrar</button>
<button class="btn btn-outline btn-sm">Secundario</button>

<input class="input input-lg" placeholder="Seu email" />
<input class="input input-lg input-icon-left" placeholder="Com icone" />

<div class="card p-6">
  <h3 class="text-heading-md font-bold">Titulo</h3>
  <p class="text-body-sm text-text-muted">Conteudo do card.</p>
</div>
```

## Guidelines

Consistencia
- Prefira tokens para cor e tipografia.
- Evite repetir cadeias longas de classes que ja existem como utilitarios.

Dark mode
- Aplique `dark:` apenas nos elementos que realmente precisam trocar.
- Use `surface-*` e `text-*` para manter contraste previsivel.

Acessibilidade
- Garanta `focus-visible` em inputs e botoes.
- Mantenha contrastes adequados em texto e CTA.

Layouts
- Para telas split, mantenha no maximo 50/50 e evite excesso de textura.
- Use `card` para conteudos destacados e `card-muted` para blocos secundarios.
