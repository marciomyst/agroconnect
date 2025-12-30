import { Component } from '@angular/core';
import { CarouselSlide } from '../shared/carousel-card/carousel-card.component';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  standalone: false,
  styleUrl: './authentication.component.css'
})
export class AuthenticationComponent {
  readonly carouselSlides: CarouselSlide[] = [
    {
      type: 'testimonial',
      rating: 5,
      quote:
        'O AgroConnect transformou a maneira como negociamos insumos. Encontramos os melhores precos e agilidade na entrega com apenas alguns cliques.',
      author: 'Carlos Mendes',
      authorRole: 'Produtor Rural - Mato Grosso',
      authorImage:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuBSVXBCwAbfrPXoL8VvaaQJ_G46AB-qCWN9Ib-A1AAt4LZEGZb6zHx2bZX4k7QKapIN9CovXUTOxJF8TesfMMEBIqMJbGsLn-KeO-6lDTW0F6tWh7xIgBryRAIsG_PeIyDRiGzHtjpPxDi_j7r3dVt8sB_1ocBQqGlnq9j5xcbaGV73wf-QTSu6SnevDUVgjJwfVMiqBytLu8XSV3NlqpAZEpe7I1fN4zxMgv1Vo3UhCPoyvzQLZR272IYgCnzKsyfkSCNHWOZVWmUa'
    }
  ];
}
