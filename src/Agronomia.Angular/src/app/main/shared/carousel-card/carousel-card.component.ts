import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';

export interface CarouselBullet {
  icon: string;
  text: string;
}

export interface CarouselSlide {
  id?: string;
  type?: 'testimonial' | 'feature';
  title?: string;
  bullets?: CarouselBullet[];
  quote?: string;
  rating?: number;
  author?: string;
  authorRole?: string;
  authorImage?: string;
}

@Component({
  selector: 'app-carousel-card',
  templateUrl: './carousel-card.component.html',
  standalone: true,
  imports: [CommonModule],
  styleUrl: './carousel-card.component.css'
})
export class CarouselCardComponent implements OnInit, OnChanges, OnDestroy {
  @Input() slides: CarouselSlide[] = [];
  @Input() autoRotate = true;
  @Input() rotationMs = 8000;
  @Input() cardClass = '';
  @Input() indicatorClass = 'mt-10';
  @Input() showIndicators = true;

  activeIndex = 0;
  private rotationId: number | null = null;

  get activeSlide(): CarouselSlide | null {
    return this.slides[this.activeIndex] ?? null;
  }

  get hasMultipleSlides(): boolean {
    return this.slides.length > 1;
  }

  ngOnInit(): void {
    this.startRotation();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['slides']) {
      if (this.activeIndex >= this.slides.length) {
        this.activeIndex = 0;
      }
    }

    if (changes['slides'] || changes['autoRotate'] || changes['rotationMs']) {
      this.resetRotation();
    }
  }

  ngOnDestroy(): void {
    this.stopRotation();
  }

  nextSlide(): void {
    if (!this.slides.length) {
      return;
    }

    this.activeIndex = (this.activeIndex + 1) % this.slides.length;
  }

  selectSlide(index: number): void {
    if (!this.slides.length) {
      return;
    }

    this.activeIndex = index;
    this.resetRotation();
  }

  isTestimonial(slide: CarouselSlide | null): boolean {
    if (!slide) {
      return false;
    }

    return slide.type === 'testimonial' || (!!slide.quote && !!slide.author);
  }

  getStars(count?: number): number[] {
    const total = Math.max(0, count ?? 0);
    return Array.from({ length: total }, (_, index) => index);
  }

  trackBySlide = (_index: number, slide: CarouselSlide): string => {
    return slide.id ?? slide.title ?? slide.author ?? String(_index);
  };

  trackByBullet = (_index: number, bullet: CarouselBullet): string => {
    return `${bullet.icon}-${bullet.text}`;
  };

  private startRotation(): void {
    if (!this.autoRotate || !this.hasMultipleSlides) {
      return;
    }

    this.stopRotation();
    this.rotationId = window.setInterval(() => {
      this.nextSlide();
    }, this.rotationMs);
  }

  private stopRotation(): void {
    if (this.rotationId === null) {
      return;
    }

    window.clearInterval(this.rotationId);
    this.rotationId = null;
  }

  private resetRotation(): void {
    this.stopRotation();
    this.startRotation();
  }
}
