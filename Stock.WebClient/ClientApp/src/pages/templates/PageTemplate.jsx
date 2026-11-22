import useUser from "../../hooks/useUser";
import Layout from "../../components/layouts/Layout";
import { Navigate } from "react-router-dom";
import Stack from '@mui/material/Stack';


function PageTemplate(props) {
    const { user, isLoading } = useUser();

    if(isLoading){
        return <div></div>;
    }

    if(!user){
        return <Navigate to="/login"/>;
    }

    return (
        <Layout>

            <Stack direction='column' sx={{width:'100%', height:'100%', alignItems:'center', gap:2}}>
                {props.children}
            </Stack>
        </Layout>
    )
}

export default PageTemplate;
