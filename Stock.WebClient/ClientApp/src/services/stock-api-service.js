import axios from "axios";

export default class StockApiService {
  baseURL;

  constructor(_baseURL) {
    this.baseURL = _baseURL;
  }

  getAllStocks = (accessToken = "") => {

      if (accessToken.length > 0) {
          const config = {
              headers: { Authorization: `Bearer ${accessToken}` }
          };
          return axios.get(this.baseURL + "stock", config);
      } else {
          return axios.get(this.baseURL + "stock");
      }
    
  };

  getAllStockPositions = (accessToken = "") => {

    if (accessToken.length > 0) {
        const config = {
            headers: { Authorization: `Bearer ${accessToken}` }
        };
        return axios.get(this.baseURL + "StockPosition", config);
    } else {
        return axios.get(this.baseURL + "StockPosition");
    }
  
};

    getAllStockOrders = (accessToken = "") => {

    if (accessToken.length > 0) {
        const config = {
            headers: { Authorization: `Bearer ${accessToken}` }
        };
        return axios.get(this.baseURL + "StockOrder", config);
    } else {
        return axios.get(this.baseURL + "StockOrder");
    }
  
};

  createStockOrder = (accessToken = "", stockOrder) =>{
    if (accessToken.length > 0) {
        const config = {
            headers: { 
                Authorization: `Bearer ${accessToken}`, 
                'Accept': 'application/json',
                'Content-Type': 'application/json' 
            }
        };
        return axios.post(this.baseURL + "StockOrder", stockOrder, config);
    } else {
        return axios.post(this.baseURL + "StockOrder", stockOrder);
    }
  }

  createStock = (newObject) => {
    return axios.post(this.baseURL, newObject);
  };

  updateStock = (id, newObject) => {
    return axios.put(`${this.baseURL}/${id}`, newObject);
  };

  getStockHistory = (stockId, accessToken = "") => {
        if (accessToken.length > 0) {
            const config = {
                headers: { Authorization: `Bearer ${accessToken}` }
            };
            return axios.get(this.baseURL + "stockHistory/" + stockId, config);
        } else {
            return axios.get(this.baseURL + "stockHistory/" + stockId);
        }
    
  };
}
