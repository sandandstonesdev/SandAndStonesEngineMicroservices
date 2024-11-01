import { useEffect, useRef, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { axiosInstance } from "../hooks/useAxios";
import axios, { isAxiosError } from "axios";
import { AuthData } from "../types/AuthData";
import { useAuth } from "../context/AuthProvider";
import pkg from 'node-forge';

function Login() {
    const emailRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);
    const { logUserIn } = useAuth() as AuthData;


    console.log("Environment mode:" + import.meta.env.MODE);

    const [error, setError] = useState<string>("");
    
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const email = emailRef.current!.value
        const passwordToHash = passwordRef.current!.value;

        if (email === "" || passwordToHash === "") {
            setError("Please fill in all fields.");
            return;
        }

        const salt = `${import.meta.env.VITE_TEST_SALT}`;;
        const hashObject = pkg.md.sha512.create();
        hashObject.update(salt + passwordToHash);
        const hashedPassword = pkg.util.encode64(hashObject.digest().data);
        
        setError("");

        try {
            const formData = axios.toFormData({
                email: email,
                password: hashedPassword
            })

            const response = await axiosInstance.post(
                `${import.meta.env.VITE_APP_BASE_URL}/userAuthorization/login`,
                formData
            );
            console.log(response.data);
            setError("Successful Login.");
            logUserIn();
            navigate('/', { replace: true });
        }
        catch (err: unknown) {
            if (isAxiosError(err)) {
                if (err?.response) {
                    const email = emailRef.current!.value;

                    setError(`Error Logging In: Email: ${email} Status: ${err.response.status} ${err.response.statusText}`);
                }
                else {
                    setError(`Error Logging in: ${err}`);
                    console.error(err);
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