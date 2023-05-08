import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AppRoutingModule } from './app-routing.module';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { StockContainerComponent } from './components/stock-container/stock-container.component';
import { HomeComponent } from './home/home.component';
import { SignInCallbackComponent } from './sign-in-callback/sign-in-callback.component';
import { SignOutCallbackComponent } from './sign-out-callback/sign-out-callback.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { StoreModule } from '@ngrx/store';
import { sidebarReducer } from './statemanagement/sidebar.reducers';


@NgModule({
  declarations: [
    AppComponent,
    LoginFormComponent,
    PageHeaderComponent,
    StockContainerComponent,
    HomeComponent,
    SignInCallbackComponent,
    SignOutCallbackComponent,
    NavbarComponent,
    SidebarComponent
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  imports: [BrowserModule,BrowserAnimationsModule, AppRoutingModule,NgxChartsModule,
    StoreModule.forRoot({ sidebar: sidebarReducer }),
    RouterModule.forRoot([
      {path:"Home", component:HomeComponent},
      {path:"**",redirectTo:"Home"}
      
    ]),
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
