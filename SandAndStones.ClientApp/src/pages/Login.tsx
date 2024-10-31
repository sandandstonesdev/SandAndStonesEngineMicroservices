import { useEffect, useRef, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { axiosInstance } from "../hooks/useAxios";
import axios, { isAxiosError } from "axios";
import { useAuth } from "../context/AuthProvider";

interface AuthObjectSet {
    setContextToken: (newToken: string | null) => void
}

function Login() {
    const { setContextToken } = useAuth() as AuthObjectSet;
    const emailRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);

    console.log("Environment mode:" + import.meta.env.MODE);

    const [error, setError] = useState<string>("");
    
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const email = emailRef.current!.value
        const password = passwordRef.current!.value;

        if (email === "" || password === "") {
            setError("Please fill in all fields.");
            return;
        }

        setError("");

        try {
            const formData = axios.toFormData({
                email: email,
                password: password
            })

            const response = await axiosInstance.post(
                `${import.meta.env.VITE_APP_BASE_URL}/userAuthorization/login`,
                formData
            );
            console.log(response.data);
            setContextToken(response.data.accessToken);
            setError("Successful Login.");
            navigate('/', { replace: true });
        }
        catch (error: unknown) {
            if (isAxiosError(error)) {
                if (error?.response) {
                    const email = emailRef.current!.value;

                    setError(`Error Logging In: Email: ${email} Status: ${error.response.status} ${error.response.statusText}`);
                }
                else {
                    setError(`Error Logging in: ${error}`);
                    console.error(error);
                }
            }
        }
    }

    useEffect(() => {
        emailRef.current!.focus();
        passwordRef.current!.focus();
    }, []);

    return (
        <>
        <div className="containerbox">
            <h3>Sign In</h3>
                <form onSubmit={handleSubmit}>
                    <div>
                        <label htmlFor="current-email">Email:</label>
                    <input type="email"
                            id="current-email"
                            name="current-email"
                        ref={emailRef}
                        placeholder="mail@domain.com"
                    />
                    </div>
                    <div>
                    <label htmlFor="current-password">Password:</label>
                    <input
                        type="password"
                        id="current-password"
                        name="current-password"
                        ref={passwordRef}
                        placeholder="*****"
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