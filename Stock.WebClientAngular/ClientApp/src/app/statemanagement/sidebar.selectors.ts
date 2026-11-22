import { createFeatureSelector, createSelector } from '@ngrx/store';
import { SidebarState } from './sidebar.reducers';

export const selectSidebar = createFeatureSelector<SidebarState>('sidebar');

export const selectSidebarStatus = createSelector(selectSidebar, (state) => state.status);
