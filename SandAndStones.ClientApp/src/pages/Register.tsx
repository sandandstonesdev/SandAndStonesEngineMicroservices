import { useRef, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { axiosInstance } from "../hooks/useAxios";

function Register() {
    const emailRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);
    const confirmedPasswordRef = useRef<HTMLInputElement>(null);
    const [error, setError] = useState("");


    const navigate = useNavigate();
    
    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const email = emailRef.current!.value;
        const password = passwordRef.current!.value;
        const confirmedPassword = confirmedPasswordRef.current!.value;

        if (email === "" || password === "" || confirmedPassword === "") {
            setError("Please fill in all fields.");
            return;
        }
        if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
            setError("Please enter a valid email address.");
            return;
        }
        if (password !== confirmedPassword) {
            setError("Passwords do not match.");
            return;
        }

        setError("");

        axiosInstance.post(`${import.meta.env.VITE_APP_BASE_URL}/userAuthorization/register`,
            {
                email: email,
                password: password,
            }
        ).then((response) => {
            console.log(response.data);
            if (response.status == 200) {
                setError("Successful register.");
                navigate('/', { replace: true });
            }
            else {
                setError("Error registering. Details: " + response.status + " " + response.statusText);
            }
        })
        .catch((error) => {
            console.error(error);
            setError("Error registering.");
        });
    };

    return (
        <div className="containerbox">
            <h3>Register</h3>

            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="email">Email:</label>
                </div><div>
                    <input
                        type="email"
                        id="new-email"
                        name="new-email"
                        ref={emailRef}
                    />
                </div>
                <div>
                    <label htmlFor="password">Password:</label></div><div>
                    <input
                        type="password"
                        id="new-password"
                        name="new-password"
                        ref={passwordRef}
                    />
                </div>
                <div>
                    <label htmlFor="confirmPassword">Confirm Password:</label></div><div>
                    <input
                        type="password"
                        id="confirm-new-password"
                        name="confirm-new-password"
                        ref={confirmedPasswordRef}
                    />
                </div>
                <div>
                    <button type="submit">Register</button>
                </div>
            </form>
            <div>
                <Link to="/login">Go to Login</Link>
            </div>
            {error && <p className="error">{error}</p>}
        </div>
    );
}

export default Register;