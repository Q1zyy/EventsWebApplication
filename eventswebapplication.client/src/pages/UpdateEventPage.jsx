import { useForm } from "react-hook-form";
import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import apiClient from './../api/ApiClient';
//import "./../css/UpdateEventPage.css";

const UpdateEventPage = () => {
    const { id } = useParams();
    const { register, handleSubmit, setValue, formState: { errors } } = useForm();
    const [categories, setCategories] = useState([]);
    const [image, setImage] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchEvent = async () => {
            try {
                const response = await apiClient.get(`/events/${id}`);
                const event = response.data;

                setValue("Title", event.title);
                setValue("Description", event.description);
                setValue("EventDateTime", event.eventDateTime);
                setValue("ParticipantsMaxCount", event.participantsMaxCount);
                setValue("Place", event.place);
                setValue("CategoryId", event.categoryId);
            } catch (error) {
                console.error("Error fetching event:", error);
            }
        };

        const fetchCategories = async () => {
            try {
                const response = await apiClient.get("/categories");
                setCategories(response.data);
            } catch (error) {
                console.error("Error fetching categories:", error);
            }
        };

        fetchEvent();
        fetchCategories();
    }, [id, setValue]);

    const onSubmit = async (data) => {
        const formData = new FormData();

        formData.append("Id", id);
        formData.append("Title", data.Title);
        formData.append("Description", data.Description);
        formData.append("EventDateTime", data.EventDateTime);
        formData.append("ParticipantsMaxCount", data.ParticipantsMaxCount);
        formData.append("Place", data.Place);
        formData.append("CategoryId", parseInt(data.CategoryId, 10));

        if (image) {
            formData.append("Image", image);
        }

        try {
            await apiClient.put("/events", formData, {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
            });
            navigate("/");
        } catch (error) {
            console.error("Error updating event:", error);
        }
    };

    const handleImageChange = (e) => {
        setImage(e.target.files[0]);
    };

    return (
        <div className="update-event-container">
            <h2 className="title">Edit Event</h2>
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
                    <select {...register("CategoryId", { required: true })}>
                        <option value="">Select a category</option>
                        {categories.map(category => (
                            <option key={category.id} value={category.id}>{category.title}</option>
                        ))}
                    </select>
                    {errors.CategoryId && <span className="error-message">Category is required</span>}
                </div>

                <div className="form-group">
                    <label>Image</label>
                    <input type="file" onChange={handleImageChange} />
                </div>

                <button type="submit" className="submit-button">Update Event</button>
            </form>
        </div>
    );
};

export default UpdateEventPage;
