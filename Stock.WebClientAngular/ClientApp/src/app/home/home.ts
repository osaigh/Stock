import { Component, computed, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { Navbar } from '../components/navbar/navbar';
import { Sidebar } from '../components/sidebar/sidebar';
import { StockContainer } from '../components/stock-container/stock-container';
import { UserAuthentication } from '../services/user-authentication';
import { selectSidebarStatus } from '../statemanagement/sidebar.selectors';

@Component({
  imports: [Navbar, Sidebar, StockContainer],
  selector: 'app-home',
  styleUrl: './home.css',
  templateUrl: './home.html',
})
export class Home {
  private readonly userAuthentication = inject(UserAuthentication);
  private readonly sidebarStatus = inject(Store).selectSignal(selectSidebarStatus);

  protected readonly isUserSignedIn = this.userAuthentication.isSignedIn;
  protected readonly contentClassList = computed(() =>
    this.sidebarStatus() === 'hide' ? 'maincontent expand' : 'maincontent',
  );
}
