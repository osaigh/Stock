import React, {useState,useContext,useEffect} from "react";
import "./home.scss"
import Sidebar from '../../components/sidebar/Sidebar';
import Navbar from '../../components/navbar/Navbar';
import classNames from 'classnames';
import { useSelector } from 'react-redux';
import { useDispatch } from "react-redux";
import { onAppClicked } from "../../slices/sidebar-slice";
import useWindowSize from '../../hooks/windowHook';
import { UserAuthenticationContext } from "../../context/user-manager-context";
import { StockContainer } from "../../components/stockcontainer/Stockcontainer";
import Stack from '@mui/material/Stack';
import Button from '@mui/material/Button';
import {Typography } from '@mui/material';
import Login from "../../components/login/Login";
import { Navigate } from "react-router-dom";

export default function Home(){
    const status = useSelector(state => state.sidebar.status);
    const [user, setUser] = useState(null);
    const { userAuthenticationService } = useContext(UserAuthenticationContext);  
    const dispatch = useDispatch();
    const windowSize = useWindowSize();

    useEffect(() => {
        async function getUserAsync() {
          const user = await userAuthenticationService.getUser();
          return user;
        }
    
        getUserAsync().then((user) => {
          setUser(user);
        });
      }, []);


    const signIn = function () {
        userAuthenticationService.signIn();
    };

    const onClicked = (event) =>{
      if(windowSize.width < 767){
        dispatch(onAppClicked(new Date().getTime()));
      }
    }

    let contentClassList = classNames("homeContainer","maincontent");
    if(status == "hide"){
        contentClassList = classNames("homeContainer","maincontent","expand");
    }

    console.log(user);
    return (
      <>
      {user ?
        <Navigate to="/" />
        :
        <Login/>
        }
    </>
    );
}
/* 
<div className='home'>
            <Sidebar/>
            <div className={contentClassList} >
                <Navbar/>
                <div className="content">

                {(user)? <StockContainer />: <div style={{marginLeft:"auto",marginRight:"auto",
                display:"flex", alignItems:"center", justifyContent:"center", height:"300px"}}><h6>Please Sign In</h6></div>}
                
                </div>
            </div>
        </div> */