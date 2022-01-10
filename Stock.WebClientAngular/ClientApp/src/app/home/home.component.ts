import { Component, OnDestroy, OnInit } from '@angular/core';
import { observable, Subscription } from 'rxjs';
import { UserAuthenticationService } from '../services/user-authentication.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {
  public IsUserSignedIn: boolean = false;
  userChangedSub: Subscription;
  constructor(private userAuthenticationService: UserAuthenticationService) { }

  ngOnDestroy(): void {
    this.userChangedSub.unsubscribe();
  }

  ngOnInit(): void {
    console.log("ngOnInit");
    this.userChangedSub =this.userAuthenticationService.userChanged.subscribe((user)=>{
      console.log("onUserChanged called");
    console.log(user);
    if(user){
      this.IsUserSignedIn = true;
    }else{
      this.IsUserSignedIn = false;
    }
    });
  }

}
