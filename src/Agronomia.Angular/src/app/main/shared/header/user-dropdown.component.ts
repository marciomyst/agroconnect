import { Component, ElementRef, EventEmitter, HostListener, Input, Output } from '@angular/core';

@Component({
  selector: 'app-user-dropdown',
  templateUrl: './user-dropdown.component.html',
  standalone: false,
  styleUrl: './user-dropdown.component.css'
})
export class UserDropdownComponent {
  @Input() userName = 'Joao Silva';
  @Input() userRole = 'Produtor';
  @Input() avatarUrl = '';
  @Input() pendingOrders = 0;

  @Output() selectProfile = new EventEmitter<void>();
  @Output() selectOrders = new EventEmitter<void>();
  @Output() selectPrescriptions = new EventEmitter<void>();
  @Output() selectSettings = new EventEmitter<void>();
  @Output() selectHelp = new EventEmitter<void>();
  @Output() logout = new EventEmitter<void>();

  isOpen = false;

  constructor(private readonly elementRef: ElementRef<HTMLElement>) {}

  toggle(event?: Event): void {
    event?.stopPropagation();
    this.isOpen = !this.isOpen;
  }

  close(): void {
    this.isOpen = false;
  }

  onProfile(): void {
    this.selectProfile.emit();
    this.close();
  }

  onOrders(): void {
    this.selectOrders.emit();
    this.close();
  }

  onPrescriptions(): void {
    this.selectPrescriptions.emit();
    this.close();
  }

  onSettings(): void {
    this.selectSettings.emit();
    this.close();
  }

  onHelp(): void {
    this.selectHelp.emit();
    this.close();
  }

  onLogout(): void {
    this.logout.emit();
    this.close();
  }

  get avatarStyle(): Record<string, string> {
    if (!this.avatarUrl) {
      return {};
    }

    return { 'background-image': `url("${this.avatarUrl}")` };
  }

  @HostListener('document:click', ['$event'])
  handleOutsideClick(event: MouseEvent): void {
    if (!this.isOpen) {
      return;
    }

    const target = event.target as HTMLElement | null;
    if (target && this.elementRef.nativeElement.contains(target)) {
      return;
    }

    this.close();
  }
}
