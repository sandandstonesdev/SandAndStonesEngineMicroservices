import { useEffect, useRef, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { axiosInstance } from "../hooks/useAxios";
import axios from "axios";
import { useAuth } from "../context/AuthProvider";

interface AuthObjectSet {
    setContextToken: (newToken: string) => void
}

function Login() {
    const { setContextToken } = useAuth() as AuthObjectSet;
    const emailRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);

    console.log("Environment mode:" + import.meta.env.MODE);

    const [error, setError] = useState<string>("");
    
    const navigate = useNavigate();

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const email = emailRef.current!.value
        const password = passwordRef.current!.value;

        if (email === "" || password === "") {
            setError("Please fill in all fields.");
            return;
        }

        setError("");
            
        const formData = axios.toFormData({
            email: emailRef.current!.value,
            password: passwordRef.current!.value
        })

        axiosInstance.post(
            `${import.meta.env.VITE_APP_BASE_URL}/userAuthorization/login`,
            formData
        )
        .then((response) => {
            console.log(response.data);
            if (response.status == 200) {
                setContextToken(response.data.accessToken);
                setError("Successful Login.");
                navigate('/', { replace: true });
            }
            else {
                const email = emailRef.current!.value;
                const password = passwordRef.current!.value;
                setError("Error Logging In. " + email + " " + password + " Status: " + response.status + " " + response.statusText);
            }
        })
        .catch((error) => {
            console.error(error);
            setError("Error Logging in.");
        });
    }

    useEffect(() => {
        emailRef.current!.focus();
        passwordRef.current!.focus();
    });

    return (
        <>
        <div className="containerbox">
            <h3>Sign In</h3>
                <form onSubmit={handleSubmit}>
                    <div>
                    <label className="forminput" htmlFor="email">Email:</label>
                    <input type="email"
                            id="current-email"
                            name="current-email"
                        ref={emailRef}
                        placeholder="mail@domain.com"
                    />
                    </div>
                    <div>
                    <label className="password">Password:</label>
                    <input
                        type="password"
                        id="current-password"
                        name="current-password"
                        ref={passwordRef}
                        />
                    </div>
                    <div>
                        <button type="submit">Login</button>
                    </div>
                </form>
                <div>
                    <Link to="/register">Go to Register</Link>
                </div>
            {error && <p className="submit">{error}</p>}
            </div>
        </>
    );
}

export default Login;