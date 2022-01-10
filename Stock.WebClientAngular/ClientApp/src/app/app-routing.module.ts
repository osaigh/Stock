import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { SignInCallbackComponent } from './sign-in-callback/sign-in-callback.component';
import { SignOutCallbackComponent } from './sign-out-callback/sign-out-callback.component';

const routes: Routes = [
  {
	  path:'',
	  component: HomeComponent,
  },
  {
	  path:'signincallback',
	  component: SignInCallbackComponent,
  },
  {
	  path:'signoutcallback',
	  component: SignOutCallbackComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
