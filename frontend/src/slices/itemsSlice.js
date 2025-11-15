import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import axios from "axios";

const API = "http://localhost:5191/api/items";

export const fetchItems = createAsyncThunk("items/fetchItems", async () => {
  const res = await axios.get(API);
  return res.data;
});

const itemsSlice = createSlice({
  name: "items",
  initialState: { data: [], loading: false },
  extraReducers: (builder) => {
    builder
      .addCase(fetchItems.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchItems.fulfilled, (state, action) => {
        state.data = action.payload;
        state.loading = false;
      })
      .addCase(fetchItems.rejected, (state) => {
        state.loading = false;
      });
  },
});

export default itemsSlice.reducer;
