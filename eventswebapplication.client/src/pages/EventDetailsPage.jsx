import { useEffect, useState } from "react";
import axios from "axios";
import { useParams } from "react-router-dom";

const EventDetailsPage = () => {
    const [event, setEvent] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const { id } = useParams(); // Получаем ID из URL

    useEffect(() => {
        const fetchEvent = async () => {
            try {
                const response = await axios.get(`https://localhost:7287/api/events/${id}`);
                setEvent(response.data);
                setLoading(false);
            } catch (err) {
                setError("Failed to fetch event details", err);
                setLoading(false);
            }
        };

        fetchEvent();
    }, [id]);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div className="container mx-auto p-6">
            <h1 className="text-3xl font-bold mb-4">{event.title}</h1>
            <div className="mb-4">
                <strong>Description:</strong>
                <p>{event.description || "No description available"}</p>
            </div>
            <div className="mb-4">
                <strong>Event Date & Time:</strong>
                <p>{new Date(event.eventDateTime).toLocaleString()}</p>
            </div>
            <div className="mb-4">
                <strong>Place:</strong>
                <p>{event.place || "No place specified"}</p>
            </div>
            <div className="mb-4">
                <strong>Participants Max Count:</strong>
                <p>{event.participantsMaxCount}</p>
            </div>
            <div className="mb-4">
                <strong>Category:</strong>
                <p>{event.category ? event.category.title : "No category specified"}</p>
            </div>
            <div className="mb-4">
                <strong>Participants:</strong>
                <ul>
                    {event.participants && event.participants.length > 0 ? (
                        event.participants.map((participant, index) => (
                            <li key={index}>{participant.name}</li>
                        ))
                    ) : (
                        <p>No participants</p>
                    )}
                </ul>
            </div>
            {event.images && event.images.length > 0 && (
                <div className="mb-4">
                    <strong>Images:</strong>
                    <div className="flex space-x-4">
                        {event.images.map((image, index) => (
                            <img
                                key={index}
                                src={`https://localhost:7287/${image}`}
                                alt={`Event image ${index + 1}`}
                                className="w-32 h-32 object-cover rounded"
                            />
                        ))}
                    </div>
                </div>
            )}
        </div>
    );
};

export default EventDetailsPage;