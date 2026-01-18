import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { UserDropdownComponent } from './header/user-dropdown.component';

@NgModule({
  declarations: [HeaderComponent, UserDropdownComponent, FooterComponent],
  imports: [CommonModule, RouterModule],
  exports: [HeaderComponent, UserDropdownComponent, FooterComponent]
})
export class SharedUiModule {}
