import { useState } from 'react'
import './App.scss'
import {Login} from '@pages/Login/Login'
import { Routes, Route } from "react-router-dom";

import {Registration} from '@pages/Registration/Registration'

export function App() {
  const [count, setCount] = useState(0)

  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Registration />} />
    </Routes>
  )
}

