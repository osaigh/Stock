import { useContext, useEffect, useState } from "react";
import { UserAuthenticationContext } from "../context/user-manager-context";

// Loads the signed-in user from the authentication service.
// `user` is null while loading and when nobody is signed in.
export default function useUser() {
  const { userAuthenticationService } = useContext(UserAuthenticationContext);
  const [state, setState] = useState({ user: null, isLoading: true });

  useEffect(() => {
    let active = true;

    userAuthenticationService.getUser().then(
      (user) => {
        if (active) {
          setState({ user, isLoading: false });
        }
      },
      () => {
        if (active) {
          setState({ user: null, isLoading: false });
        }
      }
    );

    return () => {
      active = false;
    };
  }, [userAuthenticationService]);

  return state;
}
