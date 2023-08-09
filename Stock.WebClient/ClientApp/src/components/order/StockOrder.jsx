import React, {useState,useContext,useEffect} from "react";
import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardContent from '@mui/material/CardContent';
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Radio from '@mui/material/Radio';
import RadioGroup from '@mui/material/RadioGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import FormControl from '@mui/material/FormControl';
import FormLabel from '@mui/material/FormLabel';
import MenuItem from '@mui/material/MenuItem';
import Button from '@mui/material/Button';
import { Link } from 'react-router-dom';
import { Navigate } from "react-router-dom";
import { useParams } from 'react-router-dom';
import { UserAuthenticationContext } from '../../context/user-manager-context';
import { StockApiContext } from "../../context/stock-api-context";

const orderTypes = [
    {
      value: 'Market',
      label: 'Market',
    },
    {
      value: 'Limit',
      label: 'Limit',
    },
    {
      value: 'Stop',
      label: 'Stop',
    },
  ];

const timeInForces = [
    {
        value: 'EndOfDay',
        label: 'End Of Day',
      },
      {
        value: 'ThirtyDays',
        label: 'Thirty Days',
      },
]

const getTransactionType =(transactionType) =>{
    if(transactionType){
        return transactionType;
    }
    return "buy"
}

function StockOrder() {
    const [user, setUser] = useState(null);
    const { userAuthenticationService } = useContext(UserAuthenticationContext);  
    let { stockId, transactionType } = useParams();
    const [shares,setShares] = useState("");
    const [priceLimit,setPriceLimit] = useState("");
    const [orderType,setOrderType] = useState("Market");
    const [timeInForce,setTimeInForce] = useState("EndOfDay");
    const [ftransactionType,setTransactionType] = useState(getTransactionType(transactionType));
    const [errors, setErrors] = useState(new Map());
    const { stockApiService } = useContext(StockApiContext);
    const [redirect, setRedirect] = useState(false);

    useEffect(() => {
        async function getUserAsync() {
          const user = await userAuthenticationService.getUser();
          return user;
        }
    
        getUserAsync().then((user) => {
            setUser(user);
        });
      }, []);

    let buyRadio;
    let sellRadio;

    if(ftransactionType ){
        if(ftransactionType === "buy"){
            buyRadio = (<FormControlLabel value="Buy" control={<Radio name='buy-radio' id="buy-radio" checked/>} label="Buy" />);
            sellRadio = (<FormControlLabel value="Sell" control={<Radio name='sell-radio' id="sell-radio"/>} label="Sell" />);
        }else{
            buyRadio = (<FormControlLabel value="Buy" control={<Radio name='buy-radio' id="buy-radio"/>} label="Buy" />);
            sellRadio = (<FormControlLabel value="Sell" control={<Radio checked name='sell-radio' id="sell-radio"/>} label="Sell" />);
        }
    }else{
        buyRadio = (<FormControlLabel value="Buy" control={<Radio name='buy-radio' id="buy-radio"/>} label="Buy" />);
        sellRadio = (<FormControlLabel value="Sell" control={<Radio name='sell-radio' id="sell-radio"/>} label="Sell" />);
                                
    }

    const onSubmit = (e)=> {
        e.preventDefault();

        const newErrors = new Map();
        //validation
        //shares
        if(!shares || shares === undefined){
            newErrors.set('shares', "Please enter a positive number");
        }else if(isNaN(parseInt(shares))){
            newErrors.set('shares', "Please enter a positive number");
        }
        else if(shares <= 0){
            newErrors.set('shares', "Please enter a positive number");
        }

        //price Limit
        if(!priceLimit || priceLimit === undefined){
            newErrors.set('priceLimit', "Please enter a positive number");
        }else if(isNaN(parseInt(priceLimit))){
            newErrors.set('priceLimit', "Please enter a positive number");
        }
        else if(priceLimit <= 0){
            newErrors.set('priceLimit', "Please enter a positive number");
        }

        if(newErrors.size > 0){
            setErrors(newErrors);
        }else{
            //call api
            const stockOrder = {
                stockId :stockId,
                shares: shares,
                timeInForce: timeInForce,
                transactionType: ftransactionType,
                orderType: orderType,
                stopLimitPrice: priceLimit,
            }

            const jsonString = JSON.stringify(stockOrder);
            const apiRequest = {
                requestId: "werr",
                jsonString:jsonString
            }
            const apiRequestString = JSON.stringify(apiRequest);
             try{
                if(user){
                    stockApiService.createStockOrder(user.access_token, apiRequestString);
                    setRedirect(true);
                }else{

                }
             }catch(e){
                console.log(e);
             }
        }
    }

    const onSharesChanged = (e) =>{
        setShares(e.target.value);
    }

    const onPriceLimitChanged = (e) =>{
        setPriceLimit(e.target.value);
    }

    const onOrderTypeChanged = (e) =>{
        setOrderType(e.target.value);
    }

    const onTermChanged = (e) =>{
        setTimeInForce(e.target.value);
    }

    const onTransactionTypeChanged = (e) =>{
        setTransactionType(e.target.value);
    }

  return (
    <>
        {redirect?
        <Navigate to="/orders"/>
        :
        <Card sx={{width:"100%"}}>
        <CardHeader title="Order">

        </CardHeader>

        <CardContent>
            <Box component="form"
                onSubmit={(e) => onSubmit(e)}
            >
                <Stack direction="column" gap={3}>
                    <Stack direction="row" gap={3} sx={{maxWidth:"1000px"}}>
                        <FormControl>
                            <FormLabel id="transaction-type-buttons-group-label">Transaction Type</FormLabel>
                            <RadioGroup
                                row
                                aria-labelledby="transaction-type-buttons-group-label"
                                name="transaction-type-buttons-group"
                                onChange={(e)=> onTransactionTypeChanged(e)}
                            >
                               {buyRadio}
                                {sellRadio}
                                
                            </RadioGroup>
                        </FormControl>
                    </Stack>
                    <Stack direction="row" gap={3} sx={{maxWidth:"1000px"}}>
                        <TextField
                            fullWidth
                            required
                            id="shares"
                            name="shares"
                            label="Shares"
                            value={shares}
                            onChange={(e)=> onSharesChanged(e)}
                            error={errors.has("shares")}
                            helperText={errors.has('shares')? errors.get('shares'): ""}
                            />
                        <TextField
                            fullWidth
                            required
                            name="priceLimit"
                            value={priceLimit}
                            id="priceLimit"
                            label="Price Limit"
                            onChange={(e)=> onPriceLimitChanged(e)}
                            error={errors.has("priceLimit")}
                            helperText={errors.has('priceLimit')? errors.get('priceLimit'): ""}
                            />
                    </Stack>
                    <Stack direction="row" gap={3} sx={{maxWidth:"1000px"}}>
                        <TextField
                        fullWidth
                        id="orderType"
                        name="orderType"
                        value={orderType}
                        select
                        label="Order Type"
                        
                        helperText="Please select your order type"
                        onChange={(e)=> onOrderTypeChanged(e)}
                        >
                            {orderTypes.map((option) => (
                                <MenuItem key={option.value} value={option.value}>
                                {option.label}
                                </MenuItem>
                            ))}
                        </TextField>
                        <TextField
                        fullWidth
                        id="timeInForce"
                        name="timeInForce"
                        value={timeInForce}
                        select
                        label="Term"
                        
                        helperText="Please select your term"
                        onChange={(e)=> onTermChanged(e)}
                        >
                            {timeInForces.map((option) => (
                                <MenuItem key={option.value} value={option.value}>
                                {option.label}
                                </MenuItem>
                            ))}
                        </TextField>
                    </Stack>
                    <Stack direction="row" gap={3} sx={{maxWidth:"1000px"}} alignItems="center" justifyContent="end">
                        <Button variant='contained' type='submit' >Submit</Button>
                        <Link to="/"><Button variant='outlined'>Cancel</Button></Link>
                    </Stack>
                </Stack>

            </Box>
        </CardContent>

    </Card>    
    }
    </>
  )
}

export default StockOrder