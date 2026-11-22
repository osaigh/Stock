import { signal } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideStore, Store } from '@ngrx/store';
import { StockApi } from '../services/stock-api';
import { UserAuthentication } from '../services/user-authentication';
import { toggleState } from '../statemanagement/sidebar.actions';
import { sidebarReducer } from '../statemanagement/sidebar.reducers';
import { Home } from './home';

describe('Home', () => {
  let component: Home;
  let fixture: ComponentFixture<Home>;
  let element: HTMLElement;
  const isSignedIn = signal(false);

  beforeEach(async () => {
    isSignedIn.set(false);

    await TestBed.configureTestingModule({
      imports: [Home],
      providers: [
        provideRouter([]),
        provideStore({ sidebar: sidebarReducer }),
        { provide: UserAuthentication, useValue: { isSignedIn } },
        { provide: StockApi, useValue: { getAllStocks: () => Promise.resolve([]) } },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(Home);
    component = fixture.componentInstance;
    element = fixture.nativeElement as HTMLElement;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should ask the user to sign in when signed out', () => {
    expect(element.querySelector('h3')?.textContent).toContain('Please Sign In');
    expect(element.querySelector('app-stock-container')).toBeNull();
  });

  it('should show the stocks when signed in', async () => {
    isSignedIn.set(true);
    await fixture.whenStable();

    expect(element.querySelector('h3')).toBeNull();
    // The stock container is in a @defer block, so it renders once its chunk is loaded.
    await vi.waitFor(() => expect(element.querySelector('app-stock-container')).not.toBeNull());
  });

  it('should expand the content when the sidebar is hidden', async () => {
    expect(element.querySelector('.maincontent')?.classList.contains('expand')).toBe(false);

    TestBed.inject(Store).dispatch(toggleState());
    await fixture.whenStable();

    expect(element.querySelector('.maincontent')?.classList.contains('expand')).toBe(true);
  });
});
