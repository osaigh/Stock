import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  SidebarClassList:string = "sidebar";
  HeadingClassList:string = "title";
  SpanClassList:string = "";
  BrandClassList:string = "brand";
  sidebarStatus: Observable<{status:string}>;
  
  constructor(private store: Store<{ sidebar: {status:string} }>) { 
    this.sidebarStatus = store.pipe(select('sidebar'));
  }

  ngOnInit(): void {
    this.sidebarStatus.subscribe({
      next:(value:{status:string}) => {
        console.log(value.status);
        if(window.innerWidth < 767){
          if(value.status.length > 0){
            this.SidebarClassList= "sidebar show";
            this.HeadingClassList= "title";
            this.BrandClassList= "brand";
          }
        }else{
          if(value.status.length > 0){
            this.SidebarClassList= `sidebar ${value.status}`;
            this.HeadingClassList= "title hideheading";
            this.BrandClassList= "brand hideheading";
            this.SpanClassList = 'hidespan';
          }else{
            this.SidebarClassList= `sidebar`;
            this.HeadingClassList= "title";
            this.BrandClassList= "brand";
            this.SpanClassList = '';
          }
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
