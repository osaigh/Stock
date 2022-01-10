import { Injectable } from '@angular/core';
import { UserManager, Log } from "oidc-client";
import { IdentityConfig } from "../configuration/config";
import { BehaviorSubject, fromEvent, interval, merge } from 'rxjs';
import { UserInterface } from '../models';

@Injectable({
  providedIn: 'root'
})
export class UserAuthenticationService {
  UserManager;
  currentuser;
  userChanged:BehaviorSubject<UserInterface> = new BehaviorSubject<UserInterface>(null);

  constructor() {
    this.UserManager = new UserManager(IdentityConfig);
    Log.logger = console;
    Log.level = Log.DEBUG;
    this.currentuser = null;
    this.UserManager.events.addUserLoaded(this.userLoadedListener);
    this.UserManager.events.addUserUnloaded(this.userUnloadedListener);
    this.UserManager.events.addUserSignedOut(this.userSignedOutListener);
  }

  signIn = function () {
    //alert("SignIn called");
    this.UserManager.signinRedirect();
  };

  signOut = function () {
    alert("SignOut called");
    this.currentuser = null;
    this.userChanged.next(null);
    this.UserManager.signoutRedirect();
  };

  userLoadedListener = (user) => {
    console.log("User signed in");
    console.log(user);
    this.currentuser = user;
    let _user :UserInterface={
      username:user["profile"]["name"]
    };
    this.userChanged.next(_user);
  };

  userUnloadedListener = () => {
    console.log("userUnloadedListener User signed out");
    this.currentuser = null;
    this.userChanged.next(null);
  };

  userSignedOutListener = () => {
    this.currentuser = null;
    console.log("userSignedOutListener User signed out");
  };

  getUser = async () => {
    const user = await this.UserManager.getUser();
    this.currentuser = user;
    let _user :UserInterface={
      username:user["profile"]["name"]
    };
    this.userChanged.next(_user);
    return user;
  };

  signinRedirectCallback = async () => {
    await this.UserManager.signinRedirectCallback();
  };

  signoutRedirectCallback = async () => {
    await this.UserManager.signoutRedirectCallback();
  };

  getCurrentUser() {
    return this.currentuser;
  }
}
