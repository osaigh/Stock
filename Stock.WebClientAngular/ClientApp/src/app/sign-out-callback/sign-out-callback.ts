import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserAuthentication } from '../services/user-authentication';

@Component({
  imports: [],
  selector: 'app-sign-out-callback',
  styleUrl: './sign-out-callback.css',
  templateUrl: './sign-out-callback.html',
})
export class SignOutCallback implements OnInit {
  private readonly userAuthentication = inject(UserAuthentication);
  private readonly router = inject(Router);

  ngOnInit(): void {
    void this.signOutAsync();
  }

  private async signOutAsync(): Promise<void> {
    try {
      await this.userAuthentication.signoutRedirectCallback();
    } catch (error) {
      console.error('Sign out callback failed', error);
    }
    await this.router.navigate(['/'], { replaceUrl: true });
  }
}
