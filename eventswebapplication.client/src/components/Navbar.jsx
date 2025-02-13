import { Link, useNavigate } from "react-router-dom";
import './../css/Navbar.css'; 


const Navbar = () => {

    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("user");
        navigate("/login"); 
    };

    const isAuthenticated = localStorage.getItem("accessToken") !== null;
    const user = JSON.parse(localStorage.getItem("user"));


    return (
        <nav>
            <div>
                <Link to="/" className="hover:underline">Home</Link>
                <Link to="/about" className="hover:underline">About</Link>
                {!isAuthenticated ? (
                        <>
                        <Link to="/login" className="hover:underline">Login</Link>
                        <Link to="/register" className="hover:underline">Register</Link>
                        </>
                    ) : (<></>)
                }
                <Link to="/events" className="hover:underline">Events</Link>
                <Link to="/my-events" className="hover:underline">MyEvents</Link>
                {isAuthenticated ? (
                    <>
                        <p style={{ color: 'black' }}>{user.email}</p>
                        <button onClick={handleLogout}>Logout</button>
                    </>
                   ) : (<></>)
                }
            </div>
        </nav>
    );
};

export default Navbar;
