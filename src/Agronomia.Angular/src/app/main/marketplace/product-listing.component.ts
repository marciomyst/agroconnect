import { Component } from '@angular/core';
import { ProductCard, FilterGroup } from './product-listing.types';

@Component({
  selector: 'app-product-listing',
  templateUrl: './product-listing.component.html',
  standalone: false,
  styleUrl: './product-listing.component.css'
})
export class ProductListingComponent {
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
}
