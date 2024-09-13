import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../components/AuthProvider";

function Login() {
    const auth = useAuth();

    const [input, setInput] = useState({
        email: "",
        password: "",
    });
    const [error, setError] = useState<string>("");
    
    const navigate = useNavigate();

    const handleInput = (e: any) => {
        e.preventDefault();
        const { name, value } = e.target;
        setInput((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (input.email !== "" && input.password !== "") {
            setError("");

            fetch("/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    email: input.email,
                    password: input.password,
                }),
            }).then((data) => {
                console.log(data);
                if (data.ok) {
                    auth.addUser({
                        email: input.email,
                        password: input.password,
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
        
    return (
        <div className="containerbox">
            <h3>Sign In</h3>
            <form onSubmit={handleSubmit}>
                <div>
                    <label className="forminput" htmlFor="email">Email:</label>
                </div>
                <div>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        placeholder="mail@domain.com"
                        onChange={handleInput}
                    />
                </div>
                <div>
                    <label htmlFor="password">Password:</label>
                </div>
                <div>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        onChange={handleInput}
                    />
                </div>
                <div>
                    <button type="submit">Login</button>
                </div>
            </form>
            {error && <p className="submit">{error}</p>}
        </div>
    );
}

export default Login;