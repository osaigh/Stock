import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideStore, Store } from '@ngrx/store';
import { toggleState } from '../../statemanagement/sidebar.actions';
import { sidebarReducer } from '../../statemanagement/sidebar.reducers';
import { Sidebar } from './sidebar';

describe('Sidebar', () => {
  let component: Sidebar;
  let fixture: ComponentFixture<Sidebar>;
  let element: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Sidebar],
      providers: [provideRouter([]), provideStore({ sidebar: sidebarReducer })],
    }).compileComponents();

    fixture = TestBed.createComponent(Sidebar);
    component = fixture.componentInstance;
    element = fixture.nativeElement as HTMLElement;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should collapse and expand when the sidebar state is toggled', async () => {
    const store = TestBed.inject(Store);
    expect(element.querySelector('.sidebar')?.className).toBe('sidebar');

    store.dispatch(toggleState());
    await fixture.whenStable();
    expect(element.querySelector('.sidebar')?.className).toBe('sidebar hide');

    store.dispatch(toggleState());
    await fixture.whenStable();
    expect(element.querySelector('.sidebar')?.className).toBe('sidebar');
  });
});
