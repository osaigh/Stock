import { createReducer, on } from '@ngrx/store';
import { onAppClicked, toggleState } from './sidebar.actions';

export interface SidebarState {
  status: '' | 'hide';
}

export const initialState: SidebarState = { status: '' };

export const sidebarReducer = createReducer(
  initialState,
  on(toggleState, (state): SidebarState => ({
    ...state,
    status: state.status === 'hide' ? '' : 'hide',
  })),
  on(onAppClicked, (state): SidebarState => ({ ...state, status: '' })),
);
