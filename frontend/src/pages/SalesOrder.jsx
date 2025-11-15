import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchCustomers } from "../slices/customersSlice";
import { fetchItems } from "../slices/itemsSlice";
import { addOrder, updateOrder } from "../slices/ordersSlice";
import { useNavigate, useLocation } from "react-router-dom";

function SalesOrder() {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const location = useLocation();

  const editingOrder = location.state?.order ?? null;

  const customers = useSelector((s) => s.customers.data);
  const items = useSelector((s) => s.items.data);
  const ordersSaving = useSelector((s) => s.orders.saving);

  const [order, setOrder] = useState({
    id: null,
    customerId: "",
    customerName: "",
    address1: "",
    address2: "",
    address3: "",
    suburb: "",
    state: "",
    postCode: "",
    invoiceNo: "",
    invoiceDate: new Date().toISOString().substring(0, 10),
    referenceNo: "",
    note: "",
    lines: [],
    totalExcl: 0,
    totalTax: 0,
    totalIncl: 0,
  });

  
  useEffect(() => {
    dispatch(fetchCustomers());
    dispatch(fetchItems());
  }, [dispatch]);

 
  useEffect(() => {
    if (!editingOrder) return;

    const normalized = {
      ...editingOrder,
      lines: (editingOrder.lines || []).map((l) => ({
        ...l,
        quantity: Number(l.quantity ?? 0),
        taxRate: Number(l.taxRate ?? 0),
        price: Number(l.price ?? 0),
        exclAmount: Number(l.exclAmount ?? 0),
        taxAmount: Number(l.taxAmount ?? 0),
        inclAmount: Number(l.inclAmount ?? 0),
      })),
    };

    setOrder(normalized);
  }, [editingOrder]);

  
  const recalcLine = (line) => {
    const qty = Number(line.quantity || 0);
    const price = Number(line.price || 0);
    const taxRate = Number(line.taxRate || 0);

    const exclAmount = qty * price;
    const taxAmount = exclAmount * (taxRate / 100);
    const inclAmount = exclAmount + taxAmount;

    return { ...line, qty, price, taxRate, exclAmount, taxAmount, inclAmount };
  };

  const recalcTotals = (lines) => {
    const totalExcl = lines.reduce((s, l) => s + (l.exclAmount || 0), 0);
    const totalTax = lines.reduce((s, l) => s + (l.taxAmount || 0), 0);
    const totalIncl = lines.reduce((s, l) => s + (l.inclAmount || 0), 0);
    return { totalExcl, totalTax, totalIncl };
  };

  const handleCustomerChange = (custId) => {
    const c = customers.find((x) => x.id === Number(custId));

    if (!c) {
      setOrder((o) => ({
        ...o,
        customerId: "",
        customerName: "",
        address1: "",
        address2: "",
        address3: "",
        suburb: "",
        state: "",
        postCode: "",
      }));
      return;
    }

    setOrder((o) => ({
      ...o,
      customerId: c.id,
      customerName: c.name,
      address1: c.address1 ?? "",
      address2: c.address2 ?? "",
      address3: c.address3 ?? "",
      suburb: c.suburb ?? c.city ?? "",
      state: c.state ?? "",
      postCode: c.postCode ?? c.postalCode ?? "",
    }));
  };

  const addLine = () => {
    setOrder((o) => ({
      ...o,
      lines: [
        ...o.lines,
        {
          itemId: null,
          itemCode: "",
          description: "",
          quantity: 1,
          price: 0,
          taxRate: 0,
          exclAmount: 0,
          taxAmount: 0,
          inclAmount: 0,
          note: "",
        },
      ],
    }));
  };

  const removeLine = (index) => {
    const lines = order.lines.filter((_, i) => i !== index);
    const totals = recalcTotals(lines);
    setOrder((o) => ({ ...o, lines, ...totals }));
  };

  const updateLine = (index, field, value) => {
    const lines = [...order.lines];
    const line = { ...lines[index] };

    if (field === "itemId") {
      const item = items.find((it) => it.id === Number(value));
      if (item) {
        line.itemId = item.id;
        line.itemCode = item.itemCode ?? item.code ?? "";
        line.description = item.description ?? "";
        line.price = Number(item.price ?? 0);
      }
    }

    if (field === "quantity") line.quantity = Number(value);
    if (field === "price") line.price = Number(value);
    if (field === "taxRate") line.taxRate = Number(value);
    if (field === "note") line.note = value;

    const updated = recalcLine(line);
    lines[index] = updated;

    const totals = recalcTotals(lines);
    setOrder((o) => ({ ...o, lines, ...totals }));
  };

  const onSave = async () => {
    if (!order.customerId) return alert("Please select a customer.");
    if (!order.lines.length) return alert("Add at least one item.");

    const payload = {
      customerId: order.customerId,
      invoiceNo: order.invoiceNo,
      invoiceDate: order.invoiceDate,
      referenceNo: order.referenceNo,
      note: order.note,
      totalExcl: order.totalExcl,
      totalTax: order.totalTax,
      totalIncl: order.totalIncl,
      lines: order.lines,
    };

    try {
      if (order.id) {
        await dispatch(updateOrder({ id: order.id, order: payload })).unwrap();
      } else {
        const created = await dispatch(addOrder(payload)).unwrap();
        setOrder((o) => ({ ...o, id: created.id }));
      }
      navigate("/");
    } catch (err) {
      console.error(err);
      alert("Save failed");
    }
  };

  const onPrint = () => {
    if (!order.id) return alert("Save first.");
    window.open(`http://localhost:5191/api/pdf/${order.id}`, "_blank");
  };

  return (
    <div style={{ padding: 20 }}>
      <h2>Sales Order</h2>

      <div style={{ display: "flex", gap: 8, marginBottom: 12 }}>
        <button onClick={onSave} disabled={ordersSaving}>
          {ordersSaving ? "Saving…" : "Save"}
        </button>
        <button onClick={() => navigate("/")}>Cancel</button>
        <button onClick={onPrint}>Print PDF</button>
      </div>

      {/* Customer */}
      <div style={{ marginBottom: 12 }}>
        <label>Customer</label>
        <select
          value={order.customerId || ""}
          onChange={(e) => handleCustomerChange(e.target.value)}
        >
          <option value="">-- select customer --</option>
          {customers.map((c) => (
            <option key={c.id} value={c.id}>
              {c.name}
            </option>
          ))}
        </select>
      </div>

      {/* Address */}
      <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 8 }}>
        <input value={order.address1} onChange={(e) => setOrder((o) => ({ ...o, address1: e.target.value }))} placeholder="Address 1" />
        <input value={order.address2} onChange={(e) => setOrder((o) => ({ ...o, address2: e.target.value }))} placeholder="Address 2" />
        <input value={order.address3} onChange={(e) => setOrder((o) => ({ ...o, address3: e.target.value }))} placeholder="Address 3" />
        <input value={order.suburb} onChange={(e) => setOrder((o) => ({ ...o, suburb: e.target.value }))} placeholder="Suburb" />
        <input value={order.state} onChange={(e) => setOrder((o) => ({ ...o, state: e.target.value }))} placeholder="State" />
        <input value={order.postCode} onChange={(e) => setOrder((o) => ({ ...o, postCode: e.target.value }))} placeholder="PostCode" />
      </div>

      <hr />

      {/* Lines */}
      <h3>Lines</h3>
      <button onClick={addLine}>+ Add Line</button>

      <table border="1" width="100%" style={{ marginTop: 8 }}>
        <thead>
          <tr>
            <th>Item</th>
            <th>Description</th>
            <th>Qty</th>
            <th>Price</th>
            <th>Tax %</th>
            <th>Excl</th>
            <th>Tax</th>
            <th>Incl</th>
            <th></th>
          </tr>
        </thead>

        <tbody>
          {order.lines.length === 0 && (
            <tr>
              <td colSpan={9} style={{ textAlign: "center" }}>
                No lines. Click "Add Line".
              </td>
            </tr>
          )}

          {order.lines.map((l, idx) => (
            <tr key={idx}>
              <td>
                <select value={l.itemId || ""} onChange={(e) => updateLine(idx, "itemId", e.target.value)}>
                  <option value="">--select--</option>
                  {items.map((it) => (
                    <option key={it.id} value={it.id}>
                      {it.itemCode ?? it.code}
                    </option>
                  ))}
                </select>
              </td>

              <td>{l.description}</td>

              <td>
                <input type="number" value={l.quantity} onChange={(e) => updateLine(idx, "quantity", e.target.value)} />
              </td>

              <td>
                <input type="number" value={l.price} onChange={(e) => updateLine(idx, "price", e.target.value)} />
              </td>

              <td>
                <input type="number" value={l.taxRate} onChange={(e) => updateLine(idx, "taxRate", e.target.value)} />
              </td>

              <td>{l.exclAmount.toFixed(2)}</td>
              <td>{l.taxAmount.toFixed(2)}</td>
              <td>{l.inclAmount.toFixed(2)}</td>

              <td>
                <button onClick={() => removeLine(idx)}>X</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {/* Totals */}
      <div style={{ marginTop: 16 }}>
        <div>Total Excl: {order.totalExcl.toFixed(2)}</div>
        <div>Total Tax: {order.totalTax.toFixed(2)}</div>
        <div>Total Incl: {order.totalIncl.toFixed(2)}</div>
      </div>
    </div>
  );
}

export default SalesOrder;
