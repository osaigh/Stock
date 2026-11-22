import { inject, Service } from '@angular/core';
import axios from 'axios';
import { StockAPIConfig } from '../configuration/config';
import { Stock, StockHistory } from '../models';
import { UserAuthentication } from './user-authentication';

@Service()
export class StockApi {
  private readonly userAuthentication = inject(UserAuthentication);
  private readonly baseURL = StockAPIConfig.baseURL;

  async getAllStocks(): Promise<Stock[]> {
    const data = await this.get<Stock>('Stock');
    return data.map((input) => ({
      stockId: input.stockId,
      price: input.price,
    }));
  }

  async getStockHistory(stockId: string): Promise<StockHistory[]> {
    const data = await this.get<StockHistory>(`StockHistory/${encodeURIComponent(stockId)}`);
    return data.map((input) => ({
      stockHistoryId: input.stockHistoryId,
      stockId: input.stockId,
      date: input.date,
      price: input.price,
    }));
  }

  private async get<T>(path: string): Promise<T[]> {
    const accessToken = await this.userAuthentication.getAccessToken();
    if (!accessToken) {
      return [];
    }

    const response = await axios.get<T[]>(this.baseURL + path, {
      headers: { Authorization: `Bearer ${accessToken}` },
    });
    return response.data;
  }
}
