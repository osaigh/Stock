import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import 'bootstrap/dist/css/bootstrap.min.css';
import App from './App';
import { store } from './store';
import { UserAuthenticationContext } from './context/user-manager-context';
import { StockApiContext } from './context/stock-api-context';
import {
  getUserAuthenticationService,
  getStockApiService,
} from './services/serviceprovider';
import './index.css';

const userAuthenticationService = getUserAuthenticationService();
const stockApiService = getStockApiService();

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <UserAuthenticationContext value={{ userAuthenticationService }}>
      <StockApiContext value={{ stockApiService }}>
        <Provider store={store}>
          <App />
        </Provider>
      </StockApiContext>
    </UserAuthenticationContext>
  </StrictMode>,
);
