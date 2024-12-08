import { useEffect, useRef, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { axiosInstance } from "../hooks/useAxios";
import axios, { isAxiosError } from "axios";
import { z, ZodError } from "zod"
import pkg from 'node-forge';
import { formatZodIssue } from "../hooks/useZodFormatIssue";

const registerUserSchema = z.object({
        email: z.string().email(),
    password: z.string().min(8).regex(
        new RegExp(/^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$/), {
                message: 'Your password is not valid',
            }),
    confirmedPassword: z.string().min(8)
    }).refine((data) => data.password === data.confirmedPassword, {
            message: "Passwords don't match",
            path: ["confirmedPassword"]
    });

type IRegisterUserSchema = z.infer<typeof registerUserSchema>;

function Register() {
    const emailRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);
    const confirmedPasswordRef = useRef<HTMLInputElement>(null);
    const [error, setError] = useState("");

    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        try {
            const registerUserCredentials = await registerUserSchema.parseAsync({
                email: emailRef.current!.value,
                password: passwordRef.current!.value,
                confirmedPassword: confirmedPasswordRef.current!.value
            } as IRegisterUserSchema);

            const salt = `${import.meta.env.VITE_TEST_SALT}`;
            const hashObject = pkg.md.sha512.create();

            hashObject.update(salt + registerUserCredentials.password);
            const hashedPassword = pkg.util.encode64(hashObject.digest().data);

            hashObject.update(salt + registerUserCredentials.confirmedPassword);
            const hashedConfirmedPassword = pkg.util.encode64(hashObject.digest().data);

            setError("");

            const formData = axios.toFormData({
                userName: registerUserCredentials.email,
                email: registerUserCredentials.email,
                password: hashedPassword,
                confirmedPassword: hashedConfirmedPassword
            })

            await axiosInstance.post(
                `${import.meta.env.VITE_APP_BASE_URL}/gateway-api/register`,
                formData
            );
            
            setError("Successful register.");
            navigate('/', { replace: true });
        }
        catch (err: unknown) {
            if (isAxiosError(err)) {
                if (err.response) {
                    const email = emailRef.current!.value;
                    setError(`Error while Registering: Email: ${email} Status: ${err.response.status} ${err.response.statusText}`);
                }
                else {
                    setError(`Error while Registering: ${err.message}`);
                    console.error(err);
                }
            }
            else if (err instanceof ZodError)
            {
                if (err.issues.length) {
                    const currentIssue = err.issues[0]

                    const errorIssue = formatZodIssue(currentIssue)
                    setError(`Validation Error while Registering: ${errorIssue}`);
                    console.error(err);
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
                        autoFocus
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
                        autoFocus
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
                        autoFocus
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