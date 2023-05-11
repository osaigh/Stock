import { Injectable } from '@angular/core';
import { StockAPIConfig } from "../configuration/config";
import axios from "axios";
import { Stock, StockHistory } from '../models';
import { id } from '@swimlane/ngx-charts';
import { UserAuthenticationService } from './user-authentication.service';

@Injectable({
  providedIn: 'root'
})
export class StockApiService {
  baseURL;
  constructor(private userAuthenticationService: UserAuthenticationService) {
    this.baseURL = StockAPIConfig.baseURL;
  }

  async getAllStocks(): Promise<Stock[]>{
    let _user = await this.userAuthenticationService.getUserIdentity();
    
    let stocks: Stock[] = [];
    if (_user && _user.access_token) {
      const config = {
        headers: { Authorization: `Bearer ${_user.access_token}` }
      };
      
      const data = await axios.get(this.baseURL + "stock", config).then((response) => response.data);
      
      stocks = data.map((input) => {
        let stock: Stock = {
          id: input["id"] as number,
          volume: input["volume"] as number,
          price: Math.round(input["price"] as number),
          name: input["name"]
        }
        return stock;
      });
    }

    return stocks;
  };

  create(newObject){
    return axios.post(this.baseURL, newObject);
  };

  update(id, newObject){
    return axios.put(`${this.baseURL}/${id}`, newObject);
  };

  async getStockHistory(stockId): Promise<StockHistory[]>{
    let _user = await this.userAuthenticationService.getUserIdentity();

    let stockHistoryCollection: StockHistory[] = [];

    if (_user && _user.access_token) {
      const config = {
        headers: { Authorization: `Bearer ${_user.access_token}` }
      };

      const data = await axios.get(this.baseURL + "stockHistory/" + stockId, config).then((response) => response.data);
      console.log(data);
      stockHistoryCollection = data.map((input) => {
        let stockHistory: StockHistory = {
          id: input["id"] as number,
          date: input["date"],
          price: Math.round(input["price"] as number),
          stockId: input["stockId"] as number
        }
        return stockHistory;
      });
    }

    return stockHistoryCollection;
  };
}
