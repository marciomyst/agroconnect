import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ResellersPageComponent } from './resellers-page.component';

describe('ResellersPageComponent', () => {
  let component: ResellersPageComponent;
  let fixture: ComponentFixture<ResellersPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ResellersPageComponent],
      imports: [RouterTestingModule]
    }).compileComponents();

    fixture = TestBed.createComponent(ResellersPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
