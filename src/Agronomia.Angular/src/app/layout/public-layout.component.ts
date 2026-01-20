import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-public-layout',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <div class="public-layout">
      <header class="public-header">Public header</header>
      <main class="public-content">
        <router-outlet></router-outlet>
      </main>
      <footer class="public-footer">Public footer</footer>
    </div>
  `,
})
export class PublicLayoutComponent {}
