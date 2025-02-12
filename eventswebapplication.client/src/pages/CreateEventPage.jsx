import { useForm } from "react-hook-form";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "./../css/CreateEvent.css";
import apiClient from "./../api/ApiClient";


const CreateEventPage = () => {
    const { register, handleSubmit, formState: { errors } } = useForm();
    const [categories, setCategories] = useState([]);
    const [image, setImage] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const response = await apiClient.get("https://localhost:7287/api/categories");
                setCategories(response.data);
            } catch (error) {
                console.error("Error fetching categories:", error);
            }
        };
        fetchCategories();
    }, []);

    const onSubmit = async (data) => {
        const formData = new FormData();

        formData.append("Title", data.Title);
        formData.append("Description", data.Description);
        formData.append("EventDateTime", data.EventDateTime);
        formData.append("ParticipantsMaxCount", data.ParticipantsMaxCount);
        formData.append("Place", data.Place);
        formData.append("CategoryId", parseInt(data.Category, 10));

        if (image) {
            formData.append("Image", image);
        }

        try {
            const response = await apiClient.post("https://localhost:7287/api/events", formData, {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
            });

            if (response.status === 201 || response.status === 200) {
                navigate("/");
            } else {
                console.error("Failed to create event");
            }
        } catch (error) {
            console.error("Error submitting form", error);
        }
    };

    const handleImageChange = (e) => {
        setImage(e.target.files[0]);
    };

    return (
        <div className="create-event-container">
            <h2 className="title">Create Event</h2>
            <form onSubmit={handleSubmit(onSubmit)} className="event-form">
                <div className="form-group">
                    <label>Title</label>
                    <input {...register("Title", { required: true })} />
                    {errors.Title && <span className="error-message">Title is required</span>}
                </div>

                <div className="form-group">
                    <label>Description</label>
                    <textarea {...register("Description")} />
                </div>

                <div className="form-group">
                    <label>Date & Time</label>
                    <input type="datetime-local" {...register("EventDateTime", { required: true })} />
                    {errors.EventDateTime && <span className="error-message">Event date & time is required</span>}
                </div>

                <div className="form-group">
                    <label>Max Participants</label>
                    <input type="number" {...register("ParticipantsMaxCount", { required: true, min: 1 })} />
                    {errors.ParticipantsMaxCount && <span className="error-message">Enter a valid number</span>}
                </div>

                <div className="form-group">
                    <label>Place</label>
                    <input {...register("Place")} />
                </div>

                <div className="form-group">
                    <label>Category</label>
                    <select {...register("Category", { required: true })}>
                        <option value="">Select a category</option>
                        {categories.map(category => (
                            <option key={category.id} value={category.id}>{category.title}</option>
                        ))}
                    </select>
                    {errors.Category && <span className="error-message">Category is required</span>}
                </div>

                <div className="form-group">
                    <label>Image</label>
                    <input type="file" onChange={handleImageChange} />
                </div>

                <button type="submit" className="submit-button">Create Event</button>
            </form>
        </div>
    );
};

export default CreateEventPage;
