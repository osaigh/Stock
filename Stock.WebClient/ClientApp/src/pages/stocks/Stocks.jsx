import { useState, useContext, useEffect, useMemo, useCallback, useRef } from "react";
import { StockApiContext } from "../../context/stock-api-context";
import useUser from "../../hooks/useUser";
import Alert from '@mui/material/Alert';
import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardContent from '@mui/material/CardContent';
import Chart from "../../components/chart/Chart";
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import { Link } from "react-router-dom";
import Datatable from "../../components/datatable/Datatable";
import PageTemplate from "../templates/PageTemplate";

const refreshIntervalMs = 5000;

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
      display: "flex",
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

const chartDateFormat = new Intl.DateTimeFormat("en-US", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
});

// Formats an API date as MM/DD/YYYY in local time.
const formatChartDate = (value) => {
    // Date parses a date without a time ("2024-01-31") as UTC, which can shift it by a day.
    const dateOnly = /^(\d{4})-(\d{2})-(\d{2})$/.exec(value);
    const date = dateOnly
        ? new Date(Number(dateOnly[1]), Number(dateOnly[2]) - 1, Number(dateOnly[3]))
        : new Date(value);

    return Number.isNaN(date.getTime()) ? String(value) : chartDateFormat.format(date);
}

const getChart = (lineChartData) =>{
    if (lineChartData.data.length === 0) {
        return (<div></div>);
    }

    const data = lineChartData.data.map((stockHistory) => ({
        date: formatChartDate(stockHistory.date),
        price: stockHistory.price
    }));

    return (
        <Card sx={{width: "100%", height:"330px"}}>
          <CardHeader title={lineChartData.title}>

          </CardHeader>
          <CardContent>
                  <Chart data={data} />
          </CardContent>
        </Card>
    );
}

function Stocks() {
    const { user } = useUser();
    const { stockApiService } = useContext(StockApiContext);
    const [lineChartData, setLineChartData] = useState({ title: "", data: [] });
    const [stockPositions, setStockPositions] = useState([]);
    const [errorMessage, setErrorMessage] = useState("");
    const latestHistoryRequest = useRef(0);

      useEffect(() => {
        if(!user){
            return undefined;
        }

        let active = true;

        const loadStockPositions = async () => {
            try{
                const response = await stockApiService.getAllStockPositions(user.access_token);
                if(active){
                    setStockPositions(response.data);
                    setErrorMessage("");
                }
            }catch{
                // Not logged: this runs every few seconds and the browser already reports the failed request.
                if(active){
                    setErrorMessage("Unable to load the stock positions.");
                }
            }
        };

        loadStockPositions();
        const interval = setInterval(loadStockPositions, refreshIntervalMs);

        return () => {
            active = false;
            clearInterval(interval);
        };
      }, [user, stockApiService]);

      const viewHistory = useCallback(async (stockId) => {
        if (!user) {
          return;
        }

        // Only the most recently requested history is shown.
        const request = ++latestHistoryRequest.current;
        try{
            const response = await stockApiService.getStockHistory(stockId, user.access_token);
            if(request === latestHistoryRequest.current){
                setLineChartData({
                    title: stockId,
                    data: response.data,
                });
            }
        }catch(error){
            console.error(error);
            if(request === latestHistoryRequest.current){
                setErrorMessage(`Unable to load the price history for ${stockId}.`);
            }
        }
      }, [user, stockApiService]);

      const allColumns = useMemo(() => columns.concat([
        {
          field: "action",
          headerName: "Action",
          width: 220,
          display: "flex",
          renderCell: (params) => {
            return (
              <Stack direction="row" sx={{alignItems:"center", gap:2}}>
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
      ]), [viewHistory]);

      const tableData = useMemo(() => getTableData(stockPositions), [stockPositions]);

      const chart = getChart(lineChartData);

  return (
    <PageTemplate>
        <Card sx={{width: "100%", }}>
          <CardHeader title="Stock Positions">

          </CardHeader>
          <CardContent>
            {errorMessage &&
                <Alert severity="error" sx={{marginBottom:2}}>{errorMessage}</Alert>
            }
            <Datatable height="400px" rowData={tableData} columns={allColumns}>

            </Datatable>
          </CardContent>
        </Card>

        {chart}
    </PageTemplate>
  )
}

export default Stocks
