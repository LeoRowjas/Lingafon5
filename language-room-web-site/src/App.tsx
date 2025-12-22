import { useState } from 'react'
import './App.scss'

import { Routes, Route } from "react-router-dom";
import {Login} from '@pages/Login/Login'
import {Registration} from '@pages/Registration/Registration'
import {Profile} from '@pages/Profile/Profile'
import {CreateChat} from '@pages/CreateChat/CreateChat'
import {SelectRole} from '@pages/SelectRole/SelectRole'
import {StudentChoice} from '@pages/StudentChoice/StudentChoice'
import {LoadingInvitations} from '@pages/loadingInvitations/loadingInvitations'
import {ConfirmedInvitations} from '@pages/ConfirmedInvitations/ConfirmedInvitations'
import {TaskStatistics} from '@pages/TaskStatistics/TaskStatistics'
import {Chat} from '@pages/Chat/Chat'
import {Dialog} from '@pages/Dialog/Dialog'

import { Header } from '@ui/Header/Header'
import { DevNavigation } from '@pages/DevNavigation/DevNavigation'
export function App() {
  
  const [count, setCount] = useState(0)

  return (
    <>
    <Header />
    <Routes>
      <Route path="/" element={<DevNavigation />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Registration />} />
      <Route path="/profile" element={<Profile />} />
      <Route path="/create-chat" element={<CreateChat />} />
      <Route path="/select-role" element={<SelectRole />} />
      <Route path="/student-choice" element={<StudentChoice />} />
      <Route path="/loading-in" element={<LoadingInvitations />} />
      <Route path="/confirm-in" element={<ConfirmedInvitations />} />
      <Route path="/task" element={<TaskStatistics />} />
      <Route path="/chat/:id" element={<Chat />} />
      <Route path="/dialog" element={<Dialog />} />
    </Routes>
    </>
  )
}

