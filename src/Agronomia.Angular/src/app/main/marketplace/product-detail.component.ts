import { Component, OnInit } from '@angular/core';
import { AuthService } from '../authentication/auth.service';

type ProductImage = {
  alt: string;
  src: string;
};

type PrescriptionOption = {
  id: string;
  label: string;
};

type Specification = {
  label: string;
  value: string;
  tone?: 'warning';
};

type Review = {
  name: string;
  subtitle: string;
  initials: string;
  color: string;
  stars: Array<'full' | 'half' | 'empty'>;
  timeAgo: string;
  text: string;
};

type RelatedProduct = {
  brand: string;
  name: string;
  price: string;
  originalPrice?: string;
  rating: string;
  imageUrl: string;
  discount?: string;
};

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  standalone: false,
  styleUrl: './product-detail.component.css'
})
export class ProductDetailComponent implements OnInit {
  isAuthenticated = false;

  readonly breadcrumbs = [
    { label: 'Home', link: '/marketplace' },
    { label: 'Defensivos', link: '/marketplace/products' },
    { label: 'Herbicidas', link: '/marketplace/products' },
    { label: 'Glyphosate 480 SL' }
  ];

  readonly gallery: ProductImage[] = [
    {
      alt: 'Close up of a large white plastic jerrycan of herbicide',
      src: 'https://lh3.googleusercontent.com/aida-public/AB6AXuB-3qLiYp6d30tPXJIpiaLUSBAgMY4LnJtrdhjp2dhaDCrkWB2IFkI7X5ZXU7NcCy_1u8ysm6tuZuMMznYw9dd3FPTtX2IEGjGOPa2DDQsKJdhVsO5ajfmwkd6tl7DcciKvFzcJ2brNA64TfXSKlXsILgQSAhTgR_f0QvGw5jDkPXYgIG_skgsivRQdcQxXDgFVZyOLo4BgTH58u3m0VSrj-Ja_zKBnerJVP3l_knPTDUw4qYbV0jbcimWfyh11o4gXMsLW7kDd7vFl'
    },
    {
      alt: 'Main view of herbicide container',
      src: 'https://lh3.googleusercontent.com/aida-public/AB6AXuDY30FoI4DNz_D1HN4C3GSrZnk8XllTKLDnEDPKd1KwXXIFz42H_ywX09rTEjtQ3F7XYpjdQ0e3Qg1ZLTBIwvne4raA6Kxz_WukLYnKZalZPp7tP4hXLASd1jqS9wgWK47Iwvp-qD_CWvRjFzxnm1OtGwVHVuaKhPdQmKs0cYlgZhR3rVU9rTqI8XHbb8VyN9OB6_66C-iY-cDMVJm0spMdasHXlmUnjAeicAhl7O6_wdN7eXLroWdNHh2UBTcmir8Ftv5UEd-hwVXR'
    },
    {
      alt: 'Label detail showing ingredients list',
      src: 'https://lh3.googleusercontent.com/aida-public/AB6AXuDrthuqsFKg9IS7-_nhORCGTe-4qsVcDsplZbgDt17itSL7rY4COJ4VslEguw6MEn-HmdqJDWfKNKSLmHEptCCg2Hu4fjUS6ShyjLvnXlkWMaOF8vUQ-3Y9YAN18I4ktlhy6Msm4MV39Nl_x4jFSCgQseJ25zQolNtOglvJ6evmrfuRiRnz9MjX7mVqVT9dbC67GsPP0rl7ADGNVUN4gfVxwp49YzjaH8hayqAlkFcOegS2WkKfT9rTxpBSb0DDIo0n1VmmsFrpqGg7'
    },
    {
      alt: 'Farmer inspecting crop in field',
      src: 'https://lh3.googleusercontent.com/aida-public/AB6AXuAjn0z9X4cgTZ0GFEXvBaq9nlftFpiDTpEUJCwKP-ko5iEAT8nF0HFwzzCOk_wJmGrKBlTu8Dvrf8OT61ZO18OLYEFdF28QLfn8C6wmHWR6DSZBGDbt3az_09grzc9siRFULBaq61372o0DBIIZyQryWRZhb_s2erwPu7WtbYj7XLs2pFYJ8ij4Bd9B-pi3jIooVxiZC9fWifmeuZXYPwsi2l0I4ut93JdadwyQwhZeDfAozirqRwID4_g5W-s7vFyPBJPuqfrQ4uwC'
    }
  ];

