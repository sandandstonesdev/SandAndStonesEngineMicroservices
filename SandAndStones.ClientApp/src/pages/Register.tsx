import { useEffect, useRef, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { axiosInstance } from "../hooks/useAxios";
import axios, { isAxiosError } from "axios";

function Register() {
    const emailRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);
    const confirmedPasswordRef = useRef<HTMLInputElement>(null);
    const [error, setError] = useState("");

    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
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

        try {

            const formData = axios.toFormData({
                userName: email,
                email: email,
                password: password,
                confirmedPassword: confirmedPassword
            })

            await axiosInstance.post(
                `${import.meta.env.VITE_APP_BASE_URL}/userAuthorization/register`,
                formData
            );
            
            setError("Successful register.");
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
    };

    useEffect(() => {
        emailRef.current!.focus();
        passwordRef.current!.focus();
        confirmedPasswordRef.current!.focus();
    }, []);

    return (
        <div className="containerbox">
            <h3>Register</h3>

            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="new-email">Email:</label>
                </div><div>
                    <input
                        type="email"
                        id="new-email"
                        name="new-email"
                        ref={emailRef}
                        placeholder="mail@domain.com"
                    />
                </div>
                <div>
                    <label htmlFor="new-password">Password:</label></div><div>
                    <input
                        type="password"
                        id="new-password"
                        name="new-password"
                        ref={passwordRef}
                        placeholder="*****"
                    />
                </div>
                <div>
                    <label htmlFor="confirm-new-password">Confirm Password:</label></div><div>
                    <input
                        type="password"
                        id="confirm-new-password"
                        name="confirm-new-password"
                        ref={confirmedPasswordRef}
                        placeholder="*****"
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