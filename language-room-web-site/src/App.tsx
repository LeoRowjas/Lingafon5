import { useState } from 'react'
import './App.scss'

import { Routes, Route } from "react-router-dom";
import {Login} from '@pages/Login/Login'
import {Registration} from '@pages/Registration/Registration'
import {Profile} from '@pages/Profile/Profile'

export function App() {
  const [count, setCount] = useState(0)

  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Registration />} />
      <Route path="/profile" element={<Profile />} />
    </Routes>
  )
}

