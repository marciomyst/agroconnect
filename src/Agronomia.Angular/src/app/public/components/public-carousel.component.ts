import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-public-carousel',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="relative">
      <div
        *ngIf="showFades"
        class="pointer-events-none absolute inset-y-0 left-0 w-10 bg-gradient-to-r from-background-light via-background-light/70 to-transparent"
      ></div>
      <div
        *ngIf="showFades"
        class="pointer-events-none absolute inset-y-0 right-0 w-10 bg-gradient-to-l from-background-light via-background-light/70 to-transparent"
      ></div>
      <a
        *ngIf="showArrows && itemIds.length"
        class="absolute left-1 top-1/2 -translate-y-1/2 z-10 w-9 h-9 rounded-full bg-surface-light border border-border-color text-text-muted hover:text-primary hover:border-primary transition-colors flex items-center justify-center shadow-card"
        [attr.href]="'#' + itemIds[0]"
        [attr.aria-label]="startLabel"
      >
        <span class="material-symbols-outlined text-xl">chevron_left</span>
      </a>
      <a
        *ngIf="showArrows && itemIds.length"
        class="absolute right-1 top-1/2 -translate-y-1/2 z-10 w-9 h-9 rounded-full bg-surface-light border border-border-color text-text-muted hover:text-primary hover:border-primary transition-colors flex items-center justify-center shadow-card"
        [attr.href]="'#' + itemIds[itemIds.length - 1]"
        [attr.aria-label]="endLabel"
      >
        <span class="material-symbols-outlined text-xl">chevron_right</span>
      </a>
      <div
        class="flex w-full justify-start md:justify-center gap-4 overflow-x-auto no-scrollbar pb-2 snap-x snap-mandatory scroll-smooth scroll-px-6"
        [attr.aria-label]="ariaLabel"
      >
        <ng-content></ng-content>
      </div>
      <div *ngIf="showDots && itemIds.length" class="mt-4 flex flex-wrap justify-center gap-2 text-text-muted">
        <a
          *ngFor="let id of itemIds; let index = index"
          class="w-2.5 h-2.5 rounded-full bg-border-color hover:bg-primary/60 transition-colors"
          [attr.href]="'#' + id"
          [attr.aria-label]="dotLabelPrefix + ' ' + (index + 1)"
        ></a>
      </div>
    </div>
  `,
  styles: [
    `
      .no-scrollbar::-webkit-scrollbar {
        display: none;
      }
      .no-scrollbar {
        -ms-overflow-style: none;
        scrollbar-width: none;
      }
    `,
  ],
})
export class PublicCarouselComponent {
  @Input() itemIds: string[] = [];
  @Input() ariaLabel = 'Carrossel';
  @Input() dotLabelPrefix = 'Item';
  @Input() startLabel = 'Voltar ao inicio do carrossel';
  @Input() endLabel = 'Ir para o fim do carrossel';
  @Input() showArrows = true;
  @Input() showDots = true;
  @Input() showFades = true;
}
