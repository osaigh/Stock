export interface Stock{
    id:number,
    name:string,
    price:number,
    volume:number
}

export interface StockHistory{
    id:number,
    date:string,
    price:number,
    stockId:number
}

export interface UserInterface{
    username:string,

}