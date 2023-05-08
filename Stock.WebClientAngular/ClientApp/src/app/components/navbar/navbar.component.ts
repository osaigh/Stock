import { Component, OnInit } from '@angular/core';
import { UserInterface } from '../../models';
import { observable, Subscription } from 'rxjs';
import { UserAuthenticationService } from '../../services/user-authentication.service';
import { Store, select } from '@ngrx/store';
import { ToggleState, OnAppClicked } from '../../statemanagement/sidebar.actions';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  public IsUserSignedIn: boolean = false;
  userChangedSub: Subscription;
  constructor(private userAuthenticationService: UserAuthenticationService, private store: Store<{ sidebar: {status:string} }>) {

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

  ngOnDestroy(): void {
    this.userChangedSub.unsubscribe();
  }

  toggleState() {
    
    this.store.dispatch(new ToggleState());
  }

  signIn():void{
    this.userAuthenticationService.signIn()
  }
  
  signOut():void{
    this.userAuthenticationService.signOut()
  }
}
