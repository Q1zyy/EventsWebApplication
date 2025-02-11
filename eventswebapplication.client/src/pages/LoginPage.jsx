import { useState } from "react";
import axios from "axios";
import "./../css/Login.css";

const LoginPage = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const handleLogin = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post("https://localhost:7287/api/auth/login", {
                email: email,
                password: password,
            });

            const { accessToken, refreshToken, user } = response.data;

            localStorage.setItem("accessToken", accessToken);
            localStorage.setItem("refreshToken", refreshToken);
            localStorage.setItem("user", JSON.stringify(user));

            alert("Completed!");
        } catch (err) {
            setError(err.response?.data?.message || "Authorization error");
        }
    };

    return (
        <div className="login-container">
            <h2>Login</h2>
            {error && <p className="error">{error}</p>}
            <form onSubmit={handleLogin}>
                <label>Email:</label>
                <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />

                <label>Password:</label>
                <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />

                <button type="submit">Login</button>
            </form>
        </div>
    );
};

export default LoginPage;
