import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../components/AuthProvider";

function Login() {
    const auth = useAuth();
    const emailRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);
    
    const [error, setError] = useState<string>("");
    
    const navigate = useNavigate();

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (emailRef.current!.value !== "" && passwordRef.current!.value !== "")
        {
            setError("");

            fetch("/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    email: emailRef.current!.value,
                    password: passwordRef.current!.value,
                }),
            }).then((data) => {
                console.log(data);
                if (data.ok) {
                    auth.addUser({
                        email: emailRef.current!.value,
                        password: passwordRef.current!.value,
                    });
                    setError("Successful Login.");
                    navigate('/', { replace: true });
                }
                else
                    setError("Error Logging In.");

            })
                .catch((error) => {
                    console.error(error);
                    setError("Error Logging in.");
                });
        }
        else {
            setError("Please fill in all fields.");
        }
    }

    useEffect(() => {
        emailRef.current!.focus();
        passwordRef.current!.focus();
    });

    return (
        <div className="containerbox">
            <h3>Sign In</h3>
            <form onSubmit={handleSubmit}>
                    <label className="forminput" htmlFor="email">Email:</label>
                    <input type="email"
                        id="email"
                        ref={emailRef}
                        placeholder="mail@domain.com"
                    />
                    <label className="password">Password:</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        ref={passwordRef}
                    />
                    <button type="submit">Login</button>
            </form>
            {error && <p className="submit">{error}</p>}
        </div>
    );
}

export default Login;