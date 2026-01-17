import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../authentication/auth.service';

type CategoryCard = {
  label: string;
  icon: string;
  colors: {
    background: string;
    text: string;
    hoverBorder: string;
  };
  link: string;
};

type BannerCard = {
  title: string;
  subtitle: string;
  cta: string;
  variant: 'primary' | 'dark';
  imageUrl?: string;
};

type PartnerStore = {
  name: string;
  icon: string;
  accent: string;
  heroImage: string;
  rating: string;
  tags: string;
  badges: string[];
  delivery?: string;
};

type AudienceCard = {
  title: string;
  description: string;
  cta: string;
  icon: string;
  accent: string;
  requiresAuth?: boolean;
};

@Component({
  selector: 'app-marketplace',
  templateUrl: './marketplace.component.html',
  standalone: false,
  styleUrl: './marketplace.component.css'
})
export class MarketplaceComponent implements OnInit {
  isAuthenticated = false;

  readonly categories: CategoryCard[] = [
    {
      label: 'Defensivos',
      icon: 'pest_control',
      colors: { background: 'bg-red-50 dark:bg-red-900/20', text: 'text-red-500', hoverBorder: 'group-hover:border-red-200 dark:group-hover:border-red-800' },
      link: '/platform/marketplace/categorias/defensivos'
    },
    {
      label: 'Fertilizantes',
      icon: 'water_drop',
      colors: { background: 'bg-blue-50 dark:bg-blue-900/20', text: 'text-blue-500', hoverBorder: 'group-hover:border-blue-200 dark:group-hover:border-blue-800' },
      link: '/platform/marketplace/categorias/fertilizantes'
    },
    {
      label: 'Sementes',
      icon: 'grass',
      colors: { background: 'bg-yellow-50 dark:bg-yellow-900/20', text: 'text-yellow-600', hoverBorder: 'group-hover:border-yellow-200 dark:group-hover:border-yellow-800' },
      link: '/platform/marketplace/categorias/sementes'
    },
    {
      label: 'Peças',
      icon: 'settings',
      colors: { background: 'bg-purple-50 dark:bg-purple-900/20', text: 'text-purple-500', hoverBorder: 'group-hover:border-purple-200 dark:group-hover:border-purple-800' },
      link: '/platform/marketplace/categorias/pecas'
    },
    {
      label: 'Serviços',
      icon: 'engineering',
      colors: { background: 'bg-orange-50 dark:bg-orange-900/20', text: 'text-orange-500', hoverBorder: 'group-hover:border-orange-200 dark:group-hover:border-orange-800' },
      link: '/platform/marketplace/categorias/servicos'
    }
  ];

  readonly banners: BannerCard[] = [
    {
      title: 'Herbicidas com até 30% OFF',
      subtitle: 'Oferta Relâmpago',
      cta: 'Ver Ofertas',
      variant: 'primary',
      imageUrl:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuCVVHlai8oPIaUXSq8HYOX2kwm4lPElp6T-LMKzRzU20IPe8TXDza6hJVZPOcGJttw5Fgv9WtEgsAfvWT7Qc2ZdM-QWCo4WIvn_ju6uswPWYColnqUavWxj_h8eMNZWLeDsqBmjOsmA9fa9Ih40jT9asKPoBlgrJBO-JoWUet30AN0aW78G63ZjKZCzcBc5ODJtZ5R4wcR6cCQBF3EdNannQtgcBzs9BNQPQrdQfkvhXtQju2mAZ2Ha6zeu8zgXfBuQRdqpcwHH5y5e'
    },
    {
      title: 'Frete Grátis na primeira compra',
      subtitle: 'Novidade',
      cta: 'Aproveitar',
      variant: 'dark'
    }
  ];

  readonly partnerStores: PartnerStore[] = [
    {
      name: 'AgroVida Insumos',
      icon: 'spa',
      accent: 'text-green-700',
      heroImage:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuBm5BZCC94E79MTkVb2mQz0RpTEBTgC9ng0lYgESgXnhJSkg9vRfGabZVXxzXekRD6j9ULqSUKWtY-CDVcunx0ltUwMT4cRwm7WjDHMwICALmN9oOHA9_bBcK_E7tbeZlItrwREHYonD6-BjTCqeYQgCh55lzu8ZkBpalcaL07Zsjya08oNwWwoCmqSC6NItfpepHsSvLRYhyXQIEKz9cOciNnwVo92mmTfRBdfkf8OJMdSl7RRNS9-V-lMgJB3-YxgVIvHBnFrnldL',
      rating: '4.8',
      tags: 'Defensivos • 2.5km',
      badges: ['Entrega Grátis']
    },
    {
      name: 'IrrigaTech Solutions',
      icon: 'water_drop',
      accent: 'text-blue-600',
      heroImage:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuCVVHlai8oPIaUXSq8HYOX2kwm4lPElp6T-LMKzRzU20IPe8TXDza6hJVZPOcGJttw5Fgv9WtEgsAfvWT7Qc2ZdM-QWCo4WIvn_ju6uswPWYColnqUavWxj_h8eMNZWLeDsqBmjOsmA9fa9Ih40jT9asKPoBlgrJBO-JoWUet30AN0aW78G63ZjKZCzcBc5ODJtZ5R4wcR6cCQBF3EdNannQtgcBzs9BNQPQrdQfkvhXtQju2mAZ2Ha6zeu8zgXfBuQRdqpcwHH5y5e',
      rating: '4.9',
      tags: 'Equipamentos • 5km',
      badges: ['30-45 min'],
      delivery: 'rapida'
    },
    {
      name: 'Rural Peças & Serviços',
      icon: 'agriculture',
      accent: 'text-orange-600',
      heroImage:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuClCNv4hK4oGtAji8_88JKMn7JYnluyrgS1EZLgCo1RhAy6zLm47hru-KbYetsUqmCfZf7C5GtpjE-4zEetYB8DPFiorDEe76VmkgOPmpLeVVVAR-RPqHw_cKzCj1p45lkcMIbyzaGz2YcI6Giz6h8xHIgUibFatevpoRy2iNFXQFlMylw_8Sa-dwtGstOWqIm-qnoeBssy-fjzRMpj6EujSGYk2cjvc_BHDizVetmjbPPSFy0C4rUV_rJV7a3q44fSWZX5pO2y3LTw',
      rating: '4.5',
      tags: 'Peças • 12km',
      badges: ['Cupom R$20']
    },
    {
      name: 'BioTech Sementes',
      icon: 'science',
      accent: 'text-purple-600',
      heroImage: '',
      rating: '5.0',
      tags: 'Sementes • 8km',
      badges: ['Entrega Hoje']
    }
  ];

  readonly audiences: AudienceCard[] = [
    {
      title: 'Para Agricultores',
      description: 'Compre defensivos e insumos com o melhor preço da região. Compare ofertas, pague com segurança e receba na fazenda.',
      cta: 'Começar a Comprar',
      icon: 'spa',
      accent: 'primary'
    },
    {
      title: 'Para Revendedores',
      description: 'Cadastre sua loja e venda para milhares de produtores. Gerencie estoque, receba pedidos online e amplie seu faturamento.',
      cta: 'Cadastrar Minha Loja',
      icon: 'storefront',
      accent: 'secondary',
      requiresAuth: true
    }
  ];

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.isAuthenticated = this.authService.hasActiveSession();
  }

  goToLogin(): void {
    this.router.navigate(['/authentication']);
  }
}
