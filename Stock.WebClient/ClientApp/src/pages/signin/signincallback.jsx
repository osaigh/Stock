import { useState, useEffect, useContext } from "react";
import { Navigate } from "react-router-dom";
import { UserAuthenticationContext } from "../../context/user-manager-context";

export function SignInCallback() {
  const [isRedirecting, setIsRedirecting] = useState(false);
  const { userAuthenticationService } = useContext(UserAuthenticationContext);

  useEffect(() => {
    let active = true;

    // On failure "/" sends the still signed-out user back to the login page.
    userAuthenticationService
      .signinRedirectCallback()
      .catch((error) => console.error(error))
      .then(() => {
        if (active) {
          setIsRedirecting(true);
        }
      });

    return () => {
      active = false;
    };
  }, [userAuthenticationService]);

  return isRedirecting ? <Navigate to="/" replace /> : <div>Redirecting</div>;
}
