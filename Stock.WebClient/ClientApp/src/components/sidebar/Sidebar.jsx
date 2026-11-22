import "./sidebar.scss"
import { useSelector } from 'react-redux';
import { Link } from "react-router-dom";
import useWindowSize from '../../hooks/windowHook';
import Box from '@mui/material/Box';
import ListSubheader from '@mui/material/ListSubheader';
import List from '@mui/material/List';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import SellIcon from '@mui/icons-material/Sell';
import AccountBalanceIcon from '@mui/icons-material/AccountBalance';

const Sidebar = () => {
    const status = useSelector(state => state.sidebar.status);
    const windowSize  = useWindowSize();

    const brandLinkStyle = {
        height: '64px',
        display: 'flex',
        alignItems: 'center',
        fontWeight: '300',
        justifyContent: 'center',
        overflow: 'hidden',
        textDecoration: 'none',
        color: 'white',
        fontSize: '20px',
        borderBottom: '1px solid rgb(200,200,200)'
    };
    const sideBarContainerStyle = {
        position: 'fixed',
        top:'0',
        left: '0',
        width: '280px',
        zIndex: '1000',
        borderRight: '0.5px solid rgb(230,227,227)',
        minHeight: '100vh',
        backgroundColor: '#006AC3',
        transition: '.7s ease',
        boxShadow: '2px 2px 10px 1px rgb(185, 183, 183)',
        // Without this the link text spills out of the collapsed sidebar and
        // stays clickable on top of the page content.
        overflow: 'hidden',
    };
    const subHeadingStyle = {
        backgroundColor:'transparent',
         color:'white',
         visibility: 'visible'
    };

    const iconStyle = {
        color: 'white',
        visibility: 'visible',
    };
    const listItemStyle = {
        '&:hover':{
            backgroundColor: 'rgba(255,255,255,0.2)',
        }
    }

    // MUI 9 narrowed the default icon column from 56px to 36px; keep the original spacing.
    const listItemIconStyle = {
        minWidth: '56px',
    };

    const listItemTextStyle = {
        color: 'white',
        marginLeft:"0",
        paddingTop:'1px'
    }

    // The toolbar button toggles the status: on a wide window that collapses the
    // sidebar to its icons, on a narrow one it reveals the otherwise hidden sidebar.
    const toggled = status === 'hide';

    if(windowSize.width < 767){
        if(!toggled){
            subHeadingStyle.visibility= 'collapse';
            iconStyle.visibility = 'collapse';
            brandLinkStyle.visibility = 'collapse';
            sideBarContainerStyle.width = '0px';
        }
    }else if(toggled){
        subHeadingStyle.visibility= 'collapse';
        listItemTextStyle.display = 'none';
        brandLinkStyle.visibility = 'collapse';
        sideBarContainerStyle.width = '60px';
    }

    return (
        <Box sx={sideBarContainerStyle}>
            <Box ><Link  to="/" style={brandLinkStyle}>Stock Trader</Link></Box>

            <Box >
                <List
                    component="nav"
                    aria-labelledby="nested-list-subheader"
                    subheader={
                      <ListSubheader sx={subHeadingStyle} component="div" id="nested-list-subheader">
                        Main
                      </ListSubheader>
                    }
                >
                    <ListItemButton component={Link} to="/" sx={listItemStyle}>
                        <ListItemIcon sx={listItemIconStyle}>
                            <AccountBalanceIcon sx={iconStyle}/>
                        </ListItemIcon >
                        <ListItemText sx={listItemTextStyle}  primary="Stocks" />
                    </ListItemButton>
                    <ListItemButton component={Link} to="/orders" sx={listItemStyle}>
                        <ListItemIcon sx={listItemIconStyle}>
                            <SellIcon sx={iconStyle}/>
                        </ListItemIcon >
                        <ListItemText sx={listItemTextStyle} primary="Orders" />
                    </ListItemButton>

                </List>
            </Box>

        </Box>
    );
};

export default Sidebar;
