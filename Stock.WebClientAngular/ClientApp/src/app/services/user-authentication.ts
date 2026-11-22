import { computed, Service, signal } from '@angular/core';
import { User, UserManager } from 'oidc-client-ts';
import { IdentityConfig } from '../configuration/config';
import { UserInterface } from '../models';

@Service()
export class UserAuthentication {
  private readonly userManager = new UserManager(IdentityConfig);
  private readonly currentUser = signal<User | null>(null);

  readonly user = computed<UserInterface | null>(() => {
    const profile = this.currentUser()?.profile;
    return profile ? { username: profile.name ?? profile.preferred_username ?? '' } : null;
  });
  readonly isSignedIn = computed(() => this.currentUser() !== null);

  constructor() {
    this.userManager.events.addUserLoaded((user) => this.currentUser.set(user));
    this.userManager.events.addUserUnloaded(() => this.currentUser.set(null));
    this.userManager.events.addUserSignedOut(() => this.currentUser.set(null));
    this.userManager.events.addAccessTokenExpired(() => this.currentUser.set(null));
  }

  async signIn(): Promise<void> {
    try {
      await this.userManager.signinRedirect();
    } catch (error) {
      console.error('Sign in failed', error);
    }
  }

  async signOut(): Promise<void> {
    try {
      await this.userManager.signoutRedirect();
    } catch (error) {
      console.error('Sign out failed', error);
    }
  }

  /** Restores the signed-in user (if any) from storage. */
  async loadUser(): Promise<void> {
    try {
      this.currentUser.set(await this.getValidUser());
    } catch (error) {
      console.error('Loading the signed-in user failed', error);
      this.currentUser.set(null);
    }
  }

  async getAccessToken(): Promise<string | null> {
    const user = await this.getValidUser();
    return user?.access_token ?? null;
  }

  async signinRedirectCallback(): Promise<void> {
    await this.userManager.signinCallback();
  }

  async signoutRedirectCallback(): Promise<void> {
    await this.userManager.signoutCallback();
  }

  private async getValidUser(): Promise<User | null> {
    const user = await this.userManager.getUser();
    return user && !user.expired ? user : null;
  }
}
