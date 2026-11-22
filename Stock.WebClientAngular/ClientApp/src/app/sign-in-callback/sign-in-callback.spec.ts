import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { UserAuthentication } from '../services/user-authentication';
import { SignInCallback } from './sign-in-callback';

describe('SignInCallback', () => {
  let component: SignInCallback;
  let fixture: ComponentFixture<SignInCallback>;
  const userAuthentication = { signinRedirectCallback: vi.fn(() => Promise.resolve()) };

  beforeEach(async () => {
    userAuthentication.signinRedirectCallback.mockClear();

    await TestBed.configureTestingModule({
      imports: [SignInCallback],
      providers: [provideRouter([]), { provide: UserAuthentication, useValue: userAuthentication }],
    }).compileComponents();

    vi.spyOn(TestBed.inject(Router), 'navigate').mockResolvedValue(true);
    fixture = TestBed.createComponent(SignInCallback);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should complete the sign in and go home', async () => {
    expect(userAuthentication.signinRedirectCallback).toHaveBeenCalledTimes(1);
    await vi.waitFor(() =>
      expect(TestBed.inject(Router).navigate).toHaveBeenCalledWith(['/'], { replaceUrl: true }),
    );
  });
});
