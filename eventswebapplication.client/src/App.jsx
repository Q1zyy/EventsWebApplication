import './App.css'
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import CreateEventPage from './pages/CreateEventPage'
import EventDetailsPage from './pages/EventDetailsPage'


function App() {

  return (
          <Router>
                <div style={{ display: "flex" }}>
                    <Routes>
                        <Route path="/" element={<Navigate to="/create_event" />} />
                    <Route path="/create_event" element={<CreateEventPage />} />
                  <Route path="/event/:id" element={< EventDetailsPage />} />
                    </Routes>
                </div>
          </Router>
  )
}

export default App
