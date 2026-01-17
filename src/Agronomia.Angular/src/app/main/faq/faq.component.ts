import { Component } from '@angular/core';
import { CarouselSlide } from '../shared/carousel-card/carousel-card.component';

@Component({
  selector: 'app-faq',
  templateUrl: './faq.component.html',
  standalone: false,
  styleUrl: './faq.component.css'
})
export class FaqComponent {
  readonly carouselSlides: CarouselSlide[] = [
    {
      type: 'feature',
      title: 'Suporte que acompanha voce',
      bullets: [
        {
          icon: 'forum',
          text: 'Respostas rapidas para as duvidas mais comuns.'
        },
        {
          icon: 'track_changes',
          text: 'Acompanhe pedidos e negociacoes em tempo real.'
        },
        {
          icon: 'verified_user',
          text: 'Informacoes verificadas para manter sua operacao segura.'
        }
      ]
    }
  ];
}
