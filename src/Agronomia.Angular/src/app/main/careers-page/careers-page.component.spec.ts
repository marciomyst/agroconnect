import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { CareersPageComponent } from './careers-page.component';

describe('CareersPageComponent', () => {
  let component: CareersPageComponent;
  let fixture: ComponentFixture<CareersPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CareersPageComponent],
      imports: [RouterTestingModule]
    }).compileComponents();

    fixture = TestBed.createComponent(CareersPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
