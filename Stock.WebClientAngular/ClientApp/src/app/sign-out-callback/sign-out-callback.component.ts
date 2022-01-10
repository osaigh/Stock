import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserAuthenticationService } from '../services/user-authentication.service';

@Component({
  selector: 'app-sign-out-callback',
  templateUrl: './sign-out-callback.component.html',
  styleUrls: ['./sign-out-callback.component.css']
})
export class SignOutCallbackComponent implements OnInit {

  constructor(
    private userAuthenticationService: UserAuthenticationService,
    private router: Router) { }

  ngOnInit(): void {
    this.signOutAsync();
  }

  async signOutAsync() {
      await this.userAuthenticationService
        .signoutRedirectCallback()
        .then(()=>{
          console.log("In signoutcallback, ");
          //redirect
          this.router.navigate(['/']);
        })
        .catch(function (e) {
          console.error(e);
        });
  }

}
