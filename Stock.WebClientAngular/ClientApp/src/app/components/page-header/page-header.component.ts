import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { observable, Subscription } from 'rxjs';
import { UserInterface } from 'src/app/models';
import { UserAuthenticationService } from 'src/app/services/user-authentication.service';

@Component({
  selector: 'app-page-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.css'],
})
export class PageHeaderComponent implements OnInit, OnDestroy {
  public IsUserSignedIn: boolean = false;
  public userGreeting: string = 'Hello';
  userChangedSub: Subscription;
  constructor(private userAuthenticationService: UserAuthenticationService) {}
  
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
      console.log("IsUserSignedIn "+ this.IsUserSignedIn.toString());
      this.userGreeting = "Hello "+ user.username;
    }else{
      this.IsUserSignedIn = false;
      this.userGreeting = "";
    }
    });
  }

  signIn():void{
    this.userAuthenticationService.signIn()
  }
  signOut():void{
    this.userAuthenticationService.signOut()
  }
}
