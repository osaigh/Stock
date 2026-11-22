import { lazy, Suspense } from "react";
import { BrowserRouter, Navigate, Routes, Route } from "react-router-dom";
import { SignInCallback } from "./pages/signin/signincallback";
import { SignOutCallback } from "./pages/signout/signoutcallback";
import Login from "./pages/login/Login";

// The pages behind the login are loaded on demand, which keeps the data grid
// and the chart library out of the bundle that the login page needs.
const Stocks = lazy(() => import("./pages/stocks/Stocks"));
const Orders = lazy(() => import("./pages/orders/Orders"));
const Order = lazy(() => import("./pages/orders/Order"));

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <Suspense fallback={null}>
          <Routes>
            <Route path="/" element={<Stocks />} />
            <Route path="orders" element={<Orders />} />
            <Route path="order/:stockId/:transactionType" element={<Order />} />
            <Route path="login" element={<Login />} />
            <Route path="signincallback" element={<SignInCallback />} />
            <Route path="signoutcallback" element={<SignOutCallback />} />
            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </Suspense>
      </BrowserRouter>
    </div>
  );
}

export default App;
