import React, {useState,useContext,useEffect} from "react";
import { UserAuthenticationContext } from "../../context/user-manager-context";
import { StockApiContext } from "../../context/stock-api-context";
import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardContent from '@mui/material/CardContent';
import Datatable from "../../components/datatable/Datatable";
import PageTemplate from "../templates/PageTemplate";



const getOrderTypeString = (orderType) =>{
    switch(orderType){
        case 0: return 'Market';
        case 1: return 'Stop';
        case 2: return 'Limit'
    }

    return '';
}

const getTransactionTypeString = (transactionType) =>{
    switch(transactionType){
        case 0: return 'Buy';
        case 1: return 'Sell';
    }

    return '';
}

const getTimeInForceString = (timeInForce) =>{
    switch(timeInForce){
        case 0: return 'End of Day';
        case 1: return 'Thirty Days';
    }

    return '';
}

const getTableData = (stockOrders) =>{
    if(!stockOrders){
        return [];
    }

    const tableData = [];
    stockOrders.forEach((stockOrder, index) => {
        tableData.push(
            {
                id: index+1,
                stockId: stockOrder.stockId,
                transactionType: getTransactionTypeString(stockOrder.transactionType),
                shares: stockOrder.shares,
                priceLimit: `$${stockOrder.stopLimitPrice}`,
                orderType: getOrderTypeString(stockOrder.orderType),
                timeInForce: getTimeInForceString(stockOrder.timeInForce),
            }
        );
    });

    return tableData;
}

const columns = [
    { field: "id", headerName: "ID", width: 70 },
    {
      field: "stockId",
      headerName: "Stock",
      width: 100,
    },
    {
      field: "transactionType",
      headerName: "Transaction",
      width: 100,
    },
    {
        field: "shares",
        headerName: "Shares",
        width: 100,
      },
    {
      field: "priceLimit",
      headerName: "Price Limit",
      width: 150,
    },
    {
      field: "orderType",
      headerName: "Order Type",
      width: 200,
    },
    {
      field: "timeInForce",
      headerName: "Term",
      width: 200,
    },
  ];

function Orders() {
    const [user, setUser] = useState(null);
    const { userAuthenticationService } = useContext(UserAuthenticationContext);  
    const [stockOrders, setStockOrders] = useState([]);
    const { stockApiService } = useContext(StockApiContext);

    useEffect(() => {
        userAuthenticationService.getUserIdentity()
            .then(user => {
                if(user){
                    stockApiService.getAllStockOrders(user.access_token).then((response) => {
                        setStockOrders(response.data);
                        console.log(response.data);
                    });
                }
            })
            .catch(error => console.log(error));
      }, []);
      console.log(stockOrders);
      const tableData = getTableData(stockOrders);
    
      return (
        <PageTemplate>
            <Card sx={{width: "100%", }}>
                <CardHeader title="Stock Orders">
                
                </CardHeader>
                <CardContent>
                    <Datatable height="400px" rowData={tableData} columns={columns}>

                    </Datatable>
                </CardContent>
            </Card>
        </PageTemplate>
      )
}

export default Orders