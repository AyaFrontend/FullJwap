import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangingPasswordComponent } from './changing-password.component';

describe('ChangingPasswordComponent', () => {
  let component: ChangingPasswordComponent;
  let fixture: ComponentFixture<ChangingPasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangingPasswordComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangingPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
