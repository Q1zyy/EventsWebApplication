import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import apiClient from "./../api/ApiClient";
import "./../css/UserEvents.css";


const UserEventsPage = () => {
    const [events, setEvents] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchUserEvents = async () => {
            try {
                const user = JSON.parse(localStorage.getItem("user"));

                if (!user || !user.id) {
                    throw new Error("User is not logged in.");
                }

                const response = await apiClient.get(`/events/by-user/${user.id}`);
                setEvents(response.data);
                setLoading(false);
            } catch (err) {
                setError("Failed to fetch user events", err);
                setLoading(false);
            }
        };

        fetchUserEvents();
    }, []);

    if (loading) return <div>Loading...</div>;
    if (error) return <div>{error}</div>;

    return (
        <div className="container">
            <h1>Your Registered Events</h1>
            {events.length === 0 ? (
                <p>You are not registered for any events.</p>
            ) : (
                <ul className="event-list">
                    {events.map(event => (
                        <li key={event.id} className="event-item">
                            <Link to={`/event/${event.id}`} className="event-title">{event.title}</Link>
                            <p className="event-date">{new Date(event.eventDateTime).toLocaleString()}</p>
                            <p className="event-place">{event.place || "No location specified"}</p>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default UserEventsPage;
