import { useState, useContext } from "react";
import Alert from '@mui/material/Alert';
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
import { Link, useNavigate, useParams } from 'react-router-dom';
import { UserAuthenticationContext } from '../../context/user-manager-context';
import { StockApiContext } from "../../context/stock-api-context";

// The option values are the names of the API's enums (OrderType, TimeInForce,
// TransactionType), which the API accepts in place of their numeric values.
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

// The route uses lowercase "buy"/"sell".
const getTransactionType = (routeTransactionType) => {
    return routeTransactionType?.toLowerCase() === "sell" ? "Sell" : "Buy";
}

const validate = (shares, priceLimit) => {
    const errors = new Map();

    if(!/^\d+$/.test(shares) || Number(shares) <= 0 || !Number.isSafeInteger(Number(shares))){
        errors.set('shares', "Please enter a positive whole number");
    }

    if(!/^(\d+\.?\d*|\.\d+)$/.test(priceLimit) || Number(priceLimit) <= 0){
        errors.set('priceLimit', "Please enter a positive number");
    }

    return errors;
}

const getSubmitErrorMessage = (error) => {
    if(error.response){
        return `The order was not accepted (HTTP ${error.response.status}). Please check it and try again.`;
    }
    return "The order could not be sent. Please try again.";
}

function StockOrder() {
    const { userAuthenticationService } = useContext(UserAuthenticationContext);
    const { stockApiService } = useContext(StockApiContext);
    const { stockId, transactionType: routeTransactionType } = useParams();
    const navigate = useNavigate();
    const [shares,setShares] = useState("");
    const [priceLimit,setPriceLimit] = useState("");
    const [orderType,setOrderType] = useState("Market");
    const [timeInForce,setTimeInForce] = useState("EndOfDay");
    const [transactionType,setTransactionType] = useState(() => getTransactionType(routeTransactionType));
    const [errors, setErrors] = useState(new Map());
    const [submitError, setSubmitError] = useState("");
    const [submitting, setSubmitting] = useState(false);

    const onSubmit = async (e)=> {
        e.preventDefault();

        const trimmedShares = shares.trim();
        const trimmedPriceLimit = priceLimit.trim();
        const newErrors = validate(trimmedShares, trimmedPriceLimit);
        setErrors(newErrors);
        setSubmitError("");
        if(newErrors.size > 0){
            return;
        }

        const stockOrder = {
            stockId :stockId,
            shares: Number(trimmedShares),
            timeInForce: timeInForce,
            transactionType: transactionType,
            orderType: orderType,
            stopLimitPrice: Number(trimmedPriceLimit),
        }

        const apiRequest = {
            requestId: crypto.randomUUID(),
            jsonString: JSON.stringify(stockOrder)
        }

        setSubmitting(true);
        try{
            const user = await userAuthenticationService.getUser();
            if(!user){
                setSubmitError("Your session has ended. Please sign in again.");
                setSubmitting(false);
                return;
            }
            await stockApiService.createStockOrder(user.access_token, apiRequest);
            navigate("/orders");
        }catch(error){
            console.error(error);
            setSubmitError(getSubmitErrorMessage(error));
            setSubmitting(false);
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
        <Card sx={{width:"100%"}}>
        <CardHeader title="Order">

        </CardHeader>

        <CardContent>
            <Box component="form"
                onSubmit={(e) => onSubmit(e)}
            >
                <Stack direction="column" sx={{gap:3}}>
                    <Stack direction="row" sx={{gap:3, maxWidth:"1000px"}}>
                        <FormControl>
                            <FormLabel id="transaction-type-buttons-group-label">Transaction Type</FormLabel>
                            <RadioGroup
                                row
                                aria-labelledby="transaction-type-buttons-group-label"
                                name="transaction-type-buttons-group"
                                value={transactionType}
                                onChange={(e)=> onTransactionTypeChanged(e)}
                            >
                                <FormControlLabel value="Buy" control={<Radio id="buy-radio"/>} label="Buy" />
                                <FormControlLabel value="Sell" control={<Radio id="sell-radio"/>} label="Sell" />
                            </RadioGroup>
                        </FormControl>
                    </Stack>
                    <Stack direction="row" sx={{gap:3, maxWidth:"1000px"}}>
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
                    <Stack direction="row" sx={{gap:3, maxWidth:"1000px"}}>
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
                    {submitError &&
                        <Alert severity="error" sx={{maxWidth:"1000px"}}>{submitError}</Alert>
                    }
                    <Stack direction="row" sx={{gap:3, maxWidth:"1000px", alignItems:"center", justifyContent:"end"}}>
                        <Button variant='contained' type='submit' disabled={submitting}>Submit</Button>
                        <Button variant='outlined' component={Link} to="/">Cancel</Button>
                    </Stack>
                </Stack>

            </Box>
        </CardContent>

    </Card>
  )
}

export default StockOrder
