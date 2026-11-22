import { UserManagerSettings, WebStorageStateStore } from 'oidc-client-ts';

export const IdentityConfig: UserManagerSettings = {
  userStore: new WebStorageStateStore({ store: window.localStorage }),
  authority: 'http://localhost:44702',
  client_id: 'client_id_angular',
  response_type: 'code',
  redirect_uri: 'http://localhost:44902/signincallback',
  post_logout_redirect_uri: 'http://localhost:44902/signoutcallback',
  scope: 'openid profile StockAPI',
  // Same behaviour as the previous oidc-client defaults: the profile claims
  // (name) come from the userinfo endpoint and there is no silent renewal.
  loadUserInfo: true,
  automaticSilentRenew: false,
};

export const StockAPIConfig = {
  baseURL: 'http://localhost:44700/api/',
};
