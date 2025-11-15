// frontend/src/slices/customersSlice.js
import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import axios from "axios";

const API = "http://localhost:5191/api/customers";

export const fetchCustomers = createAsyncThunk(
  "customers/fetchCustomers",
  async () => {
    const res = await axios.get(API);
    return res.data;
  }
);

const customersSlice = createSlice({
  name: "customers",
  initialState: {
    data: [],
    loading: false,
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchCustomers.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchCustomers.fulfilled, (state, action) => {
        state.loading = false;
        state.data = action.payload;
      })
      .addCase(fetchCustomers.rejected, (state) => {
        state.loading = false;
      });
  },
});

export default customersSlice.reducer;
