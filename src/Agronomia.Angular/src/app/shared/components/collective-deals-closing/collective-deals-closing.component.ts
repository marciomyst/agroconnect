import { Component } from '@angular/core';

@Component({
  selector: 'app-collective-deals-closing',
  standalone: true,
  template: `
    <section class="space-y-4">
      <div class="flex flex-wrap items-center justify-between gap-3">
        <h2 class="text-heading-md font-bold text-text-main dark:text-text-main-dark flex items-center gap-2">
          <span class="material-symbols-outlined text-text-secondary-light dark:text-text-secondary-dark">timer</span>
          CollectiveDeals closing soon
        </h2>
        <button type="button" class="btn btn-ghost btn-sm">View all</button>
      </div>
      <div class="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
        <article class="card p-5 flex flex-col gap-4 dash-reveal" style="--dash-delay: 420ms;">
          <div class="flex items-start justify-between gap-3">
            <div>
              <p class="text-body-lg font-bold text-text-main dark:text-text-main-dark">Herbicida Select 20L</p>
              <p class="text-body-sm text-text-muted dark:text-text-muted-light">Tier target: 1000L</p>
            </div>
            <span class="rounded-full bg-primary/10 text-primary-dark px-3 py-1 text-caption font-semibold">02h 14m</span>
          </div>
          <div class="space-y-2">
            <div class="flex items-center justify-between text-body-sm font-semibold text-text-muted dark:text-text-muted-light">
              <span>Filled</span>
              <span class="text-text-secondary-light dark:text-text-secondary-dark">84%</span>
            </div>
            <div class="h-2 rounded-full bg-border-light dark:bg-border-dark">
              <div class="h-2 rounded-full bg-primary" style="width: 84%"></div>
            </div>
          </div>
          <div class="flex items-center justify-between">
            <div>
              <p class="text-caption text-text-muted dark:text-text-muted-light line-through">BRL 450,00</p>
              <p class="text-body-lg font-bold text-text-main dark:text-text-main-dark">BRL 380,00</p>
            </div>
            <button type="button" class="btn btn-outline btn-sm">View</button>
          </div>
        </article>

        <article class="card p-5 flex flex-col gap-4 dash-reveal" style="--dash-delay: 480ms;">
          <div class="flex items-start justify-between gap-3">
            <div>
              <p class="text-body-lg font-bold text-text-main dark:text-text-main-dark">Fertilizante Foliar Potassio</p>
              <p class="text-body-sm text-text-muted dark:text-text-muted-light">Tier target: 500Kg</p>
            </div>
            <span class="rounded-full bg-primary/10 text-primary-dark px-3 py-1 text-caption font-semibold">05h 30m</span>
          </div>
          <div class="space-y-2">
            <div class="flex items-center justify-between text-body-sm font-semibold text-text-muted dark:text-text-muted-light">
              <span>Filled</span>
              <span class="text-text-secondary-light dark:text-text-secondary-dark">62%</span>
            </div>
            <div class="h-2 rounded-full bg-border-light dark:bg-border-dark">
              <div class="h-2 rounded-full bg-primary" style="width: 62%"></div>
            </div>
          </div>
          <div class="flex items-center justify-between">
            <div>
              <p class="text-caption text-text-muted dark:text-text-muted-light line-through">BRL 120,00</p>
              <p class="text-body-lg font-bold text-text-main dark:text-text-main-dark">BRL 95,00</p>
            </div>
            <button type="button" class="btn btn-outline btn-sm">View</button>
          </div>
        </article>

        <article class="card p-5 flex flex-col gap-4 dash-reveal" style="--dash-delay: 540ms;">
          <div class="flex items-start justify-between gap-3">
            <div>
              <p class="text-body-lg font-bold text-text-main dark:text-text-main-dark">Semente Milho Hibrido</p>
              <p class="text-body-sm text-text-muted dark:text-text-muted-light">Tier target: 200 Sacas</p>
            </div>
            <span class="rounded-full bg-primary/10 text-primary-dark px-3 py-1 text-caption font-semibold">1 day</span>
          </div>
          <div class="space-y-2">
            <div class="flex items-center justify-between text-body-sm font-semibold text-text-muted dark:text-text-muted-light">
              <span>Filled</span>
              <span class="text-text-secondary-light dark:text-text-secondary-dark">46%</span>
            </div>
            <div class="h-2 rounded-full bg-border-light dark:bg-border-dark">
              <div class="h-2 rounded-full bg-primary" style="width: 46%"></div>
            </div>
          </div>
          <div class="flex items-center justify-between">
            <div>
              <p class="text-caption text-text-muted dark:text-text-muted-light line-through">BRL 600,00</p>
              <p class="text-body-lg font-bold text-text-main dark:text-text-main-dark">BRL 540,00</p>
            </div>
            <button type="button" class="btn btn-outline btn-sm">View</button>
          </div>
        </article>
      </div>
    </section>
  `,
  styles: [
    `
      .dash-reveal {
        opacity: 0;
        transform: translateY(12px);
        animation: dash-rise 0.6s ease forwards;
        animation-delay: var(--dash-delay, 0ms);
      }

      @keyframes dash-rise {
        to {
          opacity: 1;
          transform: translateY(0);
        }
      }

      @media (prefers-reduced-motion: reduce) {
        .dash-reveal {
          animation: none;
          opacity: 1;
          transform: none;
        }
      }
    `,
  ],
})
export class CollectiveDealsClosingComponent {}
