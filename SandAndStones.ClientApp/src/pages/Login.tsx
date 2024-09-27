import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../components/AuthProvider";

function Login() {
    const auth = useAuth();
    const emailRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);
    
    const [error, setError] = useState<string>("");
    
    const navigate = useNavigate();

    const navigateRegister = () => {
        navigate('/register', { replace: true });
    }

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
                else {
                    const email = emailRef.current!.value;
                    const password = passwordRef.current!.value;
                    setError("Error Logging In. " + email + " " + password);
                }
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
        <>
        <div className="containerbox">
            <h3>Sign In</h3>
                <form onSubmit={handleSubmit}>
                    <div>
                    <label className="forminput" htmlFor="email">Email:</label>
                    <input type="email"
                        id="email"
                        ref={emailRef}
                        placeholder="mail@domain.com"
                    />
                    </div>
                    <div>
                    <label className="password">Password:</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        ref={passwordRef}
                        />
                    </div>
                    <div>
                        <button type="submit">Login</button>
                    </div>
                    <div>
                        <button onClick={navigateRegister}>Register</button>
                    </div>
            </form>
            {error && <p className="submit">{error}</p>}
            </div>
        </>
    );
}

export default Login;