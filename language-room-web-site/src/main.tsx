import {App} from './App.tsx'
import React from "react"
import ReactDOM from "react-dom/client";
import './index.css'
import { BrowserRouter } from "react-router-dom";
import {StudentChoiceProvider } from '@entities/ChatSelectionContext.tsx'
import { OnlineUsersProvider } from "@entities/OnlineUsersContext.tsx";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <BrowserRouter>
      <StudentChoiceProvider>
        <OnlineUsersProvider>
          <App />
        </OnlineUsersProvider>
      </StudentChoiceProvider>
    </BrowserRouter>
  </React.StrictMode>
);
