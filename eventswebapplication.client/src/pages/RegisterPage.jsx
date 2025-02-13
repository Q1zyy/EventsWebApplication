import { useState } from "react";
import { useNavigate } from "react-router-dom";
import apiClient from "./../api/ApiClient";


const RegisterPage = () => {
    const [formData, setFormData] = useState({
        name: "",
        surname: "",
        birthday: "",
        email: "",
        password: "",
        confirmPassword: "",
    });

    const [error, setError] = useState(null);
    const navigate = useNavigate(); 

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);

        if (!formData.email || !formData.password || !formData.confirmPassword) {
            setError("Fill all fields!");
            return;
        }

        if (formData.password !== formData.confirmPassword) {
            setError("Passwords are not equal!");
            return;
        }

        try {
            const response = await apiClient.post(
                "/auth/register",
                JSON.stringify(formData), 
                {
                    headers: {
                        "Content-Type": "application/json", 
                    },
                }
            );
            console.log("Completed:", response.data);

            navigate("/login");
        } catch (err) {
            console.log();
            if (err.response?.data?.errors["Birthday"]) {
                setError(err.response?.data?.errors["Birthday"][0] || "Regitration failed");
            }
            if (err.response?.data?.errors["Password"]) {
                setError(err.response?.data?.errors["Password"][0] || "Regitration failed");
            }
        }
    };

    return (
        <div style={styles.container}>
            <h2>Register</h2>
            {error && <p style={styles.error}>{error}</p>}
            <form onSubmit={handleSubmit} style={styles.form}>
                <input type="text" name="name" placeholder="Name" value={formData.name} onChange={handleChange} required />
                <input type="text" name="surname" placeholder="Surname" value={formData.surname} onChange={handleChange} required />
                <input type="email" name="email" placeholder="Email" value={formData.email} onChange={handleChange} required />
                <input type="date" name="birthday" value={formData.birthday} onChange={handleChange} required />
                <input type="password" name="password" placeholder="Password" value={formData.password} onChange={handleChange} required />
                <input type="password" name="confirmPassword" placeholder="Confirm password" value={formData.confirmPassword} onChange={handleChange} required />
                <button type="submit">Register</button>
            </form>
        </div>
    );
};

const styles = {
    container: {
        width: "300px",
        margin: "50px auto",
        padding: "20px",
        border: "1px solid #ccc",
        borderRadius: "5px",
        textAlign: "center",
    },
    form: {
        display: "flex",
        flexDirection: "column",
        gap: "10px",
    },
    error: {
        color: "red",
        fontSize: "14px",
    },
};

export default RegisterPage;