  activeImage: ProductImage = this.gallery[0];

  readonly product = {
    brand: 'Monsanto',
    name: 'Herbicida Glyphosate 480 SL - 20 Litros',
    description: 'Herbicida sistêmico não seletivo para controle de plantas daninhas anuais e perenes. Ideal para pré-plantio e pós-emergência.',
    originalPrice: 'R$ 520,00',
    price: 'R$ 450,00',
    unitLabel: '/ Unidade',
    paymentInfo: '12x de R$ 41,50 sem juros',
    rating: 4.8,
    reviewCount: 124,
    badge: 'Oferta',
    prescriptionNote: 'A compra deste produto está restrita à quantidade autorizada no seu receituário. Selecione um documento da sua conta para validar.'
  };

  readonly seller = {
    name: 'AgroSupplies Ltda',
    avatar:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuDYTDaJ4R8WeOd2wXtSVI4lpTNEpo6euSl86ZtoV02c42Dw-X270c16otSJgusTwxsX8ptdUle_nlvhmY4yzWbe2626HXS8_9zl0OK3QZm8sF7CNQExOH9RrwmQrRDhSD491YyZhxdmd_aNaBIKhZG_N6Do3w1XWiu3iUMiulFFx2fazMJX2ZnQ3zJ9V08fMk_D2NLmX-Sx0mBrnkVRUfx5csN_Gl_TmHUukbdmIsTIi0srmKEEpAY7Pwai41gxpQdKUQQN6l26BBkL',
    verified: true,
    score: '98%'
  };

  readonly prescriptions: PrescriptionOption[] = [
    { id: '1', label: 'Receita #8842 - Soja (Válida até 10/12/2023)' },
    { id: '2', label: 'Receita #9921 - Milho (Válida até 15/01/2024)' },
    { id: '3', label: 'Receita #1102 - Multiculturas (Válida até 20/02/2024)' }
  ];

  readonly poll = {
    title: 'Poll de Vendas Ativo',
    discount: '-15% OFF',
    description: 'Compre junto e economize! Faltam 15 unidades para atingir o desconto máximo.',
    progress: 85,
    current: '0 un',
    target: 'Meta: 100 un',
    cta: 'Detalhes do Grupo'
  };

  readonly specifications: Specification[] = [
    { label: 'Grupo Químico', value: 'Glicina Substituída' },
    { label: 'Princípio Ativo', value: 'Glifosato 480g/L' },
    { label: 'Formulação', value: 'Concentrado Solúvel (SL)' },
    { label: 'Classe Toxicológica', value: 'Classe III', tone: 'warning' }
  ];

  readonly reviews: Review[] = [
    {
      name: 'João da Silva',
      subtitle: 'Produtor de Soja • Mato Grosso',
      initials: 'JD',
      color: 'bg-primary/20 text-primary',
      stars: ['full', 'full', 'full', 'full', 'full'],
      timeAgo: 'Há 2 semanas',
      text: 'Excelente produto. Usei na dessecação pré-plantio e o resultado foi muito rápido. O vendedor entregou antes do prazo combinado. Recomendo.'
    },
    {
      name: 'Marcos Pereira',
      subtitle: 'Pequeno Produtor • Paraná',
      initials: 'MP',
      color: 'bg-orange-100 text-orange-600',
      stars: ['full', 'full', 'full', 'full', 'empty'],
      timeAgo: 'Há 1 mês',
      text: 'O produto é bom, cumpre o que promete. Só achei a embalagem um pouco frágil no transporte, mas o conteúdo estava intacto. O poll de vendas ajudou muito no preço.'
    }
  ];

