import axios from "axios";

const authorization = (accessToken) => ({
  headers: { Authorization: `Bearer ${accessToken}` },
});

export default class StockApiService {
  client;

  constructor(baseURL) {
    this.client = axios.create({ baseURL });
  }

  getAllStockPositions = (accessToken) => {
    return this.client.get("StockPosition", authorization(accessToken));
  };

  getAllStockOrders = (accessToken) => {
    return this.client.get("StockOrder", authorization(accessToken));
  };

  // stockOrderRequest is { requestId, jsonString }; axios serialises it as JSON.
  createStockOrder = (accessToken, stockOrderRequest) => {
    return this.client.post("StockOrder", stockOrderRequest, authorization(accessToken));
  };

  getStockHistory = (stockId, accessToken) => {
    return this.client.get(
      `StockHistory/${encodeURIComponent(stockId)}`,
      authorization(accessToken)
    );
  };
}
