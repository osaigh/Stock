import { Routes } from '@angular/router';
import { Home } from './home/home';
import { SignInCallback } from './sign-in-callback/sign-in-callback';
import { SignOutCallback } from './sign-out-callback/sign-out-callback';

export const routes: Routes = [
  { path: '', pathMatch: 'full', component: Home },
  { path: 'signincallback', component: SignInCallback },
  { path: 'signoutcallback', component: SignOutCallback },
  { path: '**', redirectTo: '' },
];
