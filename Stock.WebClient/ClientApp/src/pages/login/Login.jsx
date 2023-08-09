import React, {useState,useContext,useEffect} from "react";
import { UserAuthenticationContext } from "../../context/user-manager-context";
import LoginComponent from "../../components/login/Login";
import { Navigate } from "react-router-dom";

export default function Login(){
    const [user, setUser] = useState(null);
    const { userAuthenticationService } = useContext(UserAuthenticationContext);  

    useEffect(() => {
        async function getUserAsync() {
          const user = await userAuthenticationService.getUser();
          return user;
        }
    
        getUserAsync().then((user) => {
          setUser(user);
        });
      }, []);

    return (
      <>
      {user ?
        <Navigate to="/" />
        :
        <LoginComponent/>
        }
    </>
    );
}