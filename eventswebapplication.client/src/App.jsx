import './App.css'
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import CreateEventPage from './pages/CreateEventPage'
import EventDetailsPage from './pages/EventDetailsPage'
import LoginPage from './pages/LoginPage'
import Navbar from './components/Navbar'
import RegisterPage from './pages/RegisterPage';
import EventsPage from './pages/EventsPage';
import UserEventsPage from './pages/UserEventsPage';
import UpdateEventPage from './pages/UpdateEventPage';


function App() {

  return (
      <Router>
        <Navbar />
        <div>
            <Routes>
                <Route path="/" element={<Navigate to="/events" />} />
                <Route path="/create_event" element={<CreateEventPage />} />
                <Route path="/event/:id" element={< EventDetailsPage />} />
                <Route path="/login" element={< LoginPage />} />
                <Route path="/register" element={< RegisterPage />} />
                <Route path="/events" element={< EventsPage />} />
                <Route path="/my-events" element={< UserEventsPage />} />
                <Route path="event/update/:id" element={< UpdateEventPage />} />
            </Routes>
        </div>
    </Router>
  )
}

export default App
