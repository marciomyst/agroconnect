import { Component } from '@angular/core';

@Component({
  selector: 'app-public-header',
  standalone: true,
  template: `
    <header class="flex flex-col w-full z-50">
      <div class="bg-[#1a2e14] text-white h-[64px] flex items-center px-4 gap-2 lg:gap-4 w-full relative z-20">
        <a
          class="header-item-hover flex items-center gap-1 p-1 md:p-2 min-w-fit focus:outline-none focus:ring-2 focus:ring-primary/50"
          href="#"
        >
          <div class="text-primary">
            <span class="material-symbols-outlined !text-[32px]">agriculture</span>
          </div>
          <div class="flex flex-col leading-none">
            <span class="text-lg font-bold tracking-tight">AgroMarketplace</span>
            <span class="text-[10px] text-gray-300 tracking-wider hidden sm:block">SOLUCOES AGRICOLAS</span>
          </div>
        </a>

        <button
          class="header-item-hover hidden lg:flex flex-col justify-center px-2 py-1.5 min-w-[140px] leading-tight text-left"
          type="button"
        >
          <div class="text-xs text-gray-300 ml-5">Enviar para</div>
          <div class="flex items-center font-bold text-sm">
            <span class="material-symbols-outlined !text-[18px] mr-1 text-white">location_on</span>
            Sao Paulo 01000...
          </div>
        </button>

        <div class="flex-1 flex items-center h-10 max-w-[800px] mx-2">
          <form
            class="flex w-full h-full rounded-md overflow-hidden focus-within:ring-2 focus-within:ring-primary shadow-sm"
            (submit)="$event.preventDefault()"
          >
            <div
              class="hidden md:flex relative h-full bg-[#f3f3f3] hover:bg-[#e3e6e6] transition-colors border-r border-gray-300 text-gray-600 cursor-pointer"
            >
              <select
                class="appearance-none bg-transparent h-full pl-3 pr-6 text-xs text-gray-700 outline-none border-none cursor-pointer hover:text-black"
              >
                <option>Todos</option>
                <option>Defensivos</option>
                <option>Fertilizantes</option>
                <option>Sementes</option>
                <option>Maquinas</option>
                <option>Pecas</option>
              </select>
              <span
                class="material-symbols-outlined !text-[16px] absolute right-1 top-1/2 -translate-y-1/2 pointer-events-none text-gray-500"
              >
                arrow_drop_down
              </span>
            </div>
            <input
              class="flex-1 h-full px-3 text-black text-sm placeholder:text-gray-500 border-none outline-none focus:ring-0"
              placeholder="Buscar defensivos, sementes, marcas..."
              type="text"
            />
            <button
              class="bg-primary hover:bg-primary-hover transition-colors w-12 flex items-center justify-center h-full border-none cursor-pointer"
              type="submit"
            >
              <span class="material-symbols-outlined text-[#121c0d] font-bold">search</span>
            </button>
          </form>
        </div>

        <div class="flex items-center gap-1 md:gap-2 min-w-fit">
          <button class="header-item-hover hidden xl:flex items-center gap-1 px-2 py-2 font-bold text-sm" type="button">
            <span class="text-xs">BR</span>
            <span class="material-symbols-outlined !text-[16px] text-gray-400">arrow_drop_down</span>
          </button>
          <a class="header-item-hover hidden md:flex flex-col justify-center px-2 py-1.5 leading-tight" href="#">
            <span class="text-xs text-gray-100">Ola, faca seu login</span>
            <div class="flex items-center font-bold text-sm">
              Contas e Listas
              <span class="material-symbols-outlined !text-[18px] text-gray-400">arrow_drop_down</span>
            </div>
          </a>
          <a class="header-item-hover hidden lg:flex flex-col justify-center px-2 py-1.5 leading-tight" href="#">
            <span class="text-xs text-gray-100">Devolucoes</span>
            <span class="font-bold text-sm">e Pedidos</span>
          </a>
          <a class="header-item-hover flex items-end px-2 py-1 relative" href="#">
            <div class="relative">
              <span class="material-symbols-outlined !text-[32px]">shopping_cart</span>
              <span
                class="absolute -top-1 -right-1 md:right-0 bg-primary text-[#121c0d] text-xs font-bold w-5 h-5 flex items-center justify-center rounded-full border-2 border-[#1a2e14]"
              >
                0
              </span>
            </div>
            <span class="font-bold text-sm hidden md:block mb-1">Carrinho</span>
          </a>
        </div>
      </div>

      <div class="bg-[#2a4220] text-white h-[42px] flex items-center px-4 w-full shadow-md overflow-x-auto no-scrollbar">
        <button
          class="flex items-center gap-1 px-3 py-1 hover:bg-white/10 rounded-sm font-bold text-sm whitespace-nowrap mr-2 h-[34px] transition-colors"
          type="button"
        >
          <span class="material-symbols-outlined !text-[24px]">menu</span>
          <span>Todas</span>
        </button>
        <nav class="flex items-center gap-1 h-full">
          <a
            class="px-3 py-1 hover:bg-white/10 rounded-sm text-sm font-medium whitespace-nowrap h-[34px] flex items-center transition-colors"
            href="#"
          >
            Defensivos
          </a>
          <a
            class="px-3 py-1 hover:bg-white/10 rounded-sm text-sm font-medium whitespace-nowrap h-[34px] flex items-center transition-colors"
            href="#"
          >
            Fertilizantes
          </a>
          <a
            class="px-3 py-1 hover:bg-white/10 rounded-sm text-sm font-medium whitespace-nowrap h-[34px] flex items-center transition-colors"
            href="#"
          >
            Sementes
          </a>
          <a
            class="px-3 py-1 hover:bg-white/10 rounded-sm text-sm font-medium whitespace-nowrap h-[34px] flex items-center transition-colors"
            href="#"
          >
            Pool de Vendas
          </a>
          <a
            class="px-3 py-1 hover:bg-white/10 rounded-sm text-sm font-medium whitespace-nowrap h-[34px] flex items-center transition-colors"
            href="#"
          >
            Diagnostico
          </a>
          <a
            class="px-3 py-1 hover:bg-white/10 rounded-sm text-sm font-medium whitespace-nowrap h-[34px] flex items-center transition-colors hidden md:flex"
            href="#"
          >
            Maquinario
          </a>
          <a
            class="px-3 py-1 hover:bg-white/10 rounded-sm text-sm font-medium whitespace-nowrap h-[34px] flex items-center transition-colors hidden lg:flex"
            href="#"
          >
            Ofertas do Dia
          </a>
        </nav>
        <div class="ml-auto hidden xl:flex">
          <a
            class="px-3 py-1 hover:bg-white/10 rounded-sm text-sm font-bold whitespace-nowrap h-[34px] flex items-center transition-colors text-primary"
            href="#"
          >
            Venda no AgroMarketplace
          </a>
        </div>
      </div>
    </header>
  `,
  styles: [
    `
      .header-item-hover {
        border: 1px solid transparent;
        border-radius: 4px;
      }
      .header-item-hover:hover {
        border: 1px solid white;
      }
      .no-scrollbar::-webkit-scrollbar {
        display: none;
      }
      .no-scrollbar {
        -ms-overflow-style: none;
        scrollbar-width: none;
      }
    `,
  ],
})
export class PublicHeaderComponent {}
