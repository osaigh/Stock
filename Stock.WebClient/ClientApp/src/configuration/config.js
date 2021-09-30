import Oidc from "oidc-client";

export const IdentityConfig = {
  userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
    authority: "https://localhost:44376/",
  client_id: "client_id_react",
  response_type: "id_token token",
    redirect_uri: "https://localhost:44321/signincallback",
    post_logout_redirect_uri: "https://localhost:44321/signoutcallback",
  scope: "openid  profile ApiOne ApiTwo StockAPI",
  metadata: {
      issuer: "https://localhost:44376",
      jwks_uri: "https://localhost:44376/.well-known/openid-configuration/jwks",
      authorization_endpoint: "https://localhost:44376/connect/authorize",
      token_endpoint: "https://localhost:44376/connect/token",
      userinfo_endpoint: "https://localhost:44376/connect/userinfo",
      end_session_endpoint: "https://localhost:44376/connect/endsession",
      check_session_iframe: "https://localhost:44376/connect/checksession",
      revocation_endpoint: "https://localhost:44376/connect/revocation",
      introspection_endpoint: "https://localhost:44376/connect/introspect",
    device_authorization_endpoint:
      "https://localhost:44376/connect/deviceauthorization",
  },
};

export const StockAPIConfig = {
  baseURL: "https://localhost:44368/api/",
};
