import React, {useState,useContext,useEffect} from "react";
import StockOrder from "../../components/order/StockOrder";
import PageTemplate from "../templates/PageTemplate";

function Order() {

      return (
        <PageTemplate>
            <StockOrder />
        </PageTemplate>
      )
}

export default Order