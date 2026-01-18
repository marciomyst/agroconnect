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

type ProductCard = {
  title: string;
  description: string;
  imageUrl: string;
  price: string;
  originalPrice?: string;
  badge?: {
    label: string;
    tone: 'primary' | 'danger' | 'info';
    icon?: string;
  };
  vendor: {
    name: string;
    avatar?: string;
    accent?: string;
  };
  poll?: {
    progress: number;
    currentLabel: string;
    targetLabel: string;
    timeLeft?: string;
    highlight?: string;
  };
  status?: 'soldout';
};

type FilterGroup = {
  title: string;
  options: {
    label: string;
    count?: number;
    selected?: boolean;
  }[];
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
      link: '/marketplace/categorias/defensivos'
    },
    {
      label: 'Fertilizantes',
      icon: 'water_drop',
      colors: { background: 'bg-blue-50 dark:bg-blue-900/20', text: 'text-blue-500', hoverBorder: 'group-hover:border-blue-200 dark:group-hover:border-blue-800' },
      link: '/marketplace/categorias/fertilizantes'
    },
    {
      label: 'Sementes',
      icon: 'grass',
      colors: { background: 'bg-yellow-50 dark:bg-yellow-900/20', text: 'text-yellow-600', hoverBorder: 'group-hover:border-yellow-200 dark:group-hover:border-yellow-800' },
      link: '/marketplace/categorias/sementes'
    },
    {
      label: 'Peças',
      icon: 'settings',
      colors: { background: 'bg-purple-50 dark:bg-purple-900/20', text: 'text-purple-500', hoverBorder: 'group-hover:border-purple-200 dark:group-hover:border-purple-800' },
      link: '/marketplace/categorias/pecas'
    },
    {
      label: 'Serviços',
      icon: 'engineering',
      colors: { background: 'bg-orange-50 dark:bg-orange-900/20', text: 'text-orange-500', hoverBorder: 'group-hover:border-orange-200 dark:group-hover:border-orange-800' },
      link: '/marketplace/categorias/servicos'
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

  readonly filterGroups: FilterGroup[] = [
    {
      title: 'Categorias',
      options: [
        { label: 'Defensivos (128)', selected: true },
        { label: 'Fertilizantes (85)' },
        { label: 'Sementes (42)' }
      ]
    },
    {
      title: 'Culturas',
      options: [
        { label: 'Soja', selected: true },
        { label: 'Milho' },
        { label: 'Trigo' },
        { label: 'Feijão' },
        { label: 'Algodão' }
      ]
    },
    {
      title: 'Revendedor',
      options: [
        { label: 'AgroSul' },
        { label: 'RuralTech', selected: true },
        { label: 'Vida Verde' }
      ]
    }
  ];

  readonly selectedFilters = ['Herbicidas', 'R$ 100 - R$ 500', 'RuralTech'];
  readonly sortOptions = ['Mais Relevantes', 'Menor Preço', 'Maior Preço', 'Mais Vendidos'];
  selectedSort = this.sortOptions[0];

  readonly products: ProductCard[] = [
    {
      title: 'Herbicida Glifosato 5L',
      description: 'Herbicida sistêmico não seletivo para controle de plantas daninhas anuais e perenes.',
      imageUrl:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuBh7UF1wxqQ92S2dW7LXSxY8FBN4kcLwuvLCRjwmqYNnS8SqOWj163m_GEO8goAiEbUXpyRKy5suocrsrAXPe_neA8MbWJ6gQJbEJqI-71yr__EXlHZ973U48TcQe10Rjol4QqpNisH4T0vJBDf3dEpozay4xcfAsMFD1nJ-wwluteLN8cTQSaPqW2wvaR32r0ID6Xe74L2b7BegoeZhRLzGhDytqa9X1tiQeBWVBq2bhb4aumJJlRjJhExXC8E-r-96SBWEwYvtXKJ',
      price: 'R$ 150,00',
      originalPrice: 'R$ 180,00',
      badge: { label: 'Promoção', tone: 'primary' },
      vendor: {
        name: 'AgroSul',
        avatar:
          'https://lh3.googleusercontent.com/aida-public/AB6AXuCps8ZwL0-qQHlDkWL8GAX4X-1ztD7ZPo91T25DoQ2k8TXwHBEbQ3S6iiPxhsq3hfEIgOZmWVHmw3nWhX39F0ftkXkj8sBOWfyzDkYaDvReDnRd1nz444pmgPgRIGqWmy9WjNrxZvY1IhR783xWg9_H0KMqtlkIzHjp1NV1N4oNLMhMucs0qpt8XSvMoK1RQtVtYp6-Ry8NuSpUzRcalyXq-i3Llnr3zuaNVsSgg4h0FtIAE94HDmJb_tDQqrzcAfj53xZZVkGqdL1V'
      }
    },
    {
      title: 'Fungicida Sistêmico Top (Poll)',
      description: 'Oferta coletiva ativa. Preços caem conforme o grupo progride.',
      imageUrl:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuDxGV7YRswq0oerpUq7S3aZ40pS5dw8X_lHchABAinnAtf-WTHZdbi3dp1S7zl9yXFLU_egczdyOn-_jFq1ZxT7f7QMdK5cfTd6CevalkTbEqd6rWCgPfOQP1AUIxUMBfq-s0SI6aA9B45RU1wjoW4wQu25TlqS-MSaRmBonadVG7ufH83Wxqf2RmmqoiRpSyGxjheFwBafECy4Pr4GS0y_rrBpgmdt4nrWr9XP8nyxDzeiFrc6jEc5dzoqIWwj7haUVH2XakUvyDB6',
      price: 'R$ 289,90',
      originalPrice: 'R$ 320,00',
      badge: { label: 'Poll Ativo', tone: 'info', icon: 'groups' },
      vendor: { name: 'RuralTech', accent: 'text-orange-500' },
      poll: {
        progress: 75,
        currentLabel: 'Nível 2 Desbloqueado',
        targetLabel: 'Meta (Min) R$ 245,00',
        timeLeft: '2 dias'
      }
    },
    {
      title: 'Sementes de Milho Híbrido',
      description: 'Saco de 20kg. Alta produtividade e resistência a pragas.',
      imageUrl:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuAbYyNNribLuF3LIkLtIspZs0UPaJ-_oZCHRET68w4mNLOGTXl2r81PBr0fDfrHETiVMD7Osl_mQzDbikhEF0UgHDKitBFJqO9o1VUOALrgUeih5O_ZHNAQjRhLqOvS9H5Z5UuI5xQGw2tTIR1lpUm4rpdVGe6TGB3mFaRPA4EAcxptm3zOJKEDsIAo4yFagBxB0rYvg6Id34ML4H626xCWLp4oXdCuxKkxkfR1icw8mvh8li6L-wcyX3_3Ypao2XYBHrCxziGrlZDC',
      price: 'R$ 450,00',
      vendor: { name: 'CoopCampo', accent: 'text-blue-600' }
    },
    {
      title: 'Inseticida Ultra K',
      description: 'Ação rápida contra lagartas e percevejos. Ideal para soja e milho.',
      imageUrl:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuAVkMbgfvkCYHMPuEAnrnqJHYQ-wrc_839tOapF2ayGVyLNGrQoldedq6PgynV28vEUfHksCozsfQtTRWi117rcQB6huB1NEz1Ge0-YUKk0vNsTdSAW4AFmeDVAZNH8Cnw3DdaBMLiwF2jkVme6YlQbqY2Z_nyzxQazTklGkL3ulcaEBRJ_jxicDBCaL7jHuNCmWLrY5AnNuu2lMLP3XWF0jeygddz4CZ_-Ft6LKz6V6wGOR_sKbXjZUowFpq2H_o3L-07yUWN1pXOP',
      price: 'R$ 98,50',
      vendor: { name: 'AgroSul' },
      status: 'soldout'
    },
    {
      title: 'Adubo Foliar Nitro+ (Lote Final)',
      description: 'Oferta com estoque final e contagem regressiva.',
      imageUrl:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuBlOWIxApXIppYxEwbMznWkHHlAF43LDS6MBnD51x3hzycEkuXZy0X2iAQ_mxJ9rUVXxuzFdTkiOeqbjlLXSFcywcl1JOTVDiua63RNVuyVhgRw0GeDM0YrRyjt9IOpRh9rpSK9TqVghJ_2_Q2oZzOKCyq-Cue8udcEa8T2yMIG2yIfKBAh9nYm0utm_oMuj5-LqxsCHM-whIbgEHrHIcMw3uf2wt7ybg6QChIii3x5qhHcWfyBM_GW7IOmtGxrZKd0EWFdVuDSeh9m',
      price: 'R$ 65,00',
      originalPrice: 'R$ 75,00',
      badge: { label: 'Últimas Horas', tone: 'danger', icon: 'alarm' },
      vendor: { name: 'Vida Verde', accent: 'text-green-600' },
      poll: {
        progress: 92,
        currentLabel: 'Quase lá!',
        targetLabel: 'Meta (Min) R$ 55,00',
        timeLeft: '4h restantes',
        highlight: 'Faltam: 40 L'
      }
    },
    {
      title: 'Biológico Control X',
      description: 'Defensivo biológico à base de Bacillus subtilis. Sem resíduos.',
      imageUrl:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuAbvGL0T6_0arnlM3YBSgIQSQ7bJeTwHR7ap6odjcA6w9XJYPm4hh8mgCm5rFau_Ph93d1GoPkn3FWDUJtMxclKo7-0Av9f_BuDX7RtifnQjbwxk9g4oVrZHa_fDHI2T-It1dnvT3a0M_LHpK6KkD_cwIFPsL3mMP1PqHIb4DDOW_CNANYcBC3BA72HGTAUVubdyLxJ2WaahMyAZxJN-bgU1LAY8B2hKPCR9R9Qxz_ALNWDWbG2_4lbnI_MdUN8Pz2bSqh7Ensw2DoN',
      price: 'R$ 195,00',
      originalPrice: 'R$ 210,00',
      vendor: { name: 'RuralTech' }
    }
  ];

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.isAuthenticated = this.authService.hasActiveSession();
  }

  onSearch(): void {
    this.router.navigate(['/marketplace/products']);
  }

  goToLogin(): void {
    this.router.navigate(['/authentication']);
  }
}
