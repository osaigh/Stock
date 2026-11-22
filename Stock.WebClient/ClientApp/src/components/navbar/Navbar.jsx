import { useState, useContext } from 'react';
import "./navbar.scss"
import MenuIcon from '@mui/icons-material/Menu';
import { useDispatch } from 'react-redux';
import { toggleState } from '../../slices/sidebar-slice';
import { UserAuthenticationContext } from "../../context/user-manager-context";
import useUser from '../../hooks/useUser';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import Avatar from '@mui/material/Avatar';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';


const Navbar = () => {
    const [anchorElUser, setAnchorElUser] = useState(null);
    const dispatch = useDispatch();
    const { user } = useUser();
    const { userAuthenticationService } = useContext(UserAuthenticationContext);

    const menuClicked = (e)=>{
        e.preventDefault();
        dispatch(toggleState());
    }

    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
      };

    const handleOpenUserMenu = (event) => {
        setAnchorElUser(event.currentTarget);
      };

    const signOut = () => {
        userAuthenticationService.signOut().catch((error) => console.error(error));
    };

    const signIn = () => {
        userAuthenticationService.signIn().catch((error) => console.error(error));
    };

    let headerBar;
    if(user){
        headerBar = (
            <Box sx={{ flexGrow: 0 }}>
            <Tooltip title="Open settings">
              <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                <Avatar alt="Osa " src="/user2-160x160.jpg" />
              </IconButton>
            </Tooltip>
            <Menu
              sx={{ mt: '45px' }}
              id="menu-appbar"
              anchorEl={anchorElUser}
              anchorOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              open={Boolean(anchorElUser)}
              onClose={handleCloseUserMenu}
            >
              <MenuItem onClick={signOut}>
                  <Typography sx={{ textAlign: 'center' }}>Sign Out</Typography>
                </MenuItem>
            </Menu>
          </Box>
        );
    }else{
        headerBar = (
            <Button color="inherit" onClick={signIn}>Login</Button>
        );
    }

    return (
        <Box sx={{ flexGrow: 1 }}>
            <AppBar position="static">
            <Toolbar>
                <IconButton
                size="large"
                edge="start"
                color="inherit"
                aria-label="menu"
                sx={{ mr: 2 }}
                onClick={menuClicked}
                >
                <MenuIcon />
                </IconButton>
                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>

                </Typography>
                {headerBar}
            </Toolbar>
            </AppBar>
        </Box>
    );
};

export default Navbar;
