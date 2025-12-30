import { Component } from '@angular/core';
import { CarouselSlide } from '../shared/carousel-card/carousel-card.component';

@Component({
  selector: 'app-terms',
  templateUrl: './terms.component.html',
  standalone: false,
  styleUrl: './terms.component.css'
})
export class TermsComponent {
  readonly carouselSlides: CarouselSlide[] = [
    {
      type: 'feature',
      title: 'Transparencia e confianca',
      bullets: [
        {
          icon: 'assignment',
          text: 'Termos claros para manter as relacoes comerciais seguras.'
        },
        {
          icon: 'support_agent',
          text: 'Suporte dedicado para resolver duvidas sobre o uso da plataforma.'
        },
        {
          icon: 'update',
          text: 'Atualizacoes comunicadas de forma transparente sempre que necessario.'
        }
      ]
    }
  ];
}
