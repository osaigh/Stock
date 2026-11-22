import { TestBed } from '@angular/core/testing';
import axios from 'axios';
import { StockApi } from './stock-api';
import { UserAuthentication } from './user-authentication';

describe('StockApi', () => {
  let service: StockApi;
  const userAuthentication = { getAccessToken: vi.fn<() => Promise<string | null>>() };

  beforeEach(() => {
    userAuthentication.getAccessToken.mockReset();
    userAuthentication.getAccessToken.mockResolvedValue('token');

    TestBed.configureTestingModule({
      providers: [{ provide: UserAuthentication, useValue: userAuthentication }],
    });
    service = TestBed.inject(StockApi);
  });

  afterEach(() => {
    vi.restoreAllMocks();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get the stocks with the access token', async () => {
    const get = vi
      .spyOn(axios, 'get')
      .mockResolvedValue({ data: [{ stockId: 'MSFT', price: 310.55 }] });

    await expect(service.getAllStocks()).resolves.toEqual([{ stockId: 'MSFT', price: 310.55 }]);
    expect(get).toHaveBeenCalledWith('http://localhost:44700/api/Stock', {
      headers: { Authorization: 'Bearer token' },
    });
  });

  it('should get the history of a stock by stockId', async () => {
    const history = [
      { stockHistoryId: 1, stockId: 'MSFT', date: '2021-01-31T00:00:00+00:00', price: 69.5 },
    ];
    const get = vi.spyOn(axios, 'get').mockResolvedValue({ data: history });

    await expect(service.getStockHistory('MSFT')).resolves.toEqual(history);
    expect(get).toHaveBeenCalledWith('http://localhost:44700/api/StockHistory/MSFT', {
      headers: { Authorization: 'Bearer token' },
    });
  });

  it('should not call the API when nobody is signed in', async () => {
    const get = vi.spyOn(axios, 'get');
    userAuthentication.getAccessToken.mockResolvedValue(null);

    await expect(service.getAllStocks()).resolves.toEqual([]);
    expect(get).not.toHaveBeenCalled();
  });
});
