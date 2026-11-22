import { Component, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { UserAuthentication } from '../../services/user-authentication';
import { toggleState } from '../../statemanagement/sidebar.actions';

@Component({
  imports: [],
  selector: 'app-navbar',
  styleUrl: './navbar.css',
  templateUrl: './navbar.html',
})
export class Navbar {
  private readonly userAuthentication = inject(UserAuthentication);
  private readonly store = inject(Store);

  protected readonly isUserSignedIn = this.userAuthentication.isSignedIn;

  protected toggleState(): void {
    this.store.dispatch(toggleState());
  }

  protected signIn(): void {
    void this.userAuthentication.signIn();
  }

  protected signOut(): void {
    void this.userAuthentication.signOut();
  }
}
