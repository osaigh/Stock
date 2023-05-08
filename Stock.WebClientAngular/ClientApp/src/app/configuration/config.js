import Oidc from "oidc-client";

export const IdentityConfig = {
  userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
  authority: "https://localhost:44701/",
  client_id: "client_id_angular",
  response_type: "id_token token",
  redirect_uri: "http://localhost:44902/signincallback",
  post_logout_redirect_uri: "http://localhost:44902/signoutcallback",
  scope: "openid  profile ApiOne ApiTwo StockAPI",
  metadata: {
    issuer: "https://localhost:44701",
    jwks_uri: "https://localhost:44701/.well-known/openid-configuration/jwks",
    authorization_endpoint: "https://localhost:44701/connect/authorize",
    token_endpoint: "https://localhost:44701/connect/token",
    userinfo_endpoint: "https://localhost:44701/connect/userinfo",
    end_session_endpoint: "https://localhost:44701/connect/endsession",
    check_session_iframe: "https://localhost:44701/connect/checksession",
    revocation_endpoint: "https://localhost:44701/connect/revocation",
    introspection_endpoint: "https://localhost:44701/connect/introspect",
    device_authorization_endpoint:
      "https://localhost:44701/connect/deviceauthorization",
  },
};

export const StockAPIConfig = {
  baseURL: "http://localhost:44700/api/",
};
