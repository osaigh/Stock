import { useContext, useState } from 'react';
import Alert from '@mui/material/Alert';
import Stack from '@mui/material/Stack';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import { UserAuthenticationContext } from "../../context/user-manager-context";

function Login() {
    const { userAuthenticationService } = useContext(UserAuthenticationContext);
    const [signInFailed, setSignInFailed] = useState(false);

    const signIn = async () => {
        setSignInFailed(false);
        try {
            await userAuthenticationService.signIn();
        } catch (error) {
            console.error(error);
            setSignInFailed(true);
        }
    };

  return (
    <Stack
        direction="row"
        sx={{
            justifyContent:"center",
            alignItems:"center",
            width:"100vw",
            height:"100vh"
        }}
    >
        <Stack
        direction="column"
        spacing={6}
            sx={{
                justifyContent:"center",
                alignItems:"center",
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
                spacing={2}
                sx={{alignItems:"center", width:300}}>
                <Button variant="contained" size='large' fullWidth onClick={signIn}>Sign In</Button>
                <Button variant="outlined" href={userAuthenticationService.getSignUpEndPoint()} target="_blank" size='large' fullWidth>Create Account</Button>
                {signInFailed &&
                    <Alert severity="error" sx={{width:"100%"}}>Unable to reach the sign-in service.</Alert>
                }
            </Stack>

        </Stack>
    </Stack>
  )
}

export default Login
