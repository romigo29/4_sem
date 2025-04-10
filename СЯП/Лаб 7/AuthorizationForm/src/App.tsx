import './css/App.css'

import { Routes, Route } from 'react-router'
import Registration from './pages/Registration'
import Authorization from './pages/Authorization'
import PasswordRecovery from './pages/PasswordRecovery'

function App() {
  
  return (
    <>
    
      <Routes>
        <Route path='/sign-up' element={<Registration />} />
        <Route path='/sign-in' element={<Authorization />} />
        <Route path='/reset-password' element={<PasswordRecovery />} />
      </Routes>
    </>
  )
}

export default App
