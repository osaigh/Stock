import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserAuthenticationService } from '../services/user-authentication.service';

@Component({
  selector: 'app-sign-in-callback',
  templateUrl: './sign-in-callback.component.html',
  styleUrls: ['./sign-in-callback.component.css']
})
export class SignInCallbackComponent implements OnInit {

  constructor(
    private userAuthenticationService: UserAuthenticationService,
    private router: Router) { 
      console.log(userAuthenticationService);
      console.log(router);
    }

  ngOnInit(): void {
    this.signInAsync();
  }

  async signInAsync() {
    console.log("signInAsync called");
    console.log(this.userAuthenticationService);
      await this.userAuthenticationService
        .signinRedirectCallback()
        .then(()=>{
          console.log("In signincallback, ");
          //redirect
          this.router.navigate(['/']);
        })
        .catch(function (e) {
          console.error(e);
        });
  }
  
}
