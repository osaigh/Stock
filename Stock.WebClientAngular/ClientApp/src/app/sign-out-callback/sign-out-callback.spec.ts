import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { UserAuthentication } from '../services/user-authentication';
import { SignOutCallback } from './sign-out-callback';

describe('SignOutCallback', () => {
  let component: SignOutCallback;
  let fixture: ComponentFixture<SignOutCallback>;
  const userAuthentication = { signoutRedirectCallback: vi.fn(() => Promise.resolve()) };

  beforeEach(async () => {
    userAuthentication.signoutRedirectCallback.mockClear();

    await TestBed.configureTestingModule({
      imports: [SignOutCallback],
      providers: [provideRouter([]), { provide: UserAuthentication, useValue: userAuthentication }],
    }).compileComponents();

    vi.spyOn(TestBed.inject(Router), 'navigate').mockResolvedValue(true);
    fixture = TestBed.createComponent(SignOutCallback);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should complete the sign out and go home', async () => {
    expect(userAuthentication.signoutRedirectCallback).toHaveBeenCalledTimes(1);
    await vi.waitFor(() =>
      expect(TestBed.inject(Router).navigate).toHaveBeenCalledWith(['/'], { replaceUrl: true }),
    );
  });
});
