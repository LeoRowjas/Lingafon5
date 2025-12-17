import {App} from './App.tsx'
import React from "react"
import ReactDOM from "react-dom/client";
import './index.css'
import { BrowserRouter } from "react-router-dom";
import {StudentChoiceProvider } from '@entities/ChatSelectionContext.tsx'

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <BrowserRouter>
      <StudentChoiceProvider>
        <App />
      </StudentChoiceProvider>
    </BrowserRouter>
  </React.StrictMode>
);
