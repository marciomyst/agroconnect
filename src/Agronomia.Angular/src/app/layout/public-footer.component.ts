import { Component } from '@angular/core';

@Component({
  selector: 'app-public-footer',
  standalone: true,
  template: `
    <footer class="bg-background-dark border-t border-border-dark w-full mt-auto">
      <div class="border-b border-border-dark bg-surface-dark/30">
        <div class="max-w-[1280px] mx-auto px-6 py-10">
          <div class="flex flex-col lg:flex-row items-center justify-between gap-8">
            <div class="flex flex-col gap-2 text-center lg:text-left max-w-2xl">
              <h2 class="text-2xl md:text-3xl font-bold tracking-tight text-white">
                Fique por dentro das novidades do agro
              </h2>
              <p class="text-text-muted text-base">
                Receba cotacoes diarias, ofertas exclusivas e noticias do mercado agricola diretamente no seu e-mail.
              </p>
            </div>
            <div class="w-full lg:w-auto flex-shrink-0">
              <form class="flex flex-col sm:flex-row gap-3 w-full lg:min-w-[480px]">
                <div class="relative flex-grow group">
                  <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                    <span class="material-symbols-outlined text-text-muted group-focus-within:text-primary">mail</span>
                  </div>
                  <input
                    class="w-full bg-[#2f4922] border border-transparent focus:border-primary focus:ring-1 focus:ring-primary rounded-lg py-3 pl-10 pr-4 text-white placeholder:text-[#6b8c5a] transition-all outline-none"
                    placeholder="Seu e-mail profissional"
                    type="email"
                  />
                </div>
                <button
                  class="bg-primary hover:bg-[#3da608] text-[#162310] font-bold py-3 px-8 rounded-lg transition-colors flex items-center justify-center gap-2 whitespace-nowrap shadow-[0_4px_14px_0_rgba(71,194,10,0.39)]"
                  type="button"
                >
                  <span>Inscrever-se</span>
                  <span class="material-symbols-outlined text-[20px]">arrow_forward</span>
                </button>
              </form>
              <p class="text-xs text-[#6b8c5a] mt-2 text-center lg:text-left">
                Ao se inscrever, voce concorda com nossa Politica de Privacidade.
              </p>
            </div>
          </div>
        </div>
      </div>
      <div class="max-w-[1280px] mx-auto px-6 py-16">
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-x-8 gap-y-12">
          <div class="flex flex-col gap-6">
            <h3 class="text-white font-bold text-lg flex items-center gap-2">Conheca-nos</h3>
            <ul class="flex flex-col gap-3">
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Sobre a AgroMarket
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Blog do Agro
                  <span class="text-[10px] bg-primary/20 text-primary px-1.5 py-0.5 rounded font-bold uppercase">Novo</span>
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Carreiras
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Imprensa
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Sustentabilidade
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Mapa do Site
                </a>
              </li>
            </ul>
          </div>
          <div class="flex flex-col gap-6">
            <h3 class="text-white font-bold text-lg flex items-center gap-2">Ganhe Dinheiro Conosco</h3>
            <ul class="flex flex-col gap-3">
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Venda seus produtos
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Poll de Vendas
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Seja um Parceiro Logistico
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Programa de Afiliados
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Solucoes para Cooperativas
                </a>
              </li>
            </ul>
          </div>
          <div class="flex flex-col gap-8">
            <div class="flex flex-col gap-4">
              <h3 class="text-white font-bold text-lg">Pagamento</h3>
              <div class="grid grid-cols-4 gap-2">
                <div
                  class="h-9 bg-surface-dark border border-border-dark rounded flex items-center justify-center text-text-muted hover:text-white hover:border-primary/50 transition-colors"
                  title="Cartao de Credito"
                >
                  <span class="material-symbols-outlined">credit_card</span>
                </div>
                <div
                  class="h-9 bg-surface-dark border border-border-dark rounded flex items-center justify-center text-text-muted hover:text-white hover:border-primary/50 transition-colors"
                  title="Boleto Bancario"
                >
                  <span class="material-symbols-outlined">receipt_long</span>
                </div>
                <div
                  class="h-9 bg-surface-dark border border-border-dark rounded flex items-center justify-center text-text-muted hover:text-white hover:border-primary/50 transition-colors"
                  title="Pix"
                >
                  <span class="material-symbols-outlined">qr_code_2</span>
                </div>
                <div
                  class="h-9 bg-surface-dark border border-border-dark rounded flex items-center justify-center text-text-muted hover:text-white hover:border-primary/50 transition-colors"
                  title="Transferencia"
                >
                  <span class="material-symbols-outlined">account_balance</span>
                </div>
              </div>
            </div>
            <div class="flex flex-col gap-4">
              <h3 class="text-white font-bold text-lg">Seguranca</h3>
              <div class="flex flex-wrap gap-3">
                <div class="flex items-center gap-2 px-3 py-2 bg-surface-dark border border-border-dark rounded-lg">
                  <span class="material-symbols-outlined text-primary text-xl">lock</span>
                  <div class="flex flex-col">
                    <span class="text-[10px] text-text-muted leading-tight uppercase font-bold">Site</span>
                    <span class="text-xs text-white font-bold leading-tight">Seguro</span>
                  </div>
                </div>
                <div class="flex items-center gap-2 px-3 py-2 bg-surface-dark border border-border-dark rounded-lg">
                  <span class="material-symbols-outlined text-primary text-xl">verified_user</span>
                  <div class="flex flex-col">
                    <span class="text-[10px] text-text-muted leading-tight uppercase font-bold">Dados</span>
                    <span class="text-xs text-white font-bold leading-tight">Protegidos</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="flex flex-col gap-6">
            <h3 class="text-white font-bold text-lg flex items-center gap-2">Ajuda</h3>
            <ul class="flex flex-col gap-3">
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Central de Atendimento
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Termos de Uso
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Politica de Privacidade
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Trocas e Devolucoes
                </a>
              </li>
              <li>
                <a
                  class="text-text-muted hover:text-white hover:translate-x-1 transition-all duration-200 text-sm inline-flex items-center gap-2"
                  href="#"
                >
                  Como Comprar
                </a>
              </li>
            </ul>
            <a
              class="group mt-2 flex items-center justify-between p-3 rounded-lg bg-surface-dark border border-border-dark hover:border-primary/50 transition-all"
              href="#"
            >
              <div class="flex items-center gap-3">
                <div
                  class="w-8 h-8 rounded-full bg-primary/10 text-primary flex items-center justify-center group-hover:bg-primary group-hover:text-[#162310] transition-colors"
                >
                  <span class="material-symbols-outlined text-lg">headset_mic</span>
                </div>
                <div class="flex flex-col">
                  <span class="text-xs text-text-muted font-medium">Precisa de ajuda?</span>
                  <span class="text-sm text-white font-bold">Fale Conosco</span>
                </div>
              </div>
              <span class="material-symbols-outlined text-text-muted group-hover:text-primary transition-colors text-sm">
                chevron_right
              </span>
            </a>
          </div>
        </div>
      </div>
      <div class="border-t border-border-dark bg-[#0f160a]">
        <div class="max-w-[1280px] mx-auto px-6 py-8">
          <div class="flex flex-col md:flex-row items-center justify-between gap-6">
            <div class="flex flex-col items-center md:items-start gap-4">
              <div class="flex items-center gap-2 opacity-90 hover:opacity-100 transition-opacity cursor-pointer">
                <div class="bg-primary p-1.5 rounded-lg text-[#162310]">
                  <span class="material-symbols-outlined block">grass</span>
                </div>
                <span class="text-xl font-extrabold text-white tracking-tight">AgroMarket</span>
              </div>
              <div class="text-center md:text-left">
                <p class="text-xs text-[#6b8c5a]">© 2024 AgroMarketplace. Todos os direitos reservados.</p>
                <p class="text-xs text-[#4a633e] mt-1">
                  CNPJ 00.000.000/0001-00 • Av. das Lavouras, 1000 - Sao Paulo, SP
                </p>
              </div>
            </div>
            <div class="flex items-center gap-3">
              <a
                aria-label="Instagram"
                class="w-10 h-10 rounded-full bg-surface-dark border border-border-dark text-text-muted hover:text-white hover:bg-primary/20 hover:border-primary hover:scale-110 transition-all flex items-center justify-center"
                href="#"
              >
                <span class="material-symbols-outlined">photo_camera</span>
              </a>
              <a
                aria-label="LinkedIn"
                class="w-10 h-10 rounded-full bg-surface-dark border border-border-dark text-text-muted hover:text-white hover:bg-primary/20 hover:border-primary hover:scale-110 transition-all flex items-center justify-center"
                href="#"
              >
                <span class="material-symbols-outlined">business_center</span>
              </a>
              <a
                aria-label="Facebook"
                class="w-10 h-10 rounded-full bg-surface-dark border border-border-dark text-text-muted hover:text-white hover:bg-primary/20 hover:border-primary hover:scale-110 transition-all flex items-center justify-center"
                href="#"
              >
                <span class="material-symbols-outlined">groups</span>
              </a>
              <a
                aria-label="YouTube"
                class="w-10 h-10 rounded-full bg-surface-dark border border-border-dark text-text-muted hover:text-white hover:bg-primary/20 hover:border-primary hover:scale-110 transition-all flex items-center justify-center"
                href="#"
              >
                <span class="material-symbols-outlined">smart_display</span>
              </a>
            </div>
          </div>
        </div>
      </div>
    </footer>
  `,
})
export class PublicFooterComponent {}
