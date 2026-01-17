import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FarmersPageComponent } from './farmers-page.component';

describe('FarmersPageComponent', () => {
  let component: FarmersPageComponent;
  let fixture: ComponentFixture<FarmersPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FarmersPageComponent],
      imports: [RouterTestingModule]
    }).compileComponents();

    fixture = TestBed.createComponent(FarmersPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
