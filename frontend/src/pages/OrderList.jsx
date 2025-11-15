import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchOrders } from "../slices/ordersSlice";
import { useNavigate } from "react-router-dom";

function OrderList() {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const orders = useSelector((state) => state.orders.data);

  useEffect(() => {
    dispatch(fetchOrders());
  }, []);

  return (
    <div style={{ padding: "20px" }}>
      <h1>Sales Orders</h1>

      <button
        onClick={() => navigate("/order")}
        style={{ padding: "10px 20px", marginBottom: "20px" }}
      >
        ➕ Add New Order
      </button>

      <table border="1" width="100%" cellPadding="10">
        <thead>
          <tr>
            <th>ID</th>
            <th>Customer</th>
            <th>Date</th>
            <th>Total Incl</th>
            <th>Actions</th>
          </tr>
        </thead>

        <tbody>
          {orders.length === 0 ? (
            <tr>
              <td colSpan="5" align="center">No orders found</td>
            </tr>
          ) : (
            orders.map((order) => (
              <tr
                key={order.id}
                onDoubleClick={() =>
                  navigate("/order", { state: order })
                }
                style={{ cursor: "pointer" }}
              >
                <td>{order.id}</td>
                <td>{order.customerName}</td>
                <td>{order.invoiceDate}</td>
                <td>{order.totalIncl}</td>

                <td>
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      navigate("/order", { state: order });
                    }}
                  >
                    Edit
                  </button>

                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      window.open(`http://localhost:5191/api/pdf/${order.id}`);
                    }}
                    style={{ marginLeft: "10px" }}
                  >
                    Print
                  </button>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}

export default OrderList;
