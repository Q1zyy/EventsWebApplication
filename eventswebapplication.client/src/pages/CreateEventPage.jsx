import { useForm } from "react-hook-form";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const CreateEventPage = () => {
    const { register, handleSubmit, formState: { errors } } = useForm();
    const [categories, setCategories] = useState([]);
    const [image, setImage] = useState(null); 
    const navigate = useNavigate();

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const response = await axios.get("https://localhost:7287/api/categories");
                console.log("Fetched categories:", response.data);
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
            const response = await axios.post("https://localhost:7287/api/events", formData, {
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
        <div className="max-w-lg mx-auto mt-10 p-6 bg-white shadow-md rounded-lg">
            <h2 className="text-2xl font-semibold mb-4">Create Event</h2>
            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="mb-4">
                    <label className="block font-medium">Title</label>
                    <input className="w-full border p-2 rounded" {...register("Title", { required: true })} />
                    {errors.Title && <span className="text-red-500">Title is required</span>}
                </div>

                <div className="mb-4">
                    <label className="block font-medium">Description</label>
                    <textarea className="w-full border p-2 rounded" {...register("Description")} />
                </div>

                <div className="mb-4">
                    <label className="block font-medium">Date & Time</label>
                    <input type="datetime-local" className="w-full border p-2 rounded" {...register("EventDateTime", { required: true })} />
                    {errors.EventDateTime && <span className="text-red-500">Event date & time is required</span>}
                </div>

                <div className="mb-4">
                    <label className="block font-medium">Max Participants</label>
                    <input type="number" className="w-full border p-2 rounded" {...register("ParticipantsMaxCount", { required: true, min: 1 })} />
                    {errors.ParticipantsMaxCount && <span className="text-red-500">Enter a valid number</span>}
                </div>

                <div className="mb-4">
                    <label className="block font-medium">Place</label>
                    <input className="w-full border p-2 rounded" {...register("Place")} />
                </div>

                <div className="mb-4">
                    <label className="block font-medium">Category</label>
                    <select className="w-full border p-2 rounded" {...register("Category", { required: true })}>
                        <option value="">Select a category</option>
                        {categories.map(category => (
                            <option key={category.id} value={category.id}>{category.title}</option>
                        ))}
                    </select>
                    {errors.Category && <span className="text-red-500">Category is required</span>}
                </div>

                <div className="mb-4">
                    <label className="block font-medium">Image</label>
                    <input type="file" className="w-full border p-2 rounded" onChange={handleImageChange} />
                </div>

                <button type="submit" className="w-full bg-blue-500 text-white p-2 rounded">Create Event</button>
            </form>
        </div>
    );
};

export default CreateEventPage;
