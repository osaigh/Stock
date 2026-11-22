import useUser from "../../hooks/useUser";
import LoginComponent from "../../components/login/Login";
import { Navigate } from "react-router-dom";

export default function Login(){
    const { user } = useUser();

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
