import React, {useState,useContext,useEffect} from "react";
import { UserAuthenticationContext } from "../../context/user-manager-context";
import { StockApiContext } from "../../context/stock-api-context";
import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardContent from '@mui/material/CardContent';
import Chart from "../../components/chart/Chart";
import moment from "moment";
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import { Link } from "react-router-dom";
import Datatable from "../../components/datatable/Datatable";
import PageTemplate from "../templates/PageTemplate";

const getTableData = (stockPositions) =>{
    if(!stockPositions){
        return [];
    }

    const tableData = [];
    stockPositions.forEach((stock, index) => {
        tableData.push(
            {
                id: index+1,
                stockId: stock.stockId,
                shares: stock.shares,
                currentPrice: `$${Math.round(stock.currentPrice * 100)/100}`,
                costBasis: `$${Math.round(stock.costBasis * 100)/100}`,
                marketValue: `$${Math.round(stock.marketValue * 100)/100}`,
                gainLossPercent:Math.round(stock.gainLossPercent * 100)/100,
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
      field: "shares",
      headerName: "Shares",
      width: 100,
    },
  
    {
      field: "currentPrice",
      headerName: "Last",
      width: 150,
    },
    {
      field: "costBasis",
      headerName: "Cost Basis",
      width: 200,
    },
    {
      field: "marketValue",
      headerName: "Market Value",
      width: 200,
    },
    {
      field: "gainLossPercent",
      headerName: "Gain/Loss %",
      width: 200,
      renderCell: (params) => {
        return (
          <div
            style={{
                padding:"5px",
                borderRadius:"5px",
                backgroundColor: (params.row.gainLossPercent > 0 ? `rgba(0, 128, 0, 0.05)`: (params.row.gainLossPercent === 0? `rgba(255, 217, 0, 0.05)`: `rgba(255, 0, 0, 0.05)`)),
                color: (params.row.gainLossPercent > 0? `green`: (params.row.gainLossPercent === 0 ? `goldenrod`: `crimson`))
            }}
          >
            {params.row.gainLossPercent}
          </div>
        );
      },
    },
  ];

const getChart = (lineChartData) =>{
    let chart = (<div></div>);
    let data = [];
    if (lineChartData != null && lineChartData.data.length > 0) {
        let chartLabels = [];
        let chartSeries = [];
        let max = 0;
        let min = 200000000;
      lineChartData.data.forEach((stockHistory) => {
        chartLabels.push(moment(stockHistory.date).format("L"));
          chartSeries.push(stockHistory.price);
          data.push({
              date: moment(stockHistory.date).format("L"),
              price: stockHistory.price
          });
        if (stockHistory.price > max) {
          max = stockHistory.price;
        }
        if (stockHistory.price < min) {
          min = stockHistory.price;
        }
      });
  
      max = Math.ceil(max);
      min = Math.floor(min) - 10;
      if (min < 0) {
        min = 0;
      }
      chart = (
        <Card sx={{width: "100%", height:"330px"}}>
          <CardHeader title={lineChartData.title}>
        
          </CardHeader>
          <CardContent>
                  <Chart aspect={2 / 1} data={data} />
          </CardContent>
        </Card>
      );

    }

    return chart;
}

function Stocks() {
    const [user, setUser] = useState(null);
    const { userAuthenticationService } = useContext(UserAuthenticationContext);  
    const [lineChartData, setLineChartData] = useState({ title: "", data: [] });
    const [stockPositions, setStockPositions] = useState({user:null, stockPositionData:[]});
    const { stockApiService } = useContext(StockApiContext);

      useEffect(() => {
        const interval = setInterval(async () => {
          let _user =await userAuthenticationService.getUserIdentity();
          if(_user){
              stockApiService.getAllStockPositions(_user.access_token).then((response) => {
                  setStockPositions({ user: _user, stockPositionData: response.data });
              });
          }
          
        }, 5000);
    
        return () => clearInterval(interval);
      }, [stockPositions]);

      const viewHistory = (stockId) => {
        if (stockPositions.user == null) {
          return;
         }
    
        var stock = null;
        stockPositions.stockPositionData.forEach((st) => {
          if (st.stockId == stockId) {
            stock = st;
          }
        });
    
        if (stock == null) {
          return;
        }
    
        stockApiService.getStockHistory(stockId,stockPositions.user.access_token).then((response) => {
          var stockHistoryCollection = response.data;
          var newLineChartData = {
            title: stock.stockId,
            data: stockHistoryCollection,
          };
          setLineChartData(newLineChartData);
        });
      };

      const actionColumn = [
        {
          field: "action",
          headerName: "Action",
          width: 220,
          renderCell: (params) => {
            return (
              <Stack direction="row" alignItems="center" gap={2}>
                <Link to={`/order/${params.row.stockId}/buy`} style={{ textDecoration: "none" }}>
                    <Box sx={{
                        padding: '2px 5px',
                        borderRadius: '5px',
                        color: 'darkblue',
                        border: '1px dotted rgba(0, 0, 139, 0.596)',
                        cursor: 'pointer',
                    }}>
                        Buy
                    </Box>
                </Link>
                <Link to={`/order/${params.row.stockId}/sell`} style={{ textDecoration: "none" }}>
                    <Box sx={{
                            padding: '2px 5px',
                            borderRadius: '5px',
                            color: 'crimson',
                            border: '1px dotted rgba(220, 20, 60, 0.6)',
                            cursor: 'pointer',
                        }}>
                        Sell
                    </Box>
                </Link>
                <Box>
                    <Box onClick={()=> viewHistory(params.row.stockId)} sx={{
                            padding: '2px 5px',
                            borderRadius: '5px',
                            color: 'gray',
                            border: '1px dotted gray',
                            cursor: 'pointer',
                        }}>
                        View History
                    </Box> 
                </Box>
              </Stack>
            );
          },
        },
      ];
    
      const tableData =  getTableData(stockPositions.stockPositionData);
      
      const chart = getChart(lineChartData);
     
  return (
    <PageTemplate>
        <Card sx={{width: "100%", }}>
          <CardHeader title="Stock Positions">
        
          </CardHeader>
          <CardContent>
            <Datatable height="400px" rowData={tableData} columns={columns.concat(actionColumn)}>

            </Datatable>
          </CardContent>
        </Card>
        
        {chart}
    </PageTemplate>
  )
}

export default Stocks