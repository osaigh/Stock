import { onAppClicked, toggleState } from './sidebar.actions';
import { initialState, sidebarReducer } from './sidebar.reducers';

describe('sidebarReducer', () => {
  it('should return the initial state for an unknown action', () => {
    expect(sidebarReducer(undefined, { type: 'unknown' })).toEqual({ status: '' });
  });

  it('should toggle between hidden and shown', () => {
    const hidden = sidebarReducer(initialState, toggleState());
    expect(hidden).toEqual({ status: 'hide' });

    expect(sidebarReducer(hidden, toggleState())).toEqual({ status: '' });
  });

  it('should show the sidebar when the app is clicked', () => {
    expect(sidebarReducer({ status: 'hide' }, onAppClicked())).toEqual({ status: '' });
  });

  it('should not mutate the previous state', () => {
    const state = Object.freeze({ ...initialState });

    expect(sidebarReducer(state, toggleState())).not.toBe(state);
    expect(state).toEqual({ status: '' });
  });
});
