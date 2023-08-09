import React ,{useContext} from 'react';
import Stack from '@mui/material/Stack';
import Button from '@mui/material/Button';
import {Typography } from '@mui/material';
import { UserAuthenticationContext } from "../../context/user-manager-context";

function Login() {
    const { userAuthenticationService } = useContext(UserAuthenticationContext);  
    const signIn = function () {
        userAuthenticationService.signIn();
    };

  return (
    <Stack
        direction="row"
        justifyContent="center"
        alignItems="center"
        sx={{
            width:"100vw",
            height:"100vh"
        }}
    >
        <Stack
        direction="column"
        justifyContent="center"
        alignItems="center"
        spacing={6}
            sx={{
                marginLeft:"auto",
                marginRight:"auto",
                width: 400,
                height: 300,
                borderRadius:"10px",
                border:"solid 0.5px rgba(100,100,100,0.4)",
                boxShadow:"2px 2px 10px 1px rgb(185, 183, 183)"
            }}
        >
            <Typography variant="h4" component="div"
                sx={{
                    marginTop:1
                }}
            >
                    Stock Trader
            </Typography>
            <Stack 
                direction="column"
                alignItems="center"
                spacing={2}
                sx={{width:300}}>
                <Button variant="contained" size='large' fullWidth onClick={() => {signIn();}}>Sign In</Button> 
                <Button variant="outlined" href={userAuthenticationService.getSignUpEndPoint()} target="_blank" size='large' fullWidth>Create Account</Button>
            </Stack>
            
        </Stack>
    </Stack>
  )
}

export default Login