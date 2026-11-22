import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Stock } from '../../models';
import { StockApi } from '../../services/stock-api';
import { StockContainer } from './stock-container';

describe('StockContainer', () => {
  let component: StockContainer;
  let fixture: ComponentFixture<StockContainer>;
  const stockApi = {
    getAllStocks: vi.fn<() => Promise<Stock[]>>(),
    getStockHistory: vi.fn(() => Promise.resolve([])),
  };

  beforeEach(async () => {
    vi.useFakeTimers();
    stockApi.getAllStocks.mockReset();
    stockApi.getAllStocks.mockResolvedValue([
      { stockId: 'MSFT', price: 310.5 },
      { stockId: 'AAPL', price: 172.256 },
    ]);

    await TestBed.configureTestingModule({
      imports: [StockContainer],
      providers: [{ provide: StockApi, useValue: stockApi }],
    }).compileComponents();

    fixture = TestBed.createComponent(StockContainer);
    component = fixture.componentInstance;
    fixture.detectChanges();
    await vi.advanceTimersByTimeAsync(0);
    fixture.detectChanges();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load the stocks immediately and show them by stockId with a currency price', () => {
    expect(stockApi.getAllStocks).toHaveBeenCalledTimes(1);

    const rows = (fixture.nativeElement as HTMLElement).querySelectorAll('tbody tr');
    const cells = Array.from(rows, (row) =>
      Array.from(row.querySelectorAll('td'), (cell) => cell.textContent?.trim()),
    );
    expect(cells).toEqual([
      ['1', 'MSFT', '$310.50', 'View History'],
      ['2', 'AAPL', '$172.26', 'View History'],
    ]);
  });

  it('should poll every 3 seconds and stop when destroyed', async () => {
    await vi.advanceTimersByTimeAsync(3000);
    expect(stockApi.getAllStocks).toHaveBeenCalledTimes(2);

    fixture.destroy();
    await vi.advanceTimersByTimeAsync(9000);
    expect(stockApi.getAllStocks).toHaveBeenCalledTimes(2);
  });

  it('should keep polling after a failed request', async () => {
    const consoleError = vi.spyOn(console, 'error').mockImplementation(() => undefined);
    stockApi.getAllStocks.mockRejectedValueOnce(new Error('Network Error'));

    await vi.advanceTimersByTimeAsync(3000);
    expect(consoleError).toHaveBeenCalledTimes(1);

    await vi.advanceTimersByTimeAsync(3000);
    expect(stockApi.getAllStocks).toHaveBeenCalledTimes(3);
    consoleError.mockRestore();
  });
});
