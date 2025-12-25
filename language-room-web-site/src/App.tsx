import { useState } from 'react'
import './App.scss'

import { Chat } from '@pages/Chat/Chat'
import { ConfirmedInvitations } from '@pages/ConfirmedInvitations/ConfirmedInvitations'
import { CreateChat } from '@pages/CreateChat/CreateChat'
import { DevNavigation } from '@pages/DevNavigation/DevNavigation'
import { Dialog } from '@pages/Dialog/Dialog'

import { ActiveSessionsPage } from '@pages/ActiveSessionsPage/ActiveSessionsPage'
import { DialogReportPage } from '@pages/DialogReportPage/DialogReportPage'
import { GroupDetailsPage } from '@pages/GroupDetailsPage/GroupDetailsPage'
import { GroupsListPage } from '@pages/GroupsListPage/GroupsListPage'
import { LiveDialogPage } from '@pages/LiveDialogPage/LiveDialogPage'
import { IncomingInvitations } from '@pages/incomingInvitations/incomingInvitations'

import { Login } from '@pages/Login/Login'

import { Profile } from '@pages/Profile/Profile'
import { Registration } from '@pages/Registration/Registration'
import { SelectRole } from '@pages/SelectRole/SelectRole'
import { StudentChoice } from '@pages/StudentChoice/StudentChoice'
import { TaskStatistics } from '@pages/TaskStatistics/TaskStatistics'
import { LoadingInvitations } from '@pages/loadingInvitations/loadingInvitations'


import { Header } from '@ui/Header/Header'

import { Route, Routes } from "react-router-dom"
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
      <Route path="/chat/:id" element={<Chat/>} />
      <Route path="/dialog" element={<Dialog/>} />
      <Route path="/live-dialog" element={<LiveDialogPage />} />
      <Route path="/groups-list" element={<GroupsListPage />} />
      <Route path="/group-details" element={<GroupDetailsPage />} />
      <Route path="/dialog-report" element={<DialogReportPage />} />
      <Route path="/active-sessions" element={<ActiveSessionsPage />} />
      <Route path="/incoming-invitations" element={<IncomingInvitations />} />
    </Routes>
    </>
  )
}

