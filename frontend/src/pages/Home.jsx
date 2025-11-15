import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchOrders } from "../slices/ordersSlice";
import { useNavigate } from "react-router-dom";

export default function Home() {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const orders = useSelector((s) => s.orders.data || []);
  const loading = useSelector((s) => s.orders.loading);

  useEffect(() => {
    dispatch(fetchOrders());
  }, []);

  return (
    <div style={{ padding: 20 }}>
      <h1>Sales Orders</h1>

      <div style={{ marginBottom: 12 }}>
        <button onClick={() => navigate("/sales-order")}>+ Add New</button>
      </div>

      {loading ? (
        <div>Loading…</div>
      ) : (
        <table border="1" width="100%">
          <thead>
            <tr>
              <th>ID</th>
              <th>Customer</th>
              <th>Total Incl</th>
              <th>Date</th>
              <th></th>
            </tr>
          </thead>

          <tbody>
            {orders.map((o) => (
              <tr key={o.id}>
                <td
                  style={{ cursor: "pointer" }}
                  onDoubleClick={() =>
                    navigate("/sales-order", { state: { order: o } })
                  }
                >
                  {o.id}
                </td>

                <td>{o.customerName}</td>
                <td>{(o.totalIncl ?? 0).toFixed(2)}</td>
                <td>{o.invoiceDate ? o.invoiceDate.substring(0, 10) : ""}</td>

                <td>
                  <button
                    onClick={() =>
                      navigate("/sales-order", { state: { order: o } })
                    }
                  >
                    Edit
                  </button>

                  <button
                    style={{ marginLeft: 8 }}
                    onClick={() =>
                      window.open(
                        `http://localhost:5191/api/pdf/${o.id}`,
                        "_blank"
                      )
                    }
                  >
                    Print
                  </button>
                </td>
              </tr>
            ))}

            {orders.length === 0 && (
              <tr>
                <td colSpan="5" align="center">
                  No orders found
                </td>
              </tr>
            )}
          </tbody>
        </table>
      )}
    </div>
  );
}
