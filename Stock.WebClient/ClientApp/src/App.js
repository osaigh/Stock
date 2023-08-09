import React, {useState,useContext,useEffect} from "react";
import { SignInCallback } from "./pages/signin/signincallback";
import { SignOutCallback } from "./pages/signout/signoutcallback";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { UserAuthenticationContext } from "./context/user-manager-context";
import Stocks from "./pages/stocks/Stocks";
import Home from "./pages/home/Home";
import Order from "./pages/orders/Order";
import Login from "./pages/login/Login";
import Orders from "./pages/orders/Orders";

function App() {
  const { userAuthenticationService } = useContext(UserAuthenticationContext);  
  const [user, setUser] = useState(null);

  useEffect(() => {
    async function getUserAsync() {
      const user = await userAuthenticationService.getUser();
      return user;
    }

    getUserAsync().then((user) => {
      setUser(user);
    });
  }, []);

  return (
    <div className="App">
     <BrowserRouter>
        <Routes>
          <Route path="/" element={<Stocks />} />
          <Route path="orders" element={<Orders />} />
          <Route path="order/:stockId/:transactionType" element={<Order />} />
          <Route path="login" element={<Login />} />
          <Route
            path="signincallback"
            exact
            element={<SignInCallback />}
          ></Route>
          <Route
            path="signoutcallback"
            exact
            element={<SignOutCallback />}
          ></Route>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
