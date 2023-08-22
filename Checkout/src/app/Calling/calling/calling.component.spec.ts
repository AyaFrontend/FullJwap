import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CallingComponent } from './calling.component';

describe('CallingComponent', () => {
  let component: CallingComponent;
  let fixture: ComponentFixture<CallingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CallingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CallingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
