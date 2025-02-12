import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import apiClient from "./../api/ApiClient";
import "./../css/EventsPage.css";

const EventsPage = () => {
    const [events, setEvents] = useState([]);
    const [filters, setFilters] = useState({
        title: "",
        dateFrom: "",
        dateTo: "",
        place: "",
        categoryId: "",
        pageNo: 1,
        pageSize: 2,
    });
    const [totalCount, setTotalCount] = useState(0);
    const [categories, setCategories] = useState([]);
    const navigate = useNavigate();

    const user = JSON.parse(localStorage.getItem("user"));
    const isAdmin = user?.role === 1;

    const fetchEvents = async () => {
        try {
            const response = await apiClient.get("/Events", { params: { ...filters } });
            setEvents(response.data.items);
            setTotalCount(response.data.totalCount);
        } catch (error) {
            console.error("Error fetching events:", error);
        }
    };

    const fetchCategories = async () => {
        try {
            const response = await apiClient.get("/Categories");
            setCategories(response.data);
        } catch (error) {
            console.error("Error fetching categories:", error);
        }
    };

    const handleDelete = async (eventId) => {
        if (window.confirm("Are you sure you want to delete this event?")) {
            try {
                await apiClient.delete(`/events/${eventId}`);
                fetchEvents(); 
            } catch (error) {
                console.error("Error deleting event:", error);
            }
        }
    };

    const handleFilterChange = (e) => {
        const { name, value } = e.target;
        setFilters((prevFilters) => ({ ...prevFilters, [name]: value }));
    };

    const handleSearch = (e) => {
        e.preventDefault();
        setFilters((prevFilters) => ({ ...prevFilters, pageNo: 1 }));
    };

    const handlePageChange = (pageNo) => {
        setFilters((prevFilters) => ({ ...prevFilters, pageNo }));
    };

    useEffect(() => {
        fetchEvents();
        fetchCategories();
    }, [filters]);

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        return date.toLocaleString();
    };

    return (
        <div className="events-page-container">
            <h1 className="page-title">Events List</h1>

            {isAdmin && (
                <button className="add-event-button" onClick={() => navigate("/create_event")}>
                    Add Event
                </button>
            )}

            <form onSubmit={handleSearch} className="filters-form">
                <div className="form-group">
                    <input
                        type="text"
                        name="title"
                        placeholder="Event Name"
                        value={filters.title}
                        onChange={handleFilterChange}
                        className="form-input"
                    />
                </div>
                <div className="form-group">
                    <input
                        type="date"
                        name="dateFrom"
                        value={filters.dateFrom}
                        onChange={handleFilterChange}
                        className="form-input"
                    />
                    To
                    <input
                        type="date"
                        name="dateTo"
                        value={filters.dateTo}
                        onChange={handleFilterChange}
                        className="form-input"
                    />
                </div>
                <div className="form-group">
                    <input
                        type="text"
                        name="place"
                        placeholder="Place"
                        value={filters.place}
                        onChange={handleFilterChange}
                        className="form-input"
                    />
                </div>
                <div className="form-group">
                    <select
                        name="categoryId"
                        value={filters.categoryId}
                        onChange={handleFilterChange}
                        className="form-input"
                    >
                        <option value="">Category</option>
                        {categories.map((category) => (
                            <option key={category.id} value={category.id}>
                                {category.title}
                            </option>
                        ))}
                    </select>
                </div>
                <button type="submit" className="search-button">Search</button>
            </form>

            <ul className="events-list">
                {events.map((event) => (
                    <li key={event.id} className="event-item">
                        <h3>
                            <Link to={`/event/${event.id}`} className="event-title">{event.title}</Link>
                        </h3>
                        <p>{event.description}</p>
                        <p>{event.place}</p>
                        <p>{formatDate(event.eventDateTime)}</p>

                        <div className="participants-info">
                            <strong>Participants:</strong>
                            <p>Current: {event.participants.length}</p>
                            <p>Available: {event.participantsMaxCount}</p>
                            {event.participants.length >= event.participantsMaxCount && (
                                <p className="no-places">No places available</p>
                            )}
                        </div>

                        {event.images && event.images.length > 0 && (
                            <img
                                src={`https://localhost:7287/${event.images[0]}`}
                                alt={event.title}
                                className="event-image"
                            />
                        )}

                        {isAdmin && (
                            <div className="admin-buttons">
                                <button
                                    onClick={() => navigate(`/event/update/${event.id}`)}
                                    className="edit-button"
                                >
                                    Edit
                                </button>
                                <button
                                    onClick={() => handleDelete(event.id)}
                                    className="delete-button"
                                >
                                    Delete
                                </button>
                            </div>
                        )}
                    </li>
                ))}
            </ul>

            <div className="pagination">
                <button
                    onClick={() => handlePageChange(filters.pageNo - 1)}
                    disabled={filters.pageNo === 1}
                    className="pagination-button"
                >
                    Prev
                </button>
                <span>Page {filters.pageNo}</span>
                <button
                    onClick={() => handlePageChange(filters.pageNo + 1)}
                    disabled={filters.pageNo * filters.pageSize >= totalCount}
                    className="pagination-button"
                >
                    Next
                </button>
            </div>
        </div>
    );
};

export default EventsPage;
