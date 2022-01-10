import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignOutCallbackComponent } from './sign-out-callback.component';

describe('SignOutCallbackComponent', () => {
  let component: SignOutCallbackComponent;
  let fixture: ComponentFixture<SignOutCallbackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SignOutCallbackComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SignOutCallbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
