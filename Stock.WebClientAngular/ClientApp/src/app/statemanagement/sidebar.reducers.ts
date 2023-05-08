import { Action } from '@ngrx/store';
import { ActionTypes } from './sidebar.actions';

export const initialState= {status:""};

export function sidebarReducer(state = initialState, action: Action) {
    const newState = {...state};
    switch (action.type) {
      case ActionTypes.ToggleState:
        if(newState.status == "hide"){
            newState.status = "";
        }else{
            newState.status = "hide";
        }

        return newState;
   
      case ActionTypes.OnAppClicked:
        newState.status = "";
        return newState;
   
      default:
        return newState;
    }
  }