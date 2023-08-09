import React, {useState,useContext,useEffect} from "react";
import { UserAuthenticationContext } from "../../context/user-manager-context";
import Layout from "../../components/layouts/Layout";
import { Navigate } from "react-router-dom";
import Stack from '@mui/material/Stack';



function PageTemplate(props) {
    const [user, setUser] = useState(null);
    const [redirect, setRedirect] = useState(false);
    const { userAuthenticationService } = useContext(UserAuthenticationContext);  


    useEffect(() => {
        async function getUserAsync() {
          const user = await userAuthenticationService.getUser();
          return user;
        }
    
        getUserAsync().then((user) => {
          if(user){
            setUser(user);
          }else{
            setRedirect(true);
          }
        });
      }, []);


      return (
        <>
            {!user ?
                <>
                    {redirect ? 
                        <Navigate to="login"/>: <div></div>
                    }
                </>
                :
                <Layout>
                    
                    <Stack sx={{width:'100%', height:'100%'}} direction='column' alignItems='center' gap={2}>
                        {props.children}
                    </Stack>
                </Layout>
            }
        </>
      )
}

export default PageTemplate;