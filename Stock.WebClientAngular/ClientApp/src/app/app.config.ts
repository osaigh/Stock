import {
  ApplicationConfig,
  inject,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideRouter } from '@angular/router';
import { provideStore } from '@ngrx/store';
import { routes } from './app.routes';
import { UserAuthentication } from './services/user-authentication';
import { sidebarReducer } from './statemanagement/sidebar.reducers';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    // @swimlane/ngx-charts still uses @angular/animations triggers.
    provideAnimationsAsync(),
    provideStore({ sidebar: sidebarReducer }),
    provideAppInitializer(() => inject(UserAuthentication).loadUser()),
  ],
};
