import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PublicCarouselComponent } from '../components/public-carousel.component';

@Component({
  selector: 'app-public-landing',
  standalone: true,
  imports: [RouterLink, PublicCarouselComponent],
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
})
export class PublicLandingComponent {
  readonly categoryIds = [
    'cat-1',
    'cat-2',
    'cat-3',
    'cat-4',
    'cat-5',
    'cat-6',
    'cat-7',
    'cat-8',
    'cat-9',
    'cat-10',
    'cat-11',
  ];
}
