import { Component, OnInit } from '@angular/core';
import { Stock } from 'src/app/models';
import { StockApiService } from 'src/app/services/stock-api.service';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import * as moment from 'moment';

@Component({
  selector: 'app-stock-container',
  templateUrl: './stock-container.component.html',
  styleUrls: ['./stock-container.component.css']
})
export class StockContainerComponent implements OnInit {
  stocks:Stock[];
  chartTitle:string= "";
  multi: any[];

  isChartVisible:boolean= false;
  // options
  legend: boolean = true;
  showLabels: boolean = true;
  animations: boolean = true;
  xAxis: boolean = true;
  yAxis: boolean = true;
  showYAxisLabel: boolean = true;
  showXAxisLabel: boolean = true;
  xAxisLabel: string = 'Date';
  yAxisLabel: string = 'Price in ($)';
  timeline: boolean = true;

  colorScheme = {
    domain: ['#5AA454', '#E44D25', '#CFC0BB', '#7aa3e5', '#a8385d', '#aae3f5']
  };

  constructor(private stockApiService:StockApiService) { }

  ngOnInit(): void {
    setInterval(()=>{
      this.stockApiService.getAllStocks().then((data)=>{
      console.log("data received");
      console.log(data);
      this.stocks = data;
    });
    },3000);
  }

  onViewHistory(id:number){
    let stock = null;
    this.stocks.forEach((st) => {
      if (st.id == id) {
        stock = st;
      }
    });
    if (stock == null) {
      return;
    }

    this.chartTitle = stock.name + " History"
    let chartDataPoints :any[] = [];
    this.stockApiService.getStockHistory(id).then((data)=>{
      console.log("getStockHistory received");
      console.log(data);
      data.forEach((stockHistory) => {

      chartDataPoints.push({
        "name":moment(stockHistory.date).format("L"),
        "value": stockHistory.price
      });
    });

    let chartData :any[] = [];
    chartData.push({
      "name": stock.name,
      "series": chartDataPoints
    });

    this.multi = chartData;
    this.isChartVisible = true;
    });

  }

  onSelect(data): void {
    console.log('Item clicked');
  }

  onActivate(data): void {
    console.log('Activate');
  }

  onDeactivate(data): void {
    console.log('Deactivate');
  }
}
