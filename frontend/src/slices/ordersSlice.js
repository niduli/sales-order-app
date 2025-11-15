// frontend/src/slices/ordersSlice.js
import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import axios from "axios";

const API = "http://localhost:5191/api/orders";

// Fetch all orders
export const fetchOrders = createAsyncThunk("orders/fetchOrders", async () => {
  const res = await axios.get(API);
  return res.data;
});

// Create new order
export const addOrder = createAsyncThunk("orders/addOrder", async (order) => {
  const res = await axios.post(API, order);
  return res.data;
});

// Update existing order
export const updateOrder = createAsyncThunk(
  "orders/updateOrder",
  async ({ id, order }) => {
    const res = await axios.put(`${API}/${id}`, order);
    return res.data;
  }
);

const ordersSlice = createSlice({
  name: "orders",
  initialState: {
    data: [],
    loading: false,
    saving: false,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Fetch all
      .addCase(fetchOrders.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchOrders.fulfilled, (state, action) => {
        state.loading = false;
        state.data = action.payload;
      })
      .addCase(fetchOrders.rejected, (state) => {
        state.loading = false;
      })

      // Create
      .addCase(addOrder.pending, (state) => {
        state.saving = true;
      })
      .addCase(addOrder.fulfilled, (state, action) => {
        state.saving = false;
        state.data.push(action.payload);
      })
      .addCase(addOrder.rejected, (state) => {
        state.saving = false;
      })

      // Update
      .addCase(updateOrder.pending, (state) => {
        state.saving = true;
      })
      .addCase(updateOrder.fulfilled, (state, action) => {
        state.saving = false;
        const updated = action.payload;
        const idx = state.data.findIndex((o) => o.id === updated.id);
        if (idx >= 0) state.data[idx] = updated;
      })
      .addCase(updateOrder.rejected, (state) => {
        state.saving = false;
      });
  },
});

export default ordersSlice.reducer;
