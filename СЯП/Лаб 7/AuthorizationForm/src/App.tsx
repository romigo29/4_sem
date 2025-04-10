import './css/App.css'

import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Registration from './pages/Registration'
import Authorization from './pages/Authorization'
import PasswordRecovery from './pages/PasswordRecovery'

function App() {

  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path='/sign-up' element={<Registration />} />
          <Route path='/sign-in' element={<Authorization />} />
          <Route path='/reset-password' element={<PasswordRecovery />} />
        </Routes>
      </BrowserRouter>
    </>
  )
}

export default App