  readonly relatedProducts: RelatedProduct[] = [
    {
      brand: 'Yara',
      name: 'Fertilizante NPK 04-14-08',
      price: 'R$ 159,90',
      originalPrice: 'R$ 180,00',
      rating: '4.5',
      imageUrl:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuDYiPA3FgLur9zU93TLNcV2aKaUb6xG6jtWVEXZIQE2zJHfW4GhHxolUVsx2n-jiL-ElnBZeIgZzIqmTJF5HQ8Qv-52wzkXHvVr5gx0crcdRAzYPUMPKgIE9tSsb6AkOPXKaFaIrLY4t4XM6IELLJmOo1P68948YYqMa77Vfq9Yp0la9POGPO8dHXfyww1Xv5qUE9q4A2rImMMKBN2qCg1mqeiQMwNl4CATjnbjfeG6oV9LkJShXXhZzYwkZaxozJGGMoh9J75O4r-1'
    },
    {
      brand: 'Jacto',
      name: 'Pulverizador Costal 20L',
      price: 'R$ 320,00',
      rating: '4.9',
      imageUrl:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuDyfgHA-3LTKgRXyUgjM-0iNR1CzoMJ-wwr_NMsBF4DYoRYQZC5aWP_bGl2g48p7JPqBClOvUYXWzU1cRm7QNoNog3Z2XYRj7OOHu72waUiZMlyNcokSG-y1TBb6ZDHv-_kX40dZDkSR9owCMPXELiw6kyhLXs_C8codd-DIZ0e--mCDzBB7VQOMQZTB2i31R4TyYDUVTiR6DVf81oBbIPM5zdXNwka0NFxFswxUcvldSiqGLfeUwqZcR1WkD_BDEIHxt9CQIh4APxU'
    },
    {
      brand: 'Bayer',
      name: 'Semente de Milho Híbrido',
      price: 'R$ 801,00',
      originalPrice: 'R$ 890,00',
      rating: '4.7',
      discount: '-10%',
      imageUrl:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuCnV4r4xKWhoJ9BOvgpTidNv7_zZ_At67nHDl43_3uN8Ktg0W9SF5FKVxwQYFtNPvDLhqYluMEy9tx8ofDrWHYTgXAWCyI8p8UaCcwKozHBFnVYT5Ebx_Gi7y761rF7oZ9kdYgWdBq4Gxi8_wdspaS-kZeM23wtdp48kLaaCDEDdx6K3JN4LkQSB0H3SBwto9UQ69Aml40b72rrNlkdIBFTTopR9miaaUr-tZjRD_hq8UoZccKM81rmmR17VOQ_w4SL9C4fzKcpem7m'
    },
    {
      brand: 'Syngenta',
      name: 'Inseticida Engeo Pleno',
      price: 'R$ 210,00',
      rating: '5.0',
      imageUrl:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuCqXlw2s-WUAdq01tv3g2uLH_YcwIkpLXZljVJ-W-ad038-Vgl6yNEPwJHZlbYIAmkQYcgHvlxGlDCFhex7UCHrV7WiUZQ-HFLRmYry2IYOkK1djp1xuip7LJnKhKlenK8Hhk0em0AqqX7iHV7wdOEvkqtzjzGPi5x-Y_MvG1SHbjgZmE_Mn06m6eo9tLLPJX9odO-4oADTKVI_BC_rw7xqpBFiAPq5p6-zQyMzEuOh2Usxr1CVYQ_O0BbvLqhBux0auLFdJ9R6RuMy'
    }
  ];

  constructor(private readonly authService: AuthService) {}

  ngOnInit(): void {
    this.isAuthenticated = this.authService.hasActiveSession();
  }

  setActiveImage(image: ProductImage): void {
    this.activeImage = image;
  }
}
