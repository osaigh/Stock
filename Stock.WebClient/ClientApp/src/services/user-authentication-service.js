import { UserManager } from "oidc-client-ts";
import { IdentityConfig, signUpUrl } from "../configuration/config";

export default class UserAuthenticationService {
  userManager;
  signinCallbackPromise = null;
  signoutCallbackPromise = null;

  constructor() {
    this.userManager = new UserManager(IdentityConfig);
  }

  signIn = () => {
    return this.userManager.signinRedirect();
  };

  signOut = () => {
    return this.userManager.signoutRedirect();
  };

  // Resolves to the signed-in user, or null when nobody is signed in or the
  // access token has expired.
  getUser = async () => {
    const user = await this.userManager.getUser();
    return user && !user.expired ? user : null;
  };

  // The authorization code in the callback URL can only be redeemed once, so
  // repeated calls (React StrictMode runs effects twice in development) share
  // one request.
  signinRedirectCallback = () => {
    if (!this.signinCallbackPromise) {
      this.signinCallbackPromise = this.userManager.signinRedirectCallback();
    }
    return this.signinCallbackPromise;
  };

  signoutRedirectCallback = () => {
    if (!this.signoutCallbackPromise) {
      this.signoutCallbackPromise = this.userManager.signoutRedirectCallback();
    }
    return this.signoutCallbackPromise;
  };

  getSignUpEndPoint() {
    return signUpUrl;
  }
}
