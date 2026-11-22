export interface Stock {
  stockId: string;
  price: number;
}

export interface StockHistory {
  stockHistoryId: number;
  stockId: string;
  date: string;
  price: number;
}

export interface UserInterface {
  username: string;
}
