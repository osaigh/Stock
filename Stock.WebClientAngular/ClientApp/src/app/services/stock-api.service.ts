import { Injectable } from '@angular/core';
import { StockAPIConfig } from "../configuration/config";
import axios from "axios";
import { Stock, StockHistory } from '../models';
import { id } from '@swimlane/ngx-charts';

@Injectable({
  providedIn: 'root'
})
export class StockApiService {
  baseURL;
  constructor() {
    this.baseURL = StockAPIConfig.baseURL;
  }

  async getAllStocks():Promise<Stock[]>{
    const data = await axios.get(this.baseURL + "stock").then((response)=> response.data);
    console.log(data);
    let stocks:Stock[] = data.map((input)=>{
      let stock: Stock ={
        id:input["id"] as number,
        volume: input["volume"] as number,
        price: Math.round(input["price"] as number),
        name: input["name"]
      }
      return stock;
    });

    return stocks;
  };

  create(newObject){
    return axios.post(this.baseURL, newObject);
  };

  update(id, newObject){
    return axios.put(`${this.baseURL}/${id}`, newObject);
  };

  async getStockHistory(stockId):Promise<StockHistory[]>{
    const data = await axios.get(this.baseURL + "stockHistory/" + stockId).then((response)=>response.data);
    console.log(data);

    let stockHistoryCollection:StockHistory[] = data.map((input)=>{
      let stockHistory: StockHistory = {
      id:input["id"] as number,
      date:input["date"],
        price: Math.round(input["price"] as number),
      stockId:input["stockId"] as number
    }
      return stockHistory;
    });
    
    return stockHistoryCollection;
  };
}
