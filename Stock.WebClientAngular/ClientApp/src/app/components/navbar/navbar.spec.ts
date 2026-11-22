import { signal } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideStore, Store } from '@ngrx/store';
import { UserAuthentication } from '../../services/user-authentication';
import { sidebarReducer } from '../../statemanagement/sidebar.reducers';
import { selectSidebarStatus } from '../../statemanagement/sidebar.selectors';
import { Navbar } from './navbar';

describe('Navbar', () => {
  let component: Navbar;
  let fixture: ComponentFixture<Navbar>;
  let element: HTMLElement;
  const userAuthentication = {
    isSignedIn: signal(false),
    signIn: vi.fn(() => Promise.resolve()),
    signOut: vi.fn(() => Promise.resolve()),
  };

  beforeEach(async () => {
    userAuthentication.isSignedIn.set(false);
    userAuthentication.signIn.mockClear();
    userAuthentication.signOut.mockClear();

    await TestBed.configureTestingModule({
      imports: [Navbar],
      providers: [
        provideStore({ sidebar: sidebarReducer }),
        { provide: UserAuthentication, useValue: userAuthentication },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(Navbar);
    component = fixture.componentInstance;
    element = fixture.nativeElement as HTMLElement;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should sign in from the Sign In button when signed out', () => {
    const button = element.querySelector('button');
    expect(button?.textContent).toContain('Sign In');

    button?.click();

    expect(userAuthentication.signIn).toHaveBeenCalledTimes(1);
  });

  it('should sign out from the Sign Out button when signed in', async () => {
    userAuthentication.isSignedIn.set(true);
    await fixture.whenStable();

    const button = element.querySelector('button');
    expect(button?.textContent).toContain('Sign Out');

    button?.click();

    expect(userAuthentication.signOut).toHaveBeenCalledTimes(1);
  });

  it('should toggle the sidebar from the menu icon', () => {
    const status = TestBed.inject(Store).selectSignal(selectSidebarStatus);
    expect(status()).toBe('');

    element.querySelector<HTMLElement>('.menu')?.click();

    expect(status()).toBe('hide');
  });
});
