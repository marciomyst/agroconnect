import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-app-layout',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <div class="app-layout">
      <header class="app-header">
        <div class="app-header-title">Authenticated header</div>
        <div class="app-org-selector">Organization selector (placeholder)</div>
      </header>
      <div class="app-shell">
        <nav class="app-nav">Side menu</nav>
        <main class="app-content">
          <router-outlet></router-outlet>
        </main>
      </div>
    </div>
  `,
})
export class AppLayoutComponent {}
