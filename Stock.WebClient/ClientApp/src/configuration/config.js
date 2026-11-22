import { WebStorageStateStore } from "oidc-client-ts";

const identityServerUrl = "http://localhost:44702";
const clientUrl = "http://localhost:44602";

// Authorization code flow with PKCE; endpoints come from the authority's discovery document.
export const IdentityConfig = {
  authority: identityServerUrl,
  client_id: "client_id_react",
  response_type: "code",
  redirect_uri: `${clientUrl}/signincallback`,
  post_logout_redirect_uri: `${clientUrl}/signoutcallback`,
  scope: "openid profile StockAPI",
  userStore: new WebStorageStateStore({ store: window.localStorage }),
  // oidc-client-ts renews tokens in a hidden iframe by default. The app has no
  // silent-renew callback page, so keep the old oidc-client behaviour (off).
  automaticSilentRenew: false,
};

export const signUpUrl = `${identityServerUrl}/Auth/Register?returnUrl=${encodeURIComponent(`${clientUrl}/`)}`;

export const StockAPIConfig = {
  baseURL: "http://localhost:44700/api/",
};
