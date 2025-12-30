import { Component } from '@angular/core';
import { CarouselSlide } from '../shared/carousel-card/carousel-card.component';

@Component({
  selector: 'app-privacy',
  templateUrl: './privacy.component.html',
  standalone: false,
  styleUrl: './privacy.component.css'
})
export class PrivacyComponent {
  readonly carouselSlides: CarouselSlide[] = [
    {
      type: 'feature',
      title: 'Compromissos com seus dados',
      bullets: [
        {
          icon: 'encrypted',
          text: 'Criptografia em transito e em repouso para proteger suas informacoes.'
        },
        {
          icon: 'search',
          text: 'Auditorias e monitoramento continuo para prevenir incidentes.'
        },
        {
          icon: 'policy',
          text: 'Transparencia total sobre como usamos e compartilhamos dados.'
        }
      ]
    }
  ];
}
