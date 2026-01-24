import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { filter } from 'rxjs';
import { PublicFooterComponent } from './public-footer.component';
import { PublicHeaderComponent } from './public-header.component';

@Component({
  selector: 'app-public-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, PublicFooterComponent, PublicHeaderComponent],
  template: `
    <div class="public-layout min-h-screen flex flex-col bg-background-light text-text-main">
      <app-public-header *ngIf="showChrome()"></app-public-header>
      <main class="public-content flex-1">
        <router-outlet></router-outlet>
      </main>
      <app-public-footer *ngIf="showChrome()"></app-public-footer>
    </div>
  `,
})
export class PublicLayoutComponent {
  private readonly router = inject(Router);
  private readonly currentUrl = signal(this.router.url);

  readonly showChrome = computed(() => {
    const url = this.currentUrl().split('?')[0];
    return !(url === '/login' || url.startsWith('/register'));
  });

  constructor() {
    this.router.events.pipe(
      filter((event): event is NavigationEnd => event instanceof NavigationEnd),
      takeUntilDestroyed()
    ).subscribe(event => {
      this.currentUrl.set(event.urlAfterRedirects);
    });
  }
}
