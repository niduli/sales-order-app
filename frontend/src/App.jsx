// frontend/src/App.jsx
import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import SalesOrder from "./pages/SalesOrder";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/sales-order" element={<SalesOrder />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
