import { useRef, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { axiosInstance } from "../hooks/useAxios";
import { formatZodIssue } from "../hooks/useZodFormatIssue";
import axios, { isAxiosError } from "axios";
import { z, ZodError } from "zod"
import pkg from 'node-forge';
import { useDispatch } from "react-redux";
import { setAuthenticated } from "../redux/store/AuthSlice"

const loginUserSchema = z.object({
    email: z.string().email(),
    passwordToHash: z.string()
});

type ILoginUserCredentials = z.infer<typeof loginUserSchema>;

function Login() {
    const dispatch = useDispatch();
    
    const emailRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);
    
    console.log("Environment mode:" + import.meta.env.MODE);

    const [error, setError] = useState<string>("");
    
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        try {
            const userCredentials = await loginUserSchema.parseAsync({
                email: emailRef.current!.value,
                passwordToHash: passwordRef.current!.value
            } as ILoginUserCredentials);

            const salt = `${import.meta.env.VITE_TEST_SALT}`;;
            const hashObject = pkg.md.sha512.create();
            hashObject.update(salt + userCredentials.passwordToHash);
            const hashedPassword = pkg.util.encode64(hashObject.digest().data);

            setError("");

            const formData = axios.toFormData({
                email: userCredentials.email,
                password: hashedPassword
            })

            const response = await axiosInstance.post(
                `${import.meta.env.VITE_APP_BASE_URL}/gateway-api/userAuthorization/login`,
                formData
            );
            console.log(response.data);
            setError("Successful Login.");
            dispatch(setAuthenticated());
            navigate('/', { replace: true });
        }
        catch (err: unknown) {
            if (isAxiosError(err)) {
                if (err.response) {
                    const email = emailRef.current!.value;

                    setError(`Error while LoggingIn: Email: ${email} Status: ${err.response.status} ${err.response.data}`);
                }
                else {
                    setError(`Error LoggingIn: ${err}`);
                    console.error(err);
                }
            }
            else if (err instanceof ZodError) {
                if (err.issues.length) {
                    const currentIssue = err.issues[0]

                    const errorIssue = formatZodIssue(currentIssue)
                    setError(`Validation Error while LoggingIn: ${errorIssue}`);
                    console.error(err);
                }
            }
        }
    }

    
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
                            autoFocus
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
                        autoFocus
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