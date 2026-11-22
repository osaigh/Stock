import { useState, useEffect, useContext } from "react";
import { Navigate } from "react-router-dom";
import { UserAuthenticationContext } from "../../context/user-manager-context";

export function SignOutCallback() {
  const { userAuthenticationService } = useContext(UserAuthenticationContext);
  const [isRedirecting, setIsRedirecting] = useState(false);

  useEffect(() => {
    let active = true;

    userAuthenticationService
      .signoutRedirectCallback()
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
