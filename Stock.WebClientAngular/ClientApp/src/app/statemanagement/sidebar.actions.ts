import { Action } from '@ngrx/store';

export enum ActionTypes {
    ToggleState = '[App Component] ToggleState',
    OnAppClicked = '[App Component] OnAppClicked',
  }
   
  export class ToggleState implements Action {
    readonly type = ActionTypes.ToggleState;
  }
   
  export class OnAppClicked implements Action {
    readonly type = ActionTypes.OnAppClicked;
  }