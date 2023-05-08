import { Component, OnDestroy, OnInit } from '@angular/core';
import { observable, Subscription } from 'rxjs';
import { UserAuthenticationService } from '../services/user-authentication.service';
import { Store, select } from '@ngrx/store';
import { ToggleState, OnAppClicked } from '../statemanagement/sidebar.actions';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {
  public IsUserSignedIn: boolean = false;
  userChangedSub: Subscription;
  ContentClassList:string = "maincontent";
  sidebarStatus: Observable<{status:string}>;

  constructor(private userAuthenticationService: UserAuthenticationService,private store: Store<{ sidebar: {status:string} }>) {
    this.sidebarStatus = store.pipe(select('sidebar'));
   }

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

    this.sidebarStatus.subscribe({
      next:(value:{status:string}) => {
        if(value.status == "hide"){
          this.ContentClassList = "maincontent expand";
        }else{
          this.ContentClassList = "maincontent";
        }
      },
      error:(err:any) => {
        console.log(err);
      },
      complete: ()=>{
        console.log("complete");
      }
    });
  }

}
