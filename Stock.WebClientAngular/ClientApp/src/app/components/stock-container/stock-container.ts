import { CurrencyPipe, formatDate } from '@angular/common';
import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { Color, LineChartModule, MultiSeries, ScaleType } from '@swimlane/ngx-charts';
import { Stock } from '../../models';
import { StockApi } from '../../services/stock-api';

const REFRESH_INTERVAL_MS = 3000;

@Component({
  imports: [CurrencyPipe, LineChartModule],
  selector: 'app-stock-container',
  styleUrl: './stock-container.css',
  templateUrl: './stock-container.html',
})
export class StockContainer implements OnInit, OnDestroy {
  private readonly stockApi = inject(StockApi);
  private refreshTimer?: ReturnType<typeof setInterval>;
  private isRefreshing = false;

  protected readonly stocks = signal<Stock[]>([]);
  protected readonly chartTitle = signal('');
  protected readonly multi = signal<MultiSeries>([]);
  protected readonly isChartVisible = signal(false);

  // chart options
  protected readonly legend = true;
  protected readonly xAxis = true;
  protected readonly yAxis = true;
  protected readonly showYAxisLabel = true;
  protected readonly showXAxisLabel = true;
  protected readonly xAxisLabel = 'Date';
  protected readonly yAxisLabel = 'Price in ($)';
  protected readonly timeline = true;

  protected readonly colorScheme: Color = {
    domain: ['#99CCE5', '#FF7F7F'],
    group: ScaleType.Ordinal,
    selectable: true,
    name: 'Customer Usage',
  };

  ngOnInit(): void {
    void this.refreshStocks();
    this.refreshTimer = setInterval(() => void this.refreshStocks(), REFRESH_INTERVAL_MS);
  }

  ngOnDestroy(): void {
    clearInterval(this.refreshTimer);
  }

  protected async onViewHistory(stock: Stock): Promise<void> {
    try {
      const history = await this.stockApi.getStockHistory(stock.stockId);
      this.chartTitle.set(`${stock.stockId} History`);
      this.multi.set([
        {
          name: stock.stockId,
          series: history.map((stockHistory) => ({
            name: formatDate(stockHistory.date, 'MM/dd/yyyy', 'en-US'),
            value: stockHistory.price,
          })),
        },
      ]);
      this.isChartVisible.set(true);
    } catch (error) {
      console.error(`Loading the history of ${stock.stockId} failed`, error);
    }
  }

  private async refreshStocks(): Promise<void> {
    if (this.isRefreshing) {
      return;
    }

    this.isRefreshing = true;
    try {
      this.stocks.set(await this.stockApi.getAllStocks());
    } catch (error) {
      console.error('Loading the stocks failed', error);
    } finally {
      this.isRefreshing = false;
    }
  }
}
