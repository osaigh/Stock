import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserAuthentication } from '../services/user-authentication';

@Component({
  imports: [],
  selector: 'app-sign-in-callback',
  styleUrl: './sign-in-callback.css',
  templateUrl: './sign-in-callback.html',
})
export class SignInCallback implements OnInit {
  private readonly userAuthentication = inject(UserAuthentication);
  private readonly router = inject(Router);

  ngOnInit(): void {
    void this.signInAsync();
  }

  private async signInAsync(): Promise<void> {
    try {
      await this.userAuthentication.signinRedirectCallback();
    } catch (error) {
      console.error('Sign in callback failed', error);
    }
    await this.router.navigate(['/'], { replaceUrl: true });
  }
}
