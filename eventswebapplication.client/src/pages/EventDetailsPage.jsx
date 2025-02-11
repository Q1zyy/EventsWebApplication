import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import apiClient from "./../api/ApiClient";

const EventDetailsPage = () => {
    const [event, setEvent] = useState(null);
    const [participants, setParticipants] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [isRegistered, setIsRegistered] = useState(false);
    const { id } = useParams();
    const [isAdmin, setIsAdmin] = useState(false);

    useEffect(() => {
        const fetchEvent = async () => {
            try {
                const eventResponse = await apiClient.get(`/events/${id}`);
                setEvent(eventResponse.data);

                const participantsResponse = await apiClient.get(`/events/${id}/participants`);
                setParticipants(participantsResponse.data);

                const user = JSON.parse(localStorage.getItem("user"));
                if (user) {
                    setIsRegistered(participantsResponse.data.some(p => p.user.id === user.id));
                    setIsAdmin(user.role === 1);
                }
                setLoading(false);
            } catch (err) {
                setError("Failed to fetch event details", err);
                setLoading(false);
            }
        };
        fetchEvent();
    }, [id]);

    const handleRegistration = async () => {
        try {
            const user = JSON.parse(localStorage.getItem("user"));
            if (!user || !user.id) {
                throw new Error("User is not logged in or user ID is missing.");
            }

            await apiClient.post(`/participants`, { eventId: id, userId: user.id });

            setIsRegistered(true);

            const participantsResponse = await apiClient.get(`/events/${id}/participants`);
            setParticipants(participantsResponse.data);
        } catch (err) {
            setError("Failed to register for the event", err);
        }
    };

    const handleUnregistration = async () => {
        try {
            const user = JSON.parse(localStorage.getItem("user"));
            if (!user || !user.id) {
                throw new Error("User is not logged in or user ID is missing.");
            }

            await apiClient.delete(`/participants`, { data: { eventId: id, userId: user.id } });

            setIsRegistered(false);

            const participantsResponse = await apiClient.get(`/events/${ id }/participants`);
            setParticipants(participantsResponse.data);
        } catch (err) {
            setError("Failed to unregister from the event", err);
        }
    };


    const handleDeleteImage = async (image) => {
        try {
            console.log(image);
            await apiClient.delete(`/events/delete-image`, {
                data: { id, image },
                headers: { "Content-Type": "application/json" }
            });
            setEvent((prev) => ({ ...prev, images: prev.images.filter(img => img !== image) }));
        } catch (err) {
            setError("Failed to delete image", err);
        }
    };

    const handleAddImage = async (event) => {
        const file = event.target.files[0];
        if (!file) return;

        const formData = new FormData();
        formData.append("Id", id);
        formData.append("Image", file);

        try {
            await apiClient.post(`/events/add-image/`, formData, {
                headers: { "Content-Type": "multipart/form-data" }
            });
            const updatedEvent = await apiClient.get(`/events/${id}`);
            setEvent(updatedEvent.data);
        } catch (err) {
            setError("Failed to add image", err);
        }
    };

    if (loading) return <div>Loading...</div>;
    if (error) return <div>{error}</div>;

    return (
        <div className="container">
            <h1>{event.title}</h1>
            <p><strong>Description:</strong> {event.description || "No description available"}</p>
            <p><strong>Event Date & Time:</strong> {new Date(event.eventDateTime).toLocaleString()}</p>
            <p><strong>Place:</strong> {event.place || "No place specified"}</p>
            <p><strong>Participants Max Count:</strong> {event.participantsMaxCount}</p>
            <p><strong>Current Participants:</strong> {participants.length}</p>
            <p><strong>Category:</strong> {event.category ? event.category.title : "No category specified"}</p>
            <h3>Participants:</h3>
            <ul>
                {participants.length > 0 ? participants.map((participant, index) => (
                    <li key={index}>{participant.user.name} {participant.user.surname} {participant.user.email} {new Date(participant.registrationTime).toLocaleString()}</li>
                )) : <p>No participants</p>}
            </ul>
            {event.images && event.images.length > 0 && (
                <div>
                    <h3>Images:</h3>
                    <div>
                        {event.images.map((image, index) => (
                            <div key={index}>
                                <img src={`https://localhost:7287/${image}`} alt={`Event image ${index + 1}`} style={{ width: '200px', height: '200px', objectFit: 'cover', borderRadius: '8px' }} />
                                {isAdmin && <button onClick={() => handleDeleteImage(image)}>Delete</button>}
                            </div>
                        ))}
                    </div>
                </div>
            )}
            {isAdmin && (
                <div>
                    <h3>Add Image:</h3>
                    <input type="file" onChange={handleAddImage} />
                </div>
            )}
            <div>
                {!isRegistered ? (
                    <button onClick={handleRegistration}>Register for Event</button>
                ) : (
                    <>
                        <p>You are already registered for this event.</p>
                        <button onClick={handleUnregistration}>Unregister</button>
                    </>
                )}
            </div>
        </div>
    );
};

export default EventDetailsPage;