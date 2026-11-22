import { useState, useContext, useEffect } from "react";
import { StockApiContext } from "../../context/stock-api-context";
import useUser from "../../hooks/useUser";
import Alert from '@mui/material/Alert';
import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardContent from '@mui/material/CardContent';
import Datatable from "../../components/datatable/Datatable";
import PageTemplate from "../templates/PageTemplate";


// The API returns these three enums as integers.
const getOrderTypeString = (orderType) =>{
    switch(orderType){
        case 0: return 'Market';
        case 1: return 'Stop';
        case 2: return 'Limit';
        default: return '';
    }
}

const getTransactionTypeString = (transactionType) =>{
    switch(transactionType){
        case 0: return 'Buy';
        case 1: return 'Sell';
        default: return '';
    }
}

const getTimeInForceString = (timeInForce) =>{
    switch(timeInForce){
        case 0: return 'End of Day';
        case 1: return 'Thirty Days';
        default: return '';
    }
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
    const { user } = useUser();
    const { stockApiService } = useContext(StockApiContext);
    const [stockOrders, setStockOrders] = useState([]);
    const [loadFailed, setLoadFailed] = useState(false);

    useEffect(() => {
        if(!user){
            return undefined;
        }

        let active = true;
        stockApiService.getAllStockOrders(user.access_token).then(
            (response) => {
                if(active){
                    setStockOrders(response.data);
                    setLoadFailed(false);
                }
            },
            (error) => {
                console.error(error);
                if(active){
                    setLoadFailed(true);
                }
            }
        );

        return () => {
            active = false;
        };
      }, [user, stockApiService]);

      const tableData = getTableData(stockOrders);

      return (
        <PageTemplate>
            <Card sx={{width: "100%", }}>
                <CardHeader title="Stock Orders">

                </CardHeader>
                <CardContent>
                    {loadFailed &&
                        <Alert severity="error" sx={{marginBottom:2}}>Unable to load the stock orders.</Alert>
                    }
                    <Datatable height="400px" rowData={tableData} columns={columns}>

                    </Datatable>
                </CardContent>
            </Card>
        </PageTemplate>
      )
}

export default Orders
